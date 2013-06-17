using System.Diagnostics;
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ViAppleGrab.Properties;
using ViToolkit.PSMoveSharp;
using ViAppleGrab.Collections;
using System.Threading;
using System;
using System.Drawing;

namespace ViAppleGrab
{
    //Clarify Point as the XNA framework point and not the System.Drawing point
    using Point = Microsoft.Xna.Framework.Point;

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ViAppleGrabInput : Microsoft.Xna.Framework.GameComponent
    {        
        #region GENERAL SETTINGS AND FIELDS

        private static Settings _settings = Settings.Default;
        private ViAppleGrabLogic _logic;
        private PSMoveClientThreadedRead _moveClient = null;
        private uint _processedPacketIndex = 0;
        public static bool GameHasFocus = true;

        #endregion

        #region CONTROLLER FIELDS AND PROPERTIES

        private bool _isMoveUpdated = false;
        public bool IsMoveUpdated { get { return _isMoveUpdated; } }

        //_cs = Controllers
        public ControllerCollection Controllers;
        //private Controller[] _cs;

        #endregion

        #region CURRENT AND PREVIOUS STATE PROPERTIES

        private KeyboardState _currentKeyboardState = Keyboard.GetState();
        public KeyboardState CurrentKeyboardState
        {
            get { return _currentKeyboardState; }
        }

        private KeyboardState _previousKeyboardState = Keyboard.GetState();
        public KeyboardState PreviousKeyBoardState
        {
            get { return _previousKeyboardState; }
        }

        private PSMoveSharpState _currentMoveState = null;
        public PSMoveSharpState CurrentMoveState
        {
            get
            {
                //Set the updated flag so that client classes can check to see if this has 
                //been updated since last checked...
                _isMoveUpdated = false;

                return _currentMoveState;
            }
        }

        PSMoveSharpState dummy_state = new PSMoveSharpState();
        PSMoveSharpCameraFrameState _frame;
        public Image _currentFrame;

        private ushort[] _previousMoveButtons = new ushort[_settings.MAX_CONTROLLERS];

        #endregion

        /// <summary>
        /// Default constructor. Enables this component and sets it to update first.
        /// </summary>
        /// <param name="game">The current game instance</param>
        public ViAppleGrabInput(Game game)
            : base(game)
        {
            //Initialize the controllers
            Controllers = new ControllerCollection(_settings.MAX_CONTROLLERS);

            //This component should be the first component to update (updates before the game logic is processed)
            Enabled = true;
            UpdateOrder = 1;

            Debug.WriteLine("Input component created");
        }

        ~ViAppleGrabInput()
        {
            Controllers.StopRumbles();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            //Get a reference to the game logic component
            _logic = (ViAppleGrabLogic)Game.Components[(int)ComponentIndex.Logic];

            //Subscribe to events within other game components
            _logic.GameShutDownEvent += new ViAppleGrabLogic.GameShutDownEventHandler(GameShutDownEvent);

            //Connect to the move.me server
            if (!_settings.USE_KEYBOARD_ONLY)
            {
                _moveClient = new PSMoveClientThreadedRead();

                try
                {
                    _moveClient.Connect(Dns.GetHostAddresses(_settings.IP_ADDRESS)[0].ToString(), _settings.PORT);
                    _moveClient.StartThread();
                    Controllers.Init(_moveClient);

                    if (_settings.SHOW_CAMERA)
                        _moveClient.CameraFrameResume();
                }
                catch(CouldNotConnectException ex)
                {
                    Debug.WriteLine(ex.Message);

                    //Turn off the Move capability
                    Settings.Default.USE_KEYBOARD_ONLY = true;
                    _moveClient = null;
                    string text = "Check the network connections between the PS and the computer and try again...";
                    
                    GameHasFocus = true;

                    _logic.ReportSystemError(ex.Message, text);
                    
                    return;
                }

                PSMoveSharpState state = _moveClient.GetLatestState();
                _currentMoveState = state;
                _currentFrame = _moveClient.GetLatestCameraFrameState().GetCameraFrameAndState(ref dummy_state);

                for (int i = 0; i < _settings.MAX_CONTROLLERS; i++)
                    _previousMoveButtons[i] = _currentMoveState.gemStates[i].pad.digitalbuttons;

                _processedPacketIndex = state.packet_index;
            }

            base.Initialize();

            Debug.WriteLine("Input component initialized");
        }

        public void TurnOnCamera()
        {
            Settings.Default.SHOW_CAMERA = true;

            if(_moveClient != null)
                _moveClient.CameraFrameResume();
        }

        public void TurnOffCamera()
        {
            Settings.Default.SHOW_CAMERA = false;

            if (_moveClient != null)
                _moveClient.CameraFramePause();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //Only update if the user info has been entered
            if (GameHasFocus)
                _updateKeyboardState();
            
            _checkForMenuInput();

            if (_settings.USE_KEYBOARD_ONLY)
            {
                //Process the controller info from the keyboard only
                if (GameHasFocus)
                    _handleKeyboardInput();
            }
            else
            {
                //Process the controller info from the move controllers
                _updateMoveState();
            }

            base.Update(gameTime);
        }

        private void _updateKeyboardState()
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();
        }

        /// <summary>
        /// Gathers the most recent state information from the Move.Me server.
        /// If an updated state is not yet available, this will return false.
        /// </summary>
        /// <returns>True if the state has been updated</returns>
        private bool _updateMoveState()
        {
            //Process the controller info from the move controllers
            uint index = _moveClient.GetLatestStateIndex();

            if (_processedPacketIndex != index)
            {
                _isMoveUpdated = true;

                _processedPacketIndex = index;

                for (int i = 0; i < _settings.MAX_CONTROLLERS; i++)
                    _previousMoveButtons[i] = _currentMoveState.gemStates[i].pad.digitalbuttons;

                _currentMoveState = _moveClient.GetLatestState();

                if (_settings.SHOW_CAMERA)
                {
                    _frame = _moveClient.GetLatestCameraFrameState();
                    _frame.camera_frame_state_rwl.AcquireReaderLock(-1);
                    _currentFrame = _frame.GetCameraFrameAndState(ref dummy_state);
                    _frame.camera_frame_state_rwl.ReleaseReaderLock();
                }

                _handleMoveInput();
                
                return true;
            }

            return false;
        }

        #region INPUT PARSING
        /// <summary>
        /// This function checks the keyboard for any input that the user may
        /// have given for dealing with situations such as pausing the game
        /// or ending the game
        /// </summary>
        private void _checkForMenuInput()
        {
            //[H] has been pressed. Toggle the output of the game to the screen
            if (_justPressed(Keys.H))
            {
                _settings.HIDE_OUTPUT = !_settings.HIDE_OUTPUT;
            }

            //[ESC] has been pressed: End the game...
            if (_justPressed(Keys.Escape)
                || _justPressed(PSMoveSharpConstants.ctrlSelect, ControllerIndex.RightHand)
                || _justPressed(PSMoveSharpConstants.ctrlSelect, ControllerIndex.LeftHand))
            {
                if(_logic.State != GameState.GameOver)
                    _logic.EndGame();

                _logic.ShutDown();
            }

            //[HOME] has been pressed: Start the calibration sequence...
            else if (_justPressed(Keys.Home)
                || _justPressed(PSMoveSharpConstants.ctrlStart, ControllerIndex.RightHand)
                || _justPressed(PSMoveSharpConstants.ctrlStart, ControllerIndex.LeftHand))
            {
                _logic.PauseGame();
                //_logic.PerformCalibration();
                _logic.RestartGame();
            }

            //[SPACE] has been pressed
            else if (_justPressed(Keys.Space)
                || _justPressed(PSMoveSharpConstants.ctrlTick, ControllerIndex.RightHand)
                || _justPressed(PSMoveSharpConstants.ctrlTick, ControllerIndex.LeftHand))
            {
                //If the game is over or we have encountered a fatal error, then
                //  shut down the program
                if (_logic.State == GameState.GameOver || _logic.State == GameState.Error)
                {
                    _logic.ShutDown();
                    return;
                }

                //Else, If the game is not paused then pause it
                else if (_logic.State == GameState.Active)
                {
                    if (!_settings.USE_KEYBOARD_ONLY && _moveClient != null)
                    {
                        if (_justPressed(Keys.Space)) _moveClient.Pause();
                        Controllers[ControllerIndex.RightHand].Target.Pause();
                        Controllers[ControllerIndex.LeftHand].Target.Pause();
                    }

                    _logic.PauseGame();
                }

                //Otherwise the game must be paused so unpause it
                else if (_logic.State == GameState.Paused)
                {
                    if (!_settings.USE_KEYBOARD_ONLY && _moveClient != null)
                    {
                        if (_moveClient.IsPaused) _moveClient.Resume();
                        Controllers[ControllerIndex.RightHand].Target.Resume();
                        Controllers[ControllerIndex.LeftHand].Target.Resume();
                    }

                    _logic.ResumeGame();
                }

                else if (_logic.State == GameState.Instructions)
                {
                    //Stop the instructions
                    _logic.StopInstructions();

                    //Return to the main menu
                    ViAppleGrabGame.MainMenu();
                }
            }

            //[TRIANGLE] has been pressed
            else if (_justPressed(PSMoveSharpConstants.ctrlTriangle, ControllerIndex.RightHand)
                || _justPressed(PSMoveSharpConstants.ctrlTriangle, ControllerIndex.LeftHand))
            {
                //If the game has already been started, restart it
                if (_logic.State == GameState.Active || _logic.State == GameState.Paused)
                {
                    //Toggle the control type and restart the game
                    _settings.CONTROL_TYPE = (_settings.CONTROL_TYPE + 1) % 2;

                    Controllers.StopRumbles();
                    Controllers.SwitchControlType();

                    _logic.RestartGame();
                }
            }

            //[CIRCLE] has been pressed
            else if (_justPressed(PSMoveSharpConstants.ctrlCircle, ControllerIndex.RightHand)
                || _justPressed(PSMoveSharpConstants.ctrlCircle, ControllerIndex.LeftHand))
            {
                //If the game has already been started, restart it
                if (_logic.State == GameState.Active || _logic.State == GameState.Paused)
                {
                    //Toggle the control type and restart the game
                    Controllers.StopRumbles();

                    _logic.RestartGame();
                }
            }
        }

        /// <summary>
        /// This function handles the keyboard input to immitate move input. 
        /// This includes storing the controller locations, state of the 
        /// controllers (searching, target lock, waiting for new apple), and 
        /// recognizing any controller gestures.
        /// </summary>
        private void _handleKeyboardInput()
        {
            //Controller 1: Left/Right -> J/L, Up/Down -> I/K, Trigger -> H
            _handleKBControllerInput(ControllerIndex.RightHand, Keys.I, Keys.K, Keys.J, Keys.L, Keys.H);
                        
            //Controller 2: Left/Right -> A/D, Up/Down -> W/S, Trigger -> F
            _handleKBControllerInput(ControllerIndex.LeftHand, Keys.W, Keys.S, Keys.A, Keys.D, Keys.F);
        }

        private void _handleKBControllerInput(ControllerIndex ci, Keys up, Keys down, Keys left, Keys right, Keys trig)
        {
            //Only up or down can be pressed at one time, but both up or down
            //  and left or right can be pressed at the same time
            Controller c = Controllers[ci];

            if (_justPressed(up) || _stillPressed(up))
            {
                c.y -= _settings.KEYBOARD_CONTROLLER_INCREMENT;
            }
            else if (_justPressed(down) || _stillPressed(down))
            {
                c.y += _settings.KEYBOARD_CONTROLLER_INCREMENT;
            }

            if (_justPressed(left) || _stillPressed(left))
            {
                c.x -= _settings.KEYBOARD_CONTROLLER_INCREMENT;
            }
            else if (_justPressed(right) || _stillPressed(right))
            {
                c.x += _settings.KEYBOARD_CONTROLLER_INCREMENT;
            }

            if (_justPressed(trig))
            {
                c.TriggerVal = 100;
            }
            else if (_stillPressed(trig))
            {
                c.TriggerVal = 255;
            }
            else
            {
                c.TriggerVal = 0;
            }

            c.UpdateFromKeyboard(_logic.State);
        }

        /// <summary>
        /// This function handles the move controller input. This includes 
        /// storing the controller locations, state of the controllers 
        /// (searching, target lock, waiting for new apple), and recognizing 
        /// any controller gestures.
        /// </summary>
        private void _handleMoveInput()
        {
            Controller c;

            for (int i = 0; i < _settings.MAX_CONTROLLERS; i++)
            {
                PSMoveSharpGemState gem = _currentMoveState.gemStates[i];

                c = Controllers[(ControllerIndex)i];

                //Setting the TriggerVal updates the rest of the trigger
                //  properties
                c.TriggerVal = (int)gem.pad.analog_trigger;

                //Always update the controller position after the trigger value
                c.UpdateFromMove((int)gem.pos.x, (int)gem.pos.y, (int)gem.pos.z, _logic.State);
            }
        }
        #endregion

        #region POSITION CALIBRATION

        /// <summary>
        /// This function allows the calibration mode to request location 
        /// values from the move controllers. The location value is only set 
        /// when the user pulls the trigger. If the user is not pulling the 
        /// trigger, then false is returned.
        /// </summary>
        /// <param name="ci">The controller index to wait for</param>
        /// <param name="location">Location of the controller</param>
        /// <returns>True if the trigger was pulled and the location obtained</returns>
        public bool DetectCalibration(ControllerIndex ci, ref Point location)
        {
            if (Controllers[ci].TriggerVal > 200)
            {
                //Wait for them to release the trigger
                do
                {
                    if (!_settings.USE_KEYBOARD_ONLY)
                        _updateMoveState();
                    else
                    {
                        _updateKeyboardState();
                        _handleKeyboardInput();
                    }
                }
                while(Controllers[ci].TriggerVal > 200);

                location = Controllers[ci].Location;
                return true;
            }

#if DEBUG
            if (_currentKeyboardState.IsKeyDown(Keys.Space) && _previousKeyboardState.IsKeyUp(Keys.Space))
            {
                switch(_logic.CalibrationStage)
                {
                    case 1:
                        location.X = 0;
                        location.Y = 0;
                        break;
                    case 2:
                        location.X = 0;
                        location.Y = _settings.SCREEN_HEIGHT;
                        break;
                    case 3:
                        location.X = _settings.SCREEN_WIDTH;
                        location.Y = 0;
                        break;
                    case 4:
                        location.X = _settings.SCREEN_WIDTH;
                        location.Y = _settings.SCREEN_HEIGHT;
                        break;
                }

                return true;
            }
#endif
            return false;
        }

        /// <summary>
        /// This is a blocking function which waits for either the Move button 
        /// or the trigger to be pressed
        /// </summary>
        /// <returns>True if the Move was pressed False if the trigger was pressed</returns>
        public bool DetectMainMenuButtonPress(ref bool Move, ref bool GoBack)
        {
            if (!_settings.USE_KEYBOARD_ONLY)
            {
                if (_justPressed(PSMoveSharpConstants.ctrlSquare, ControllerIndex.LeftHand)
                    || _justPressed(PSMoveSharpConstants.ctrlSquare, ControllerIndex.RightHand))
                {
                    GoBack = true;
                    Move = false;

                    return true;
                }

                if (_justPressed(PSMoveSharpConstants.ctrlTick, ControllerIndex.LeftHand)
                    || _justPressed(PSMoveSharpConstants.ctrlTick, ControllerIndex.RightHand))
                {
                    GoBack = false;
                    Move = true;

                    return true;
                }

                if (Controllers[ControllerIndex.LeftHand].TriggerVal > 125
                    || Controllers[ControllerIndex.RightHand].TriggerVal > 125)
                {
                    GoBack = false;
                    Move = false;

                    return true;
                }
            }
            else
            {
                if (_justPressed(Keys.Delete))
                {
                    GoBack = true;
                    Move = false;

                    return true;
                }

                if (_justPressed(Keys.Space))
                {
                    GoBack = false;
                    Move = true;

                    return true;
                }

                if (_justPressed(Keys.F) || _justPressed(Keys.H))
                {
                    GoBack = false;
                    Move = false;

                    return true;
                }
            }

            return false;
        }

        #endregion

        #region GESTURE DETECTION

        /// <summary>
        /// This gesture recognition function performs only a very basic analysis
        /// of "apple grabbing" gestures. If the apple is not rotten (or rotten
        /// apples are not allowed) then the function looks to see if the user 
        /// grabs lowers the apple into the imaginary bucket at their feet 
        /// before letting go of the trigger or running out of time. If the apple
        /// is rotten, the function looks to see that the user pitches it over
        /// their should before releasing the trigger
        /// </summary>
        /// <param name="ci">The controller whose input needs to be analyzed</param>
        /// <returns>True if the apple was collected or if the rotten apple
        /// was pitched</returns>
        public void DetectCollectionGesture(ControllerIndex ci)
        {
            //DateTime start = DateTime.Now;
            //TimeSpan limit = new TimeSpan(0, 0, 0, 0, _settings.TIME_TO_COLLECT_TARGET);
            //Point rottenDest = Point.Zero;
            //double rottenDistance = 0;

            //Determine which controller is going to perform the gesture based
            //  on which controller the user pulled the trigger on

            if (ci == ControllerIndex.BothHands)
            {
                if (Controllers[ControllerIndex.RightHand].TriggerState == InputButtonState.JustPressed
                    || Controllers[ControllerIndex.RightHand].TriggerState == InputButtonState.StillPressed)
                {
                    //ci = ControllerIndex.RightHand;
                    if (Settings.Default.SIMULTANEOUS_TARGETS)
                    {
                        Controllers.StopRumble(ControllerIndex.RightHand, RumbleStates.Off);
                    }
                    else
                    {
                        Controllers.StopRumbles();
                    }

                    Controllers[ControllerIndex.RightHand].Target.WasCollected = true;
                }
                else
                {
                    //ci = ControllerIndex.LeftHand;
                    if (Settings.Default.SIMULTANEOUS_TARGETS)
                    {
                        Controllers.StopRumble(ControllerIndex.LeftHand, RumbleStates.Off);
                    }
                    else
                    {
                        Controllers.StopRumbles();
                    }

                    Controllers[ControllerIndex.LeftHand].Target.WasCollected = true;
                }
            }
            else
            {
                Controllers.StopRumbles();
                Controllers[ci].Target.WasCollected = true;
            }

            //Controller controller = Controllers[ci];

            //int goalDeltaY = (int)((double)_settings.CAL_Y_RANGE / (-8));
            //int goalDeltaZ = 0;

            ////If the apple is rotten, invert the y direction of the gesture
            ////  and set the Z delta to indicate throwing the apple over their 
            ////  shoulder
            //if (controller.Target.IsRotten)
            //{
            //    if (ci == ControllerIndex.RightHand)
            //        rottenDest = new Point(_settings.CAL_RIGHT_SHOULDER_X, _settings.CAL_RIGHT_SHOULDER_Y);
            //    else
            //        rottenDest = new Point(_settings.CAL_LEFT_SHOULDER_X, _settings.CAL_LEFT_SHOULDER_Y);

            //    rottenDistance = Controller.Distance(controller.Location, rottenDest);

            //    goalDeltaZ = (-50);
            //}

            ////Wait for the user to release the trigger
            //while(controller.TriggerVal > 0)
            //{
            //    if (!_settings.USE_KEYBOARD_ONLY)
            //        _updateMoveState();
            //    else
            //    {
            //        _updateKeyboardState();
            //        _handleKeyboardInput();
            //    }

            //    if (DateTime.Now - start > limit)
            //    {
            //        controller.Target.WasCollected = false; //Time limit ran out
            //        return;
            //    }
            //}

            //if (_settings.USE_KEYBOARD_ONLY)
            //    controller.Target.WasCollected = true;

            ////Check for the gestures completion
            //if (!controller.Target.IsRotten) //Good apple
            //{
            //    if (controller.LastTrigYDelta >= 25)
            //    {
            //        controller.Target.WasCollected = false;
            //    }
            //    else if (controller.LastTrigYDelta <= goalDeltaY)
            //    {
            //        controller.Target.WasCollected = true; //moved far enough down
            //    }
            //    else
            //    {
            //        controller.Target.WasCollected = false;
            //    }
            //}
            //else //Rotten apple
            //{
            //    if (controller.LastTrigZDelta >= 100)
            //    {
            //        controller.Target.WasCollected = false; //moved in the wrong direction
            //    }
            //    else if(Controller.WithinDist(controller.Location, rottenDest, 100)
            //            && controller.LastTrigZDelta <= goalDeltaZ)
            //    {
            //        controller.Target.WasCollected = true;
            //    }
            //    else if (Controller.Distance(controller.Location, rottenDest) < rottenDistance
            //        && controller.LastTrigZDelta <= goalDeltaZ)
            //    {
            //        controller.Target.WasCollected = true; //moved far enough in the right direction
            //    }
            //    else
            //    {
            //        controller.Target.WasCollected = false;
            //    }
            //}
        }

        #endregion

        #region KEY PRESS MONITORING
        /// <summary>
        /// Returns true if the key passed in is pressed but was not pressed in
        /// the previous keyboard state
        /// </summary>
        /// <param name="key">Keyboard key to check</param>
        private bool _justPressed(Keys key)
        {
            if (_currentKeyboardState.IsKeyDown(key)
                && _previousKeyboardState.IsKeyUp(key))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns true if the button passed in is pressed but was not pressed
        /// in the previous move state. Due to the fact that the Move input is
        /// being gathered asynchronously on a separate thread, there is a 
        /// high probability that this will not capture the exact moment that 
        /// a button is pressed. Using _stillPressed is far more reliable...
        /// </summary>
        /// <param name="key">Move button to check - PSMoveSharpConstants</param>
        private bool _justPressed(int button, ControllerIndex ci)
        {
            if (_moveClient != null && Settings.Default.MOVE_BUTTONS_ACTIVE)
            {
                ushort lastButtons = _previousMoveButtons[(int)ci];
                ushort buttons = _currentMoveState.gemStates[(int)ci].pad.digitalbuttons;
                ushort changed = (ushort)(buttons ^ lastButtons);
                ushort justPressed = (ushort)(changed & buttons);

                if ((justPressed & button) == button)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }

        /// <summary>
        /// Returns true if the key passed in was pressed in the previous state
        /// and is still pressed in the current state
        /// </summary>
        /// <param name="key">Keyboard key to check</param>
        private bool _stillPressed(Keys key)
        {
            if (_currentKeyboardState.IsKeyDown(key)
                && _previousKeyboardState.IsKeyDown(key))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns true if the button passed in was pressed in the previous 
        /// state and is still pressed in the current state
        /// </summary>
        /// <param name="key">Move button to check - PSMoveSharpConstants</param>
        private bool _stillPressed(int button, ControllerIndex ci)
        {
            if (_moveClient != null && Settings.Default.MOVE_BUTTONS_ACTIVE)
            {
                ushort lastButtons = _previousMoveButtons[(int)ci];
                ushort buttons = _currentMoveState.gemStates[(int)ci].pad.digitalbuttons;
                ushort stillPressed = (ushort)(lastButtons & buttons);

                if ((stillPressed & button) == button)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }
        #endregion

        #region EVENT HANDLERS
        /// <summary>
        /// Event handler called when the game is shutting down. This shuts down the move
        /// client by stopping its update thread and closing the client.
        /// </summary>
        private void GameShutDownEvent()
        {
            if (_moveClient != null)
            {
                _moveClient.StopThread();
                _moveClient.Close();
            }

            Debug.WriteLine("Input component shut down");
        } 
        #endregion

    }
}
