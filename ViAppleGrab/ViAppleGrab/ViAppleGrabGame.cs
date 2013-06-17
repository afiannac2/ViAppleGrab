using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ViAppleGrab.Properties;
using System;
using System.Xml;
using ViToolkit.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace ViAppleGrab
{
    /// <summary>
    /// This is the parent type which runs the ViAppleGrab game. It is primarily responsible for 
    /// initializing the input and logic components, loading content, and then handling the drawing
    /// of the game to the screen.
    /// </summary>
    public class ViAppleGrabGame : Microsoft.Xna.Framework.Game
    {
        #region GRAPHICS AND SETTINGS
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private static Settings _settings = Settings.Default;
        private string errorMessage = "";
        private string fixMessage = "";
        private static LoadingState _loadingStep = LoadingState.Initializing;
        private DateTime _loadingStart;
        private bool _wasMoveButton = false;
        private bool _goBack = false;
        private bool _loadingSpeak = true;
        #endregion

        #region GAME COMPONENTS
        private static ViAppleGrabInput _input = null;
        public static ViAppleGrabInput Input
        {
            get { return _input; }
        }

        private static ViAppleGrabLogic _logic = null;
        public static ViAppleGrabLogic Logic
        {
            get { return _logic; }
        }

        private static ViAppleGrabSound _sound = null;
        public static ViAppleGrabSound Sound
        {
            get { return _sound; }
        }
        #endregion

        #region SPRITE TEXTURES AND FONTS
        private Texture2D texApple;
        private Texture2D texRottenApple;
        private Texture2D texCross;
        private Texture2D texRightHand;
        private Texture2D texLeftHand;
        private Texture2D texDivider;
        private SpriteFont outputFont;
        private SpriteFont largeFont;
        private Texture2D bgd;
        #endregion

        UserSelection form = null;

        /// <summary>
        /// Default constructor for the ViAppleGrabEngine. This constructor creates each of the
        /// game components and subscribes to important game events like the event which fires
        /// when the game is over.
        /// </summary>
        public ViAppleGrabGame()
        {
            //Write out the important settings to the trace file?

            //DON'T CHANGE THE ORDER THESE COMPONENTS ARE ADDED IN
            _input = new ViAppleGrabInput(this);
            Components.Add(_input);

            _logic = new ViAppleGrabLogic(this);
            Components.Add(_logic);

            _sound = new ViAppleGrabSound(this);
            Components.Add(_sound);
            
            //Create the graphics manager and set the size of the display screen
            graphics = new GraphicsDeviceManager(this);
#if DEBUG
            graphics.PreferredBackBufferHeight = _settings.SCREEN_HEIGHT * 2;
#else
            graphics.PreferredBackBufferHeight = (int)((double)_settings.SCREEN_HEIGHT * 1.5);
#endif
            graphics.PreferredBackBufferWidth = _settings.SCREEN_WIDTH;

            Content.RootDirectory = "Content";

            Debug.WriteLine("Game created");
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //Show the user data input window
            if (_settings.IN_TESTING_MODE)
            {
                form = new UserSelection(this);
                form.ShowInTaskbar = false;
                form.Show();
                ViAppleGrabInput.GameHasFocus = false;
            }
            else
            {
                ViAppleGrabInput.GameHasFocus = true;
            }

            //Subscribe to events within game components
            _logic.GameOverEvent += new ViAppleGrabLogic.GameOverEventHandler(GameOverEvent);
            _logic.GameShutDownEvent += new ViAppleGrabLogic.GameShutDownEventHandler(GameShutDownEvent);
            _logic.GameSystemErrorOccured += new ViAppleGrabLogic.GameSysErrorHandler(GameSystemErrorOccured);

            //Set the timeframe in which the game updates
            this.IsFixedTimeStep = false;
            //this.TargetElapsedTime = new TimeSpan(0, 0, 0, 0, _settings.UPDATE_INTERVAL);

            base.Initialize();

            Debug.WriteLine("Game Initialized...");

            //_logic.OutputGameInfo();

            _loadingStart = DateTime.Now;

            bgd = new Texture2D(GraphicsDevice, 640, 480, false, SurfaceFormat.Color);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load the different textures to be rendered in the game
            texApple = Content.Load<Texture2D>(@"Graphics\Apple");
            texRottenApple = Content.Load<Texture2D>(@"Graphics\RottenApple");
            texRightHand = Content.Load<Texture2D>(@"Graphics\RightHand");
            texLeftHand = Content.Load<Texture2D>(@"Graphics\LeftHand");
            texDivider = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            outputFont = Content.Load<SpriteFont>(@"Fonts\OutputFont");
            largeFont = Content.Load<SpriteFont>(@"Fonts\OutputFontLarge");
            texCross = Content.Load<Texture2D>(@"Graphics\Cross");

            Debug.WriteLine("All content loaded");
        }

        protected override void UnloadContent()
        {
            this.Content.Unload();
            _sound.UnloadContent();

            base.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //Keep the game in the initializing state until the user info has been collected
            if (!ViAppleGrabInput.GameHasFocus)
            {
                _loadingStep = LoadingState.GatheringUserData;
            }

            if (_logic.State == GameState.Calibration)
            {
                if (!_settings.IS_CALIBRATED)
                {
                    _logic.PerformCalibration();
                }
                else
                {
                    _logic.SkipCalibration();
                }
            }
            else if (_logic.State == GameState.Instructions)
            {
                _logic.PlayInstructions();
            }
            else if (_logic.State == GameState.Loading)
            {
                switch (_loadingStep)
                {
                    case LoadingState.GatheringUserData:
                        _loadingStep = LoadingState.Initializing; //Allow the game to escape this state only if the focus has shifted
                        break;

                    case LoadingState.Initializing: //Allow the screen to be created before doing anything else
                        if ((DateTime.Now - _loadingStart).TotalMilliseconds > 1000)
                            _loadingStep++;
                        break;

                    case LoadingState.Welcome: //Welcome!
                        _sound.GameWelcome(0, true);

                        if (_settings.QUICK_CALIBRATION) 
                        {
                            //The "quick calibration" state will skip the audio 
                            //  cues for the calibration and skip the rest of 
                            //  the menu afterwards
                            if (!_settings.IS_CALIBRATED)
                            {
                                _logic.PerformCalibration();
                            }

                            _loadingStep = LoadingState.Completing;
                        }
                        else
                            _loadingStep = LoadingState.Calibration;
                        break;

                    case LoadingState.Calibration: //Does the user want to calibrate the game?
                        if (_loadingSpeak)
                        {
                            _sound.GameWelcome(1, false);
                            _loadingSpeak = false;
                            break;
                        }

                        if (_input.DetectMainMenuButtonPress(ref _wasMoveButton, ref _goBack))
                        {
                            _sound.GameWelcomeStop(1);
                            _loadingStep = LoadingState.Instructions;
                            _loadingSpeak = true;

                            if (_wasMoveButton)
                            {
                                _settings.IS_CALIBRATED = false;
                                _logic.PerformCalibration();
                            }
                        }
                        break;

                    case LoadingState.Instructions: //Does the user want to hear the rules and instructions?
                        if (_loadingSpeak)
                        {
                            _sound.GameWelcome(2, false);
                            _loadingSpeak = false;
                            break;
                        }

                        if (_input.DetectMainMenuButtonPress(ref _wasMoveButton, ref _goBack))
                        {
                            _sound.GameWelcomeStop(2);
                            _loadingStep = LoadingState.Completing;
                            _loadingSpeak = true;

                            if (_goBack)
                                _loadingStep = LoadingState.Calibration;
                            else if (_wasMoveButton)
                                _logic.PlayInstructions();
                        }
                        break;

                    case LoadingState.Completing: //Start the game
                        //Here we go! Lets grab some apples
                        _sound.GameWelcome(3, true);

                        //Reset the loading step indicator in case the user restarts the game later
                        _loadingStep = LoadingState.Initializing;

                        //Begin the game now that all of the components have been 
                        //  instantiated and initialized...
                        _logic.StartGame();
                        break;
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This function will take the user back to the top of the main menu
        /// while the game is in the Loading state. Only call this function if 
        /// the game is in the loading state
        /// </summary>
        public static void MainMenu()
        {
            _loadingStep = LoadingState.Welcome;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            //Image2Texture(_input._currentFrame, GraphicsDevice, ref bgd);
            if (_settings.SHOW_CAMERA && form.CameraForm != null)
                form.CameraForm.SetImage(ref _input._currentFrame);

            spriteBatch.Begin();
            
            if (_logic.State == GameState.Error)
            {
                DrawError();
            }
            else
            {
                //Draw the divider
                texDivider.SetData(new[] { Color.Black });

                spriteBatch.Draw(texDivider,
                                new Vector2(0, _settings.SCREEN_HEIGHT), null,
                                Color.White, 0.0f, Vector2.Zero,
                                new Vector2(_settings.SCREEN_WIDTH, 3),
                                SpriteEffects.None, 0);

                DrawStats();

                //Draw to the screen based on the state the game is in
                switch (_logic.State)
                {
                    case GameState.Loading:
                        DrawLoadingGame();
                        break;

                    case GameState.Active:
                        if(!_settings.HIDE_OUTPUT)
                            DrawActiveGame();
                        break;

                    case GameState.Calibration:
                        DrawCalibration();
                        break;

                    case GameState.Paused:
                        DrawPausedGame();
                        break;

                    case GameState.GameOver:
                        DrawGameOver();
                        break;
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawStats()
        {
            //Draw game output text to the screen
            if (ViAppleGrabInput.GameHasFocus)
            {
                if (_settings.TIMED_GAME)
                    spriteBatch.DrawString(largeFont, "Next level in " + (_logic.RemainingTime).ToString() + "...", new Vector2(0, 480), Color.Black);
                else
                    spriteBatch.DrawString(largeFont, (_logic.RemainingTargets).ToString() + " Targets Left...", new Vector2(0, 480), Color.Black);
            }

            spriteBatch.DrawString(largeFont, "Game Level: " + _logic.GameLevel.ToString() + " / " + _settings.MAX_LEVELS, new Vector2(0, 520), Color.Black);
            spriteBatch.DrawString(largeFont, "Current Score: " + _logic.Score.ToString(), new Vector2(0, 560), Color.Black);

#if DEBUG
            //Draw controller state information to the screen
            spriteBatch.DrawString(largeFont, "Cont 1 - X: "
                            + _input.Controllers[ControllerIndex.RightHand].x.ToString()
                            + ", Y: "
                            + _input.Controllers[ControllerIndex.RightHand].y.ToString(),
                            new Vector2(0, 640), Color.Black);
            spriteBatch.DrawString(largeFont, "         Out of Bounds: "
                            + _input.Controllers[ControllerIndex.RightHand].IsOutOfBounds.ToString(),
                            new Vector2(0, 675), Color.Black);
            spriteBatch.DrawString(largeFont, "         Trigger: "
                            + _input.Controllers[ControllerIndex.RightHand].TriggerState.ToString(),
                            new Vector2(0, 710), Color.Black);
            spriteBatch.DrawString(largeFont, "         Rumble: "
                            + _input.Controllers[ControllerIndex.RightHand].RumbleState.ToString()
                            + " | " + _input.Controllers[ControllerIndex.RightHand].RumbleIntensity.ToString(),
                            new Vector2(0, 745), Color.Black);

            spriteBatch.DrawString(largeFont, "Cont 2 - X: "
                            + _input.Controllers[ControllerIndex.LeftHand].x.ToString()
                            + ", Y: "
                            + _input.Controllers[ControllerIndex.LeftHand].y.ToString(),
                            new Vector2(0, 805), Color.Black);
            spriteBatch.DrawString(largeFont, "         Out of Bounds: "
                            + _input.Controllers[ControllerIndex.LeftHand].IsOutOfBounds.ToString(),
                            new Vector2(0, 840), Color.Black);
            spriteBatch.DrawString(largeFont, "         Trigger: "
                            + _input.Controllers[ControllerIndex.LeftHand].TriggerState.ToString(),
                            new Vector2(0, 875), Color.Black);
            spriteBatch.DrawString(largeFont, "         Rumble: "
                            + _input.Controllers[ControllerIndex.LeftHand].RumbleState.ToString()
                            + " | " + _input.Controllers[ControllerIndex.LeftHand].RumbleIntensity.ToString(),
                            new Vector2(0, 910), Color.Black);
#endif
        }

        private void DrawLoadingGame()
        {
            string text = "", text2 = "", text3 = "";

            spriteBatch.DrawString(largeFont, "Main Menu:", new Vector2(0, 0), Color.Black);

            //Output the spoken statements to the screen
            switch(_loadingStep)
            {
                case LoadingState.Initializing:
                    text = "Initializing...";

                    if(!ViAppleGrabInput.GameHasFocus)
                        text2 = "Input user test data to continue...";

                    break;

                case LoadingState.Welcome:
                    text = "Welcome to ViAppleGrab!";
                    break;

                case LoadingState.Calibration:
                    text =  "[Move Button]:   Calibrate the game...";
                    text2 = "[Trigger]:       Skip to next menu item...";
                    break;

                case LoadingState.Instructions:
                    text =  "[Move Button]:   Hear the instructions...";
                    text2 = "[Trigger]:       Skip to next menu item...";
                    text3 = "[Square Button]: Go to previous menu item...";
                    break;

                case LoadingState.Completing:
                    text = "Game starting...";
                    break;
            }

            spriteBatch.DrawString(outputFont, text, new Vector2(50, 50), Color.Black);

            if (text2 != "")
            {
                spriteBatch.DrawString(outputFont, text2, new Vector2(50, 80), Color.Black);
            }

            if (text3 != "")
            {
                spriteBatch.DrawString(outputFont, text3, new Vector2(50, 110), Color.Black);
            }
        }

        private void DrawActiveGame()
        {
            Point leftPoint = _input.Controllers[ControllerIndex.LeftHand].Location;
            Point rightPoint = _input.Controllers[ControllerIndex.RightHand].Location;
            int radius = _settings.HAND_SPRITE_SIDE_LENGTH / 2;
            int targetRadius = _settings.TARGET_SPRITE_SIDE_LENGTH / 2;
            int x, y;

            //Draw the image
            //spriteBatch.Draw(bgd, new Vector2(0, 0), Color.White);

            //Draw the target - either an apple or a rotten apple
            if (_input.Controllers.CurrController != ControllerIndex.BothHands)
            {
                x = _input.Controllers.Current.Target.x - targetRadius;
                y = _input.Controllers.Current.Target.y - targetRadius;

                if (_input.Controllers.Current.Target.IsRotten)
                {
                    spriteBatch.Draw(texRottenApple, new Vector2(x, y), Color.White);
                }
                else
                {
                    spriteBatch.Draw(texApple, new Vector2(x, y), Color.White);
                }
            }
            else
            {
                x = _input.Controllers[ControllerIndex.RightHand].Target.x - targetRadius;
                y = _input.Controllers[ControllerIndex.RightHand].Target.y - targetRadius;

                if (_input.Controllers[ControllerIndex.RightHand].Target.IsRotten)
                {
                    spriteBatch.Draw(texRottenApple, new Vector2(x, y), Color.White);
                }
                else
                {
                    spriteBatch.Draw(texApple, new Vector2(x, y), Color.White);
                }

                if (Settings.Default.SIMULTANEOUS_TARGETS)
                {
                    x = _input.Controllers[ControllerIndex.LeftHand].Target.x - targetRadius;
                    y = _input.Controllers[ControllerIndex.LeftHand].Target.y - targetRadius;

                    if (_input.Controllers[ControllerIndex.LeftHand].Target.IsRotten)
                    {
                        spriteBatch.Draw(texRottenApple, new Vector2(x, y), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(texApple, new Vector2(x, y), Color.White);
                    }
                }
            }


            //Draw the controller positions using the hand graphics
            spriteBatch.Draw(texLeftHand, new Vector2(leftPoint.X - radius, leftPoint.Y - radius), Color.White);
            spriteBatch.Draw(texRightHand, new Vector2(rightPoint.X - radius, rightPoint.Y - radius), Color.White);

            //Draw a red x on top of the current controllers
            if (_input.Controllers.CurrController == ControllerIndex.RightHand)
            {
                spriteBatch.Draw(texCross, new Vector2(rightPoint.X - radius, rightPoint.Y - radius), Color.White);
            }
            else if (_input.Controllers.CurrController == ControllerIndex.LeftHand)
            {
                spriteBatch.Draw(texCross, new Vector2(leftPoint.X - radius, leftPoint.Y - radius), Color.White);
            }
            else
            {
                spriteBatch.Draw(texCross, new Vector2(rightPoint.X - radius, rightPoint.Y - radius), Color.White);
                spriteBatch.Draw(texCross, new Vector2(leftPoint.X - radius, leftPoint.Y - radius), Color.White);
            }
        }

        
        private void DrawCalibration()
        {
            spriteBatch.DrawString(largeFont, "Calibration Stage: " + _logic.CalibrationStage,
                            new Vector2(0, 0), Color.Black);

            string text = "";

            switch(_logic.CalibrationStage)
            {
                case 0:
                    text = "Calibration is starting up...";
                    break;

                case 1: //Upper Left Corner
                    text = "UPPER LEFT CORNER";
                    break;

                case 2: //Lower Left Corner
                    text = "LOWER LEFT CORNER";
                    break;

                case 3: //Upper Right Corner
                    text = "UPPER RIGHT CORNER";
                    break;

                case 4: //Lower Left Corner
                    text = "LOWER RIGHT CORNER";
                    break;

                case 5: //Right arm straight forward
                    text = "RIGHT ARM STRAIGHT FORWARD";
                    break;

                case 6: //Left arm straight forward
                    text = "LEFT ARM STRAIGHT FORWARD";
                    break;
            }

            spriteBatch.DrawString(largeFont, text, new Vector2(0, 50), Color.Black);
        }

        private void DrawPausedGame()
        {
            //Only draw a message that the game is paused
            spriteBatch.DrawString(largeFont, "GAME PAUSED", new Vector2(0, 0), Color.Black);
            spriteBatch.DrawString(largeFont, "Press [SPACE] to continue...", new Vector2(0, 50), Color.Black);
        }

        private void DrawGameOver()
        {
            //Only draw a message that the game is over
            spriteBatch.DrawString(largeFont, "GAME OVER", new Vector2(0, 0), Color.Black);
            spriteBatch.DrawString(largeFont, "Final Score: " + _logic.Score, new Vector2(0, 50), Color.Black);
            spriteBatch.DrawString(largeFont, "Press [ESC] or [MOVE] to quit...", new Vector2(0, 100), Color.Black);
        }

        private void DrawError()
        {
            int maxLen = 51;
            int size = (errorMessage.Length / 51) + 1, height = 0;
            int length = 0;
            int prev = 0;

            spriteBatch.DrawString(outputFont, "ERROR: ", new Vector2(0, height), Color.Black);

            for (int i = 0; i < size; i++)
            {
                if ((maxLen + prev) > errorMessage.Length)
                    length = errorMessage.Length - prev;
                else
                {
                    length = maxLen;

                    while (errorMessage[prev + length - 1] != ' ')
                        length--;
                }

                spriteBatch.DrawString(outputFont, errorMessage.Substring(prev, length), new Vector2(77, height), Color.Black);

                prev += length;

                height += 30;
            }

            if (fixMessage.Length > 0)
            {
                height += 30;
                size = (fixMessage.Length / 51) + 1;
                prev = 0;

                spriteBatch.DrawString(outputFont, "TRY:   ", new Vector2(0, height), Color.Black);

                for (int i = 0; i < size; i++)
                {
                    if ((maxLen + prev) > fixMessage.Length)
                        length = fixMessage.Length - prev;
                    else
                    {
                        length = maxLen;

                        while (fixMessage[prev + length - 1] != ' ')
                            length--;
                    }

                    spriteBatch.DrawString(outputFont, fixMessage.Substring(prev, length), new Vector2(77, height), Color.Black);

                    prev += length;

                    height += 30;
                }
            }

            height += 60;
            spriteBatch.DrawString(outputFont, "Press [SPACE] to quit...", new Vector2(0, height), Color.Black);
        }

        //This region contains the methods that handle events fired by the game components
        #region EVENT HANDLERS
        void GameOverEvent()
        {
            form.Enabled = true;
            ViAppleGrabInput.GameHasFocus = false;
            Debug.WriteLine("Game over");
        }

        void GameShutDownEvent()
        {
            form.Close();
            Debug.WriteLine("Game shut down");
        }

        void GameSystemErrorOccured(string message, string suggestions)
        {
            errorMessage = message;
            fixMessage = suggestions;
        }
        #endregion
    }
}
