using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using ViAppleGrab.Properties;
using System.Threading;
using System.IO;
using ViToolkit.SoundManagement;
using System.Xml;
using ViToolkit.Logging;
using System.Collections.Generic;

namespace ViAppleGrab
{
    /// <summary>
    /// This component handles all of the game logic. It uses events to notify 
    /// the other game components of important events in game logic such as the
    /// game state changing, the game ending, or the game level changing.
    /// </summary>
    public class ViAppleGrabLogic : Microsoft.Xna.Framework.GameComponent
    {

        #region GENERAL SETTINGS AND COMPONENT DATA

        private static Settings _settings = Settings.Default;
        private ViAppleGrabInput _input;
        private ViAppleGrabSound _sound;
        private string _errorMessage = "";
        private string _fixMessage = "";
        private int scorePerApple = _settings.INITIAL_SCORE_PER_APPLE;
        private int _instructionsStage = 0;
        private bool _userDataOutput = false;
        private TimeSpan _timeSinceLocUpdate;
        private bool _gameInfoOutput = false;

        #endregion

        #region CALIBRATION DATA
        Point upperRight, upperLeft, lowerRight, lowerLeft, rightShoulder, leftShoulder;
        private int _calibrationStage = 0;
        public int CalibrationStage
        {
            get
            {
                return _calibrationStage;
            }
        }
        private bool _speak = true;
        #endregion

        #region LEVEL SPECIFIC INFORMATION

        //Number of targets left in this round
        private int _remainingTargets = (-1);
        public int RemainingTargets
        {
            get
            {
                return _remainingTargets;
            }
            private set
            {
                if (value != _remainingTargets)
                {
                    if (value == 0)
                    {
                        if (GameLevel == _settings.MAX_LEVELS)
                        {
                            _input.Controllers.DeactivateTargets();
                            EndGame();
                        }
                        else
                        {
                            GameLevel++;
                        }
                    }
                    else
                    {
                        _remainingTargets = value;

                        //Spawn an apple
                        _input.Controllers.SpawnApple();

                        //Write the target trace data
                        _outputTraceLevelData();
                    }
                }
            }
        }

        //Amount of time left for the current apple
        private int _aliveTime = Settings.Default.BASE_ALIVE_TIME;
        public int AliveTime
        {
            get
            {
                return _aliveTime;
            }
            private set
            {
                if (value <= Settings.Default.BASE_ALIVE_TIME && value > 0)
                    _aliveTime = value;
            }
        }

        //Total time for this level
        private int _levelDuration = Settings.Default.BASE_LEVEL_TIME;
        public int LevelDuration
        {
            get
            {
                return _levelDuration;
            }
            private set
            {
                if (value <= Settings.Default.BASE_LEVEL_TIME && value > 0)
                    _levelDuration = value;
            }
        }

        private int _remainingAliveTime;
        public int RemainingAliveTime
        {
            get
            {
                return _remainingAliveTime;
            }
            private set
            {
                if (value != _remainingAliveTime)
                {
                    if (value >= _aliveTime)
                    {
                        _remainingAliveTime = _aliveTime;
                    }
                    else if (value <= 0)
                    {
                        //Time is up, kill the current apple and spawn a new one
                        _input.Controllers.TimeOutTargets();
                        _input.Controllers.StopRumbles();
                        _input.Controllers.SpawnApple();

                        //Write the target trace data
                        _outputTraceLevelData();

                        _remainingAliveTime = _aliveTime;
                    }
                    else
                    {
                        _remainingAliveTime = value;
                    }
                }
            }
        }
        //The second timer decrements the remaining time every time it ticks.
        //  When the remaining time gets low it fires warnings to allow other
        //  game components to react to the low time. When time runs out, it
        //  moves the game into the next level or signals a 'Game Over'.
        private Timer secondTimer = null;
        private int _remainingTime = Settings.Default.BASE_LEVEL_TIME;
        public int RemainingTime
        {
            get
            {
                return _remainingTime;
            }
            private set
            {
                //Only update if it has actually changed
                if (value != _remainingTime)
                {
                    if (value >= _levelDuration)
                    {
                        _remainingTime = _levelDuration;
                    }
                    else if (value > 3)
                    {
                        _remainingTime = value;
                    }
                    //Time is running short - give a warning
                    else if (value <= 3 && value > 0)
                    {
                        if (TimeIsLowAlert != null) TimeIsLowAlert(value);

                        _remainingTime = value;
                    }
                    //Time is up
                    else if (GameLevel < Settings.Default.MAX_LEVELS)
                    {
                        //Time is up, kill the current apple and spawn a new one
                        _input.Controllers.TimeOutTargets();
                        _input.Controllers.StopRumbles();

                        secondTimer.Change(Timeout.Infinite, Timeout.Infinite);
                        GameLevel++;

                        _input.Controllers.SpawnApple();

                        //Write the target trace data
                        _outputTraceLevelData();
                    }
                    //The time has expired in the final level - GAME OVER
                    else
                    {
                        _input.Controllers.DeactivateTargets();
                        EndGame();
                    }
                }
            }
        }
        public delegate void TimeIsLowAlertEvent(int secondsRemaining);
        public event TimeIsLowAlertEvent TimeIsLowAlert;

        #endregion

        #region PLAYER INFO FIELDS AND PROPERTIES
        public int UserID = -1;
        private Controller[] _controllerData = null;
        private int _score = 0;
        public int Score
        {
            get
            {
                return _score;
            }
            private set
            {
                if (value < _score && ScoreDecreased != null)
                    ScoreDecreased();
                else if (value > _score && ScoreIncreased != null)
                {
                    if (_settings.SIMULTANEOUS_TARGETS)
                        ScoreIncreased(value - _score, RemainingTargets - 2, GameLevel);
                    else
                        ScoreIncreased(value - _score, RemainingTargets - 1, GameLevel);
                }

                _score = value;

                Debug.WriteLine("Score: " + _score.ToString());
            }
        }
        public delegate void ScoreDecreasedEventHandler();
        public event ScoreDecreasedEventHandler ScoreDecreased;
        public delegate void ScoreIncreasedEventHandler(int increase, int remainingTargets, int gamelevel);
        public event ScoreIncreasedEventHandler ScoreIncreased;

        #endregion

        #region GAME_STATE FIELD, PROPERTY, AND EVENT HANDLERS

        private GameState _state = GameState.Loading;
        public GameState State
        {
            get
            {
                return _state;
            }
            private set
            {
                if (_state != value)
                {
                    switch (value)
                    {
                        case GameState.Active:
                            if (_state == GameState.Loading
                                    && GameStarted != null)
                                GameStarted();

                            else if (_state == GameState.Paused
                                    && GameResumed != null)
                            {
                                GameResumed();

                                if (_settings.TIMED_GAME)
                                {
                                    secondTimer.Change(1000, 1000);
                                }
                            }
                            break;

                        case GameState.Paused:
                            if (_settings.TIMED_GAME)
                            {
                                secondTimer.Change(Timeout.Infinite, Timeout.Infinite);
                            }
                            if (GamePaused != null) GamePaused();
                            break;

                        case GameState.GameOver:
                            if (GameOverEvent != null) GameOverEvent();
                            break;

                        case GameState.ShutDown:
                            if (GameShutDownEvent != null) GameShutDownEvent();
                            break;

                        case GameState.Error:
                            if (GameSystemErrorOccured != null) GameSystemErrorOccured(_errorMessage, _fixMessage);
                            _errorMessage = "";
                            break;
                    }

                    _state = value;
                }
            }
        }
        public delegate void GameStartedEventHandler();
        public event GameStartedEventHandler GameStarted;
        public delegate void GameResumedEventHandler();
        public event GameResumedEventHandler GameResumed;
        public delegate void GamePausedEventHandler();
        public event GamePausedEventHandler GamePaused;
        public delegate void GameOverEventHandler();
        public event GameOverEventHandler GameOverEvent;
        public delegate void GameShutDownEventHandler();
        public event GameShutDownEventHandler GameShutDownEvent;
        public delegate void GameSysErrorHandler(string message, string suggestions);
        public event GameSysErrorHandler GameSystemErrorOccured;
        public delegate void GameCalibratingEventHandler();
        public event GameCalibratingEventHandler GameCalibrating;

        #endregion

        #region GAME_LEVEL FIELD, PROPERTY, AND LEVEL CHANGE EVENT HANDLER

        //Set up a readonly property which exposes the current level of the game
        //In addition, set up an event which will fire when the level changes
        private int _gameLevel; //DON'T alter the game level using this field
        public int GameLevel
        {
            get
            {
                return _gameLevel;
            }
            private set
            {
                if (_gameLevel < _settings.MAX_LEVELS)
                {
                    _gameLevel = value;

                    //Fire an event to notify other components that the level has changed
                    GameLevelChangedEventHandler handler = GameLevelChanged;
                    if (handler != null)
                    {
                        handler(value);
                    }

                    //This will start the remaining time timer so it needs to be
                    //  called last
                    _startNewLevel(value);

                    if (value > 1)
                        scorePerApple += _settings.SCORE_PER_APPLE_INCREASE;
                }
            }
        }    //ONLY ALTER IT USING THIS PROPERTY
        public delegate void GameLevelChangedEventHandler(int newLevel);
        public event GameLevelChangedEventHandler GameLevelChanged;

        #endregion

        #region TARGET ACQUISITION FIELDS, PROPERTIES, AND EVENT HANDLERS

        public delegate void TargetLostEventHandler();
        public event TargetLostEventHandler TargetLost;

        public delegate void TargetFoundEventHandler(bool IsRotten);
        public event TargetFoundEventHandler TargetFound;

        public delegate void TargetCollectingEventHandler();
        public event TargetCollectingEventHandler TargetCollecting;

        public delegate void TargetCollectedEventHandler();
        public event TargetCollectedEventHandler TargetCollected;

        public delegate void TargetMissedEventHandler();
        public event TargetMissedEventHandler TargetMissed;

        #endregion

        //These are the three functions which are implemented by default in 
        //  every game component: the constructor, Initialize, and Update
        #region CORE GAME COMPONENT OVERRIDES AND CONSTRUCTOR

        /// <summary>
        /// Default constructor. Enables the component and sets it to update 
        /// after the input component
        /// </summary>
        /// <param name="game">The current game instance</param>
        public ViAppleGrabLogic(Game game)
            : base(game)
        {
            //Initialize game logic fields
            _controllerData = new Controller[_settings.MAX_CONTROLLERS];

            if(_settings.TIMED_GAME)
                secondTimer = new Timer(delegate { 
                    RemainingTime--; 
                    RemainingAliveTime--; 
                });

            //This component should be the second component to update (updates 
            //  after the game input is processed)
            Enabled = true;
            UpdateOrder = 2;

            Debug.WriteLine("Logic component created");
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to
        /// before starting to run.  This is where it can query for any 
        /// required services and load content.
        /// </summary>
        public override void Initialize()
        {
            //This component handles all the user input from the keyboard and 
            //  the move controllers
            _input = (ViAppleGrabInput)Game.Components[(int)ComponentIndex.Input];
            _sound = (ViAppleGrabSound)Game.Components[(int)ComponentIndex.Sound];

            _timeSinceLocUpdate = TimeSpan.Zero;

            base.Initialize();

            Debug.WriteLine("Logic component initialized");
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (ViAppleGrabInput.GameHasFocus && State == GameState.Active)
            {
                if (_settings.USE_KEYBOARD_ONLY || _input.IsMoveUpdated)
                {
                    bool IsCollecting = _input.Controllers.IsTargetCollecting;

                    //Deal with recording analytics
                    _timeSinceLocUpdate += gameTime.ElapsedGameTime;

                    if (_timeSinceLocUpdate.TotalMilliseconds >= 100 || IsCollecting)
                    {
                        _input.Controllers.RecordPositions(_timeSinceLocUpdate);
                        _timeSinceLocUpdate = TimeSpan.Zero;
                    }

                    //Deal with game state
                    if (_input.Controllers.WasTargetCollected)
                    {
                        //If it took the use less than 10 secs, full points
                        //If it took them less than 20 second, they get partial points
                        //Otherwise, no points
                        double temp = _input.Controllers.TargetAliveTime.TotalSeconds;

                        if (temp <= 20)
                        {
                            int temp3 = 20000 / scorePerApple;
                            int temp2 = (int)((_input.Controllers.TargetAliveTime.TotalMilliseconds) / (double)temp3);
                            temp2 = scorePerApple - temp2;

                            Score += Math.Max(temp2, 1);
                        }
                        else
                            Score += 1;

                        //Trigger the target collected event
                        //if (TargetCollected != null) TargetCollected();

                        if (_settings.TIMED_GAME)
                        {
                            //Spawn an apple
                            _input.Controllers.SpawnApple();

                            //Write the target trace data
                            _outputTraceLevelData();

                            RemainingAliveTime = AliveTime;
                        }
                        else
                        {
                            if (_settings.SIMULTANEOUS_TARGETS)
                                RemainingTargets = RemainingTargets - 2;
                            else
                                RemainingTargets--;
                        }
                    }
                    else if (_input.Controllers.WasTargetMissed)
                    {
                        //Score -= scorePerApple;

                        if (TargetMissed != null) TargetMissed();

                        if (_settings.TIMED_GAME)
                        {
                            //Spawn an apple
                            _input.Controllers.SpawnApple();

                            //Write the target trace data
                            _outputTraceLevelData();

                            RemainingAliveTime = AliveTime;
                        }
                        else
                        {
                            //RemainingTargets--;

                            if (_input.Controllers[ControllerIndex.RightHand].Target.State == TargetState.Missed)
                                _input.Controllers[ControllerIndex.RightHand].Target.Activate();

                            if (_input.Controllers[ControllerIndex.LeftHand].Target.State == TargetState.Missed)
                                _input.Controllers[ControllerIndex.LeftHand].Target.Activate();
                        }
                    }
                    else if (IsCollecting) //Have any of the controllers collected the target?
                    {
                        //Trigger the target collecting event
                        if (TargetCollecting != null) TargetCollecting();

                        //Detect the collection gesture and alter the score accordingly
                        _input.DetectCollectionGesture(_input.Controllers.CurrController);

                        TargetCollected();
                    }
                    else if (_input.Controllers.IsTargetFound) //Have any of the controllers found the target?
                    {
                        //Trigger the target found event
                        if (TargetFound != null) TargetFound(_input.Controllers.Current.Target.IsRotten);
                        _input.Controllers.UpdateRumbles();
                    }
                    else if (_input.Controllers.WasTargetLost)
                    {
                        //Trigger the target lost event
                        if (TargetLost != null) TargetLost();
                        _input.Controllers.UpdateRumbles();
                    }
                    else //The user is still searching for the target
                    {
                        _input.Controllers.UpdateRumbles();
                    }
                }
            }

            base.Update(gameTime);
        }

        private void _outputTraceLevelData()
        {
            XmlNode node = XMLTrace.FindTargetDataNode(Target._uniqueTargetID - 1);
            XmlNode child = XMLTrace.AppendSubchild(node, "LevelData", "");

            if (_settings.TIMED_GAME)
            {
                XMLTrace.AddAttributes(child, new Dictionary<string, string> 
                    { 
                        { "Level", GameLevel.ToString() },
                        { "TargetAliveTime", AliveTime.ToString() },
                        { "PotentialPoints", scorePerApple.ToString() }
                    });
            }
            else
            {
                XMLTrace.AddAttributes(child, new Dictionary<string, string> 
                    { 
                        { "Level", GameLevel.ToString() },
                        { "PotentialPoints", scorePerApple.ToString() }
                    });
            }
        }

        public void OutputGameInfo()
        {
            //Output trace information
            XmlNode node = XMLTrace.AppendElement("GameInfo", "");

            string f;
            if ((ControlType)_settings.CONTROL_TYPE == ControlType.Alternating && !_settings.SINGLE_TARGET)
            {
                f = Path.GetFileNameWithoutExtension(_settings.ALTERNATING_FILE);
                f = f.Substring(0, f.IndexOf('_'));
            }
            else if ((ControlType)_settings.CONTROL_TYPE == ControlType.Alternating && _settings.SINGLE_TARGET)
            {
                f = Path.GetFileNameWithoutExtension(_settings.SINGLE_FILE);
            }
            else if ((ControlType)_settings.CONTROL_TYPE == ControlType.Together && _settings.SIMULTANEOUS_TARGETS)
            {
                f = Path.GetFileNameWithoutExtension(_settings.SIMULTANEOUS_FILE);
            }
            else
            {
                f = Path.GetFileNameWithoutExtension(_settings.TOGETHER_FILE);
                f = f.Substring(0, f.IndexOf('_'));
            }



            if (_settings.TIMED_GAME)
            {
                XMLTrace.AddAttributes(node, new Dictionary<string, string> 
                {                     
                    { "MaxControllers", _settings.MAX_CONTROLLERS.ToString() },
                    { "GameType", ((GameType)_settings.GAME_TYPE).ToString() },
                    { "ControlType", ((ControlType)_settings.CONTROL_TYPE).ToString() }, 
                    { "IsWarmup", (f == "Warmup") ? "True" : "False" },
                    { "BaseLevelTime", _settings.BASE_LEVEL_TIME.ToString() },
                    { "TimeDecreasePerLevel", _settings.TIME_DECREASE_PER_LEVEL.ToString() },
                    { "BaseAliveTime", _settings.BASE_ALIVE_TIME.ToString() },
                    { "AliveTimeDecreasePerLevel", _settings.ALIVE_TIME_DECREASE_PER_LEVEL.ToString() },
                    { "TimeToCollectTarget", _settings.TIME_TO_COLLECT_TARGET.ToString() }
                });
            }
            else
            {
                if (_settings.SINGLE_TARGET || _settings.SIMULTANEOUS_TARGETS)
                {
                    string stage;

                    if(f == "Warmup")
                    {
                        stage = "Warmup1";
                    }
                    else if (f == "Warmup2")
                    {
                        stage = "Warmup2";
                    }
                    else
                    {
                        if (_settings.SINGLE_TARGET)
                        {
                            stage = "Single";
                        }
                        else
                        {
                            stage = "Simultaneous";
                        }
                    }

                    XMLTrace.AddAttributes(node, new Dictionary<string, string> 
                    {       
                        { "Study", "Camp Abilities Study"},
                        { "Stage", stage }
                    });
                }
                else
                {
                    XMLTrace.AddAttributes(node, new Dictionary<string, string> 
                    {                     
                        { "MaxControllers", _settings.MAX_CONTROLLERS.ToString() },
                        { "GameType", ((GameType)_settings.GAME_TYPE).ToString() },
                        { "ControlType", ((ControlType)_settings.CONTROL_TYPE).ToString() },
                        { "IsWarmup", (f == "Warmup") ? "True" : "False" },
                        { "TimeToCollectTarget", _settings.TIME_TO_COLLECT_TARGET.ToString() }
                    });
                }
            }

            XmlNode child = XMLTrace.AppendSubchild(node, "TimeStarted", "");
            XMLTrace.AddText(child, DateTime.Now.ToString("O"));
            XMLTrace.AppendElement("Targets", "");

            _gameInfoOutput = true;
        }

        public void OutputGameEndInfo()
        {
            if (_gameInfoOutput)
            {
                XmlNode gameInfo = XMLTrace.FindLastGameInfoNode();
                XmlNode child = XMLTrace.AppendSubchild(gameInfo, "TimeEnded", "");
                XMLTrace.AddText(child, DateTime.Now.ToString("O"));
                child = XMLTrace.AppendSubchild(gameInfo, "Score", "");
                XMLTrace.AddText(child, Score.ToString());

                _gameInfoOutput = false;
            }
        }

        private void OutputUserInfo()
        {
            //Output the user's information once per execution of the game
            if (!_userDataOutput)
            {
                if (UserID != (-1))
                {
                    XmlNode node = XMLTrace.FindUserInfoNode("Users.xml", UserID);

                    XMLTrace.AppendForeignNodeToDocElem(node);
                }

                _userDataOutput = true;
            }
        }

        #endregion

        //The 'Game Flow' functions control events like the pausing, starting, 
        //  and starting new levels of the game.
        #region GAME FLOW FUNCTIONS

        /// <summary>
        /// This function may be called from other game components and is used
        /// to start the game for the first time. This will trigger the 
        /// GameLevelChanged event in order to allow subscribing game 
        /// components to set their state for level 1. This should only be 
        /// called once at the very beginning a game instance.
        /// </summary>
        public void StartGame()
        {
            //Output user info
            OutputUserInfo();

            if (State == GameState.Loading)
            {
                OutputGameInfo(); //xml trace info
                State = GameState.Active;
                GameLevel = 1;

                if (_settings.TIMED_GAME)
                {
                    _input.Controllers.SpawnApple();

                    //Write the target trace data
                    _outputTraceLevelData();
                }
            }
        }

        /// <summary>
        /// This causes the calibration sequence to be started. This centers 
        /// the users controller position values on the visible screen.
        /// </summary>
        public void PerformCalibration()
        {
            State = GameState.Calibration;

            _performCalibration();
        }

        /// <summary>
        /// This will cause the calibration sequence to be skipped in the main menu
        /// </summary>
        public void SkipCalibration()
        {
            State = GameState.Loading;
        }

        /// <summary>
        /// This calibration sequence gathers the information required to 
        /// create a virtual box around the user in which their proprioceptive
        /// display will be bounded. This information is then used to scale the
        /// user's movements into the the dimensions of the game window.
        /// </summary>
        private void _performCalibration()
        {
            switch(_calibrationStage)
            {
                case 0: //Start the calibration cycle
                    //Trigger the calibration event if any other component has subscribed
                    if (GameCalibrating != null) GameCalibrating();

                    if (!_settings.QUICK_CALIBRATION)
                    {
                        _sound.CalInstructionsBlock(0);
                    }

                    _calibrationStage++;
                    break;

                case 1: //Upper Left Corner
                    if (_speak && !_settings.QUICK_CALIBRATION)
                    {
                        _sound.CalInstructions(1);
                        _speak = false;
                    }
                    
                    if (_input.DetectCalibration(ControllerIndex.LeftHand, ref upperLeft))
                    {
                        _sound.CalInstructionsStop(1);
                        Debug.WriteLine("Calibration: UpperLeft - " + upperLeft.ToString());

                        _calibrationStage++;
                        _speak = true;
                    }
                    break;

                case 2: //Lower Left Corner
                    if (_speak && !_settings.QUICK_CALIBRATION)
                    {
                        _sound.CalInstructions(2);
                        _speak = false;
                    }

                    if (_input.DetectCalibration(ControllerIndex.LeftHand, ref lowerLeft))
                    {
                        _sound.CalInstructionsStop(2);
                        Debug.WriteLine("Calibration: LowerLeft - " + lowerLeft.ToString());

                        _calibrationStage++;
                        _speak = true;
                    }
                    break;

                case 3: //Upper Right Corner

                    if (_speak && !_settings.QUICK_CALIBRATION)
                    {
                        _sound.CalInstructions(3);
                        _speak = false;
                    }

                    if (_input.DetectCalibration(ControllerIndex.RightHand, ref upperRight))
                    {
                        _sound.CalInstructionsStop(3);
                        Debug.WriteLine("Calibration: UpperRight - " + upperRight.ToString());

                        _calibrationStage++;
                        _speak = true;
                    }
                    break;

                case 4: //Lower Right Corner
                    if (_speak && !_settings.QUICK_CALIBRATION)
                    {
                        _sound.CalInstructions(4);
                        _speak = false;
                    }

                    if (_input.DetectCalibration(ControllerIndex.RightHand, ref lowerRight))
                    {
                        _sound.CalInstructionsStop(4);
                        Debug.WriteLine("Calibration: LowerRight - " + lowerRight.ToString());

                        _calibrationStage++;
                        _speak = true;

                    }
                    break;

                case 5:
                    if (_speak && !_settings.QUICK_CALIBRATION)
                    {
                        _sound.CalInstructions(5);
                        _speak = false;
                    }

                    if (_input.DetectCalibration(ControllerIndex.RightHand, ref rightShoulder))
                    {
                        _sound.CalInstructionsStop(5);
                        Debug.WriteLine("Calibration: RightShoulder - " + rightShoulder.ToString());

                        _calibrationStage++;
                        _speak = true;
                    }
                    break;

                case 6:
                    if (_speak && !_settings.QUICK_CALIBRATION)
                    {
                        _sound.CalInstructions(6);
                        _speak = false;
                    }

                    if (_input.DetectCalibration(ControllerIndex.LeftHand, ref leftShoulder))
                    {
                        _sound.CalInstructionsStop(6);

                        //Calculate calibration values
                        _settings.CAL_X_MIN = (upperLeft.X + lowerLeft.X) / 2;
                        _settings.CAL_X_MAX = (upperRight.X + lowerRight.X) / 2;
                        _settings.CAL_Y_MIN = (lowerLeft.Y + lowerRight.Y) / 2;
                        _settings.CAL_Y_MAX = (upperLeft.Y + upperRight.Y) / 2;
                        _settings.CAL_X_RANGE = _settings.CAL_X_MAX - _settings.CAL_X_MIN;
                        _settings.CAL_Y_RANGE = _settings.CAL_Y_MAX - _settings.CAL_Y_MIN;
                        _settings.CAL_LEFT_SHOULDER_X = leftShoulder.X;
                        _settings.CAL_LEFT_SHOULDER_Y = leftShoulder.Y;
                        _settings.CAL_RIGHT_SHOULDER_X = rightShoulder.X;
                        _settings.CAL_RIGHT_SHOULDER_Y = rightShoulder.Y;

                        //Set the calibrated flag and restart the game
                        _settings.IS_CALIBRATED = true;
                        _calibrationStage = 0;
                        _speak = true;

                        //RestartGame();
                        State = GameState.Loading;
                    }

                    break;
            }
        }

        public void PlayInstructions()
        {
            State = GameState.Instructions;

            _playInstructions();
        }

        private void _playInstructions()
        {
            int pauseBetween = 750;

            switch(_instructionsStage)
            {
                case 0:
                    if (!ViAppleGrabSound.TutorialStageStarted)
                    {
                        Thread.Sleep(pauseBetween);
                        _sound.Tutorial(0); 
                    }

                    if (ViAppleGrabSound.TutorialStageComplete)
                    {
                        ViAppleGrabSound.TutorialStageStarted = false;
                        _instructionsStage++;
                    }
                    break;

                case 1://Fast and intense vibration example
                    if (!ViAppleGrabSound.TutorialStageStarted)
                    {
                        _sound.Tutorial(1);
                    }

                    if (ViAppleGrabSound.TutorialStageComplete)
                    {
                        _input.Controllers.StartRumble(ControllerIndex.RightHand, 255);
                        _input.Controllers.StartRumble(ControllerIndex.LeftHand, 255);

                        Thread.Sleep(200);

                        for (int i = 0; i < 10; i++)
                        {
                            _input.Controllers.StopRumbles();

                            Thread.Sleep(200);

                            _input.Controllers.StartRumble(ControllerIndex.RightHand, 255);
                            _input.Controllers.StartRumble(ControllerIndex.LeftHand, 255);

                            Thread.Sleep(200);
                        }

                        _input.Controllers.StopRumbles();
                        Thread.Sleep(750);

                        ViAppleGrabSound.TutorialStageStarted = false;
                        _instructionsStage++;
                    }
                    break;

                case 2://Slow and light vibration example
                    if (!ViAppleGrabSound.TutorialStageStarted)
                    {
                        _sound.Tutorial(2);
                    }

                    if (ViAppleGrabSound.TutorialStageComplete)
                    {
                        _input.Controllers.StartRumble(ControllerIndex.RightHand, 80);
                        _input.Controllers.StartRumble(ControllerIndex.LeftHand, 80);

                        Thread.Sleep(1500);

                        for (int i = 0; i < 2; i++)
                        {
                            _input.Controllers.StopRumbles();

                            Thread.Sleep(1500);

                            _input.Controllers.StartRumble(ControllerIndex.RightHand, 80);
                            _input.Controllers.StartRumble(ControllerIndex.LeftHand, 80);

                            Thread.Sleep(1500);
                        }

                        _input.Controllers.StopRumbles();
                        Thread.Sleep(750);

                        ViAppleGrabSound.TutorialStageStarted = false;
                        _instructionsStage++;
                    }
                    break;

                case 3://Two types of apples...
                    if ((GameType)_settings.GAME_TYPE == GameType.ApplesAndRottenApples)
                    {
                        if (!ViAppleGrabSound.TutorialStageStarted)
                        {
                            _sound.Tutorial(3);
                        }

                        if (ViAppleGrabSound.TutorialStageComplete)
                        {
                            ViAppleGrabSound.TutorialStageStarted = false;
                            _instructionsStage++;
                        }
                    }
                    else
                        _instructionsStage++;
                    break;

                case 4://What happens when you find an apple
                    if (!ViAppleGrabSound.TutorialStageStarted)
                    {
                        _sound.Tutorial(4);
                    }

                    if (ViAppleGrabSound.TutorialStageComplete)
                    {
                        ViAppleGrabSound.TutorialStageStarted = false;
                        _instructionsStage++;
                    }
                    break;

                case 5://Apple found vibration
                    if (!ViAppleGrabSound.TutorialStageStarted)
                    {
                        _sound.Tutorial(5);
                    }

                    if (ViAppleGrabSound.TutorialStageComplete)
                    {
                        uint intensity = 0;
                        for (int i = 0; i < 45; i++)
                        {
                            intensity = (intensity + 20) % 255;

                            _input.Controllers.StartRumble(ControllerIndex.RightHand, intensity);
                            _input.Controllers.StartRumble(ControllerIndex.LeftHand, intensity);

                            Thread.Sleep(150);
                        }

                        _input.Controllers.StopRumbles();
                        Thread.Sleep(750);

                        ViAppleGrabSound.TutorialStageStarted = false;
                        _instructionsStage++;
                    }
                    break;

                case 6://What happens when you lose an apple
                    if (!ViAppleGrabSound.TutorialStageStarted)
                    {
                        _sound.Tutorial(6);
                    }

                    if (ViAppleGrabSound.TutorialStageComplete)
                    {
                        ViAppleGrabSound.TutorialStageStarted = false;
                        _instructionsStage++;
                    }
                    break;

                case 7:
                    if ((GameType)_settings.GAME_TYPE == GameType.ApplesAndRottenApples)
                    {
                        if (!ViAppleGrabSound.TutorialStageStarted)
                        {
                            //What happens when you find a rotten apple
                            _sound.Tutorial(7);
                        }

                        if (ViAppleGrabSound.TutorialStageComplete)
                        {
                            ViAppleGrabSound.TutorialStageStarted = false;
                            _instructionsStage++;
                        }
                    }
                    else
                        _instructionsStage++;
                    break;

                case 8:
                    if ((GameType)_settings.GAME_TYPE == GameType.ApplesAndRottenApples)
                    {
                        if (!ViAppleGrabSound.TutorialStageStarted)
                        {
                            //Rotten apple found vibration
                            _sound.Tutorial(8);
                        }

                        if (ViAppleGrabSound.TutorialStageComplete)
                        {
                            uint intensity = 255;
                            for (int i = 0; i < 45; i++)
                            {
                                intensity = (intensity - 20) % 255;

                                _input.Controllers.StartRumble(ControllerIndex.RightHand, intensity);
                                _input.Controllers.StartRumble(ControllerIndex.LeftHand, intensity);

                                Thread.Sleep(150);
                            }

                            _input.Controllers.StopRumbles();
                            Thread.Sleep(750);

                            ViAppleGrabSound.TutorialStageStarted = false;
                            _instructionsStage++;
                        }
                    }
                    else
                        _instructionsStage++;
                    break;

                case 9://How to grab an apple
                    if (!ViAppleGrabSound.TutorialStageStarted)
                    {
                        _sound.Tutorial(9);
                    }

                    if (ViAppleGrabSound.TutorialStageComplete)
                    {
                        ViAppleGrabSound.TutorialStageStarted = false;
                        _instructionsStage++;
                    }
                    break;

                case 10://How to grab a rotten apple
                    if ((GameType)_settings.GAME_TYPE == GameType.ApplesAndRottenApples)
                    {
                        if (!ViAppleGrabSound.TutorialStageStarted)
                        {
                            _sound.Tutorial(10);
                        }

                        if (ViAppleGrabSound.TutorialStageComplete)
                        {
                            ViAppleGrabSound.TutorialStageStarted = false;
                            _instructionsStage++;
                        }
                    }
                    else
                        _instructionsStage++;
                    break;

                case 11://How scoring works
                    if (!ViAppleGrabSound.TutorialStageStarted)
                    {
                        _sound.Tutorial(11);
                    }

                    if (ViAppleGrabSound.TutorialStageComplete)
                    {
                        ViAppleGrabSound.TutorialStageStarted = false;
                        _instructionsStage++;
                    }
                    break;

                case 12:
                    if (!ViAppleGrabSound.TutorialStageStarted)
                    {
                        _sound.Tutorial(12);
                    }

                    if (ViAppleGrabSound.TutorialStageComplete)
                    {
                        ViAppleGrabSound.TutorialStageStarted = false;
                        _instructionsStage++;
                    }
                    break;

                case 13:
                    if (!ViAppleGrabSound.TutorialStageStarted)
                    {
                        _sound.Tutorial(13);
                    }

                    if (ViAppleGrabSound.TutorialStageComplete)
                    {
                        ViAppleGrabSound.TutorialStageStarted = false;
                        _instructionsStage++;
                    }
                    break;

                case 14:
                    if (!ViAppleGrabSound.TutorialStageStarted)
                    {
                        _sound.Tutorial(14);
                    }

                    if (ViAppleGrabSound.TutorialStageComplete)
                    {
                        ViAppleGrabSound.TutorialStageStarted = false;
                        _instructionsStage++;
                    }
                    break;

                case 15:
                    if (!ViAppleGrabSound.TutorialStageStarted)
                    {
                        _sound.Tutorial(15);
                    }

                    if (ViAppleGrabSound.TutorialStageComplete)
                    {
                        ViAppleGrabSound.TutorialStageStarted = false;
                        _instructionsStage++;
                    }
                    break;

                case 16://Which controller do you use?
                    if (!ViAppleGrabSound.TutorialStageStarted)
                    {
                        if ((ControlType)_settings.CONTROL_TYPE == ControlType.Together)
                        {
                            _sound.Tutorial(16);
                        }
                        else
                        {
                            _sound.Tutorial(17);
                        }
                    }

                    if (ViAppleGrabSound.TutorialStageComplete)
                    {
                        ViAppleGrabSound.TutorialStageStarted = false;
                        _instructionsStage++;
                    }
                    break;

                case 17://How to pause or end the game
                    if (!ViAppleGrabSound.TutorialStageStarted)
                    {
                        _sound.Tutorial(18);
                    }

                    if (ViAppleGrabSound.TutorialStageComplete)
                    {
                        ViAppleGrabSound.TutorialStageStarted = false;
                        _instructionsStage++;
                    }
                    break;

                case 18://End of tutorial
                    if (!ViAppleGrabSound.TutorialStageStarted)
                    {
                        _sound.Tutorial(19);
                    }

                    if (ViAppleGrabSound.TutorialStageComplete)
                    {
                        ViAppleGrabSound.TutorialStageStarted = false;
                        _instructionsStage = 0;
                        State = GameState.Loading;
                    }
                    break;
            }
        }

        public void StopInstructions()
        {
            if (_instructionsStage != 0)
                _sound.StopTutorial(_instructionsStage);

            _input.Controllers.StopRumbles();

            State = GameState.Loading;
        }

        /// <summary>
        /// This function may be called from other game components and is used 
        /// to restart the game at level 1 with the initial settings. This will
        /// cause the initial menu to be read again allowing the user to listen 
        /// to the rules and instructions again or recalibrate the system
        /// </summary>
        public void RestartGame()
        {
            OutputGameEndInfo();
            Score = 0;
            State = GameState.Loading;
        }

        public void NewGame()
        {
            Score = 0; 
            scorePerApple = _settings.INITIAL_SCORE_PER_APPLE;
            State = GameState.Loading;
        }

        /// <summary>
        /// This function may be called from other game components and is used
        /// to pause the game. This will trigger the GamePausedEvent event in
        /// order to allow any subscribing game components to update their
        /// states based on the game being paused.
        /// </summary>
        public void PauseGame()
        {
            if (State == GameState.Active)
            {
                State = GameState.Paused;
                _input.Controllers.PauseRumbles();
            }
        }

        /// <summary>
        /// This function may be called from other game components and is used
        /// to turn game play back on after the game has been paused. This will
        /// trigger the GameUnpausedEvent event in order to allow any 
        /// subscribing game components to udate their states based on the game
        /// being unpaused.
        /// </summary>
        public void ResumeGame()
        {
            if (State == GameState.Paused)
            {
                State = GameState.Active;
                _input.Controllers.ResumeRumbles();
            }
        }

        /// <summary>
        /// This function sets up the game for the next level. This is only 
        /// called by the GameLevel property when the level is incremented
        /// </summary>
        /// <param name="newLevel">
        /// The level that is about to start, NOT the level that just ended
        /// </param>
        private void _startNewLevel(int levelToStart)
        {
            if (_settings.TIMED_GAME)
            {
                //Set the LevelDuration property
                LevelDuration = _settings.BASE_LEVEL_TIME
                           - _settings.TIME_DECREASE_PER_LEVEL * (levelToStart - 1);

                //Set the RemainingTime property to the value of the LevelDuration
                RemainingTime = LevelDuration;

                AliveTime = _settings.BASE_ALIVE_TIME
                     - _settings.ALIVE_TIME_DECREASE_PER_LEVEL * (levelToStart - 1);

                RemainingAliveTime = AliveTime;


                //Start the timer for this level
                if (secondTimer == null)
                    secondTimer = new Timer(delegate { RemainingTime--; }, null, 0, 1000);
                else
                    secondTimer.Change(1000, 1000);
            }
            else
            {
                RemainingTargets = _settings.TARGETS_PER_LEVEL;
            }
        }

        /// <summary>
        /// This function may be called from other game components and is used
        /// to put the game in the 'Game Over' state. This will trigger the 
        /// GameOverEvent event allowing any subscribing game components to 
        /// update their states based on the game being in the GameOver state.
        /// </summary>
        public void EndGame()
        {
            //Make sure the controllers have stopped rumbling
            _input.Controllers.StopRumbles();

            //Output the trace info
            OutputGameEndInfo();

            if (secondTimer != null)
            {
                secondTimer.Dispose();
                secondTimer = null;
            }

            State = GameState.GameOver;
        }

        /// <summary>
        /// This function may be called from other game components and will 
        /// trigger the GameShutDownEvent event in order to allow any 
        /// subscribing component to perform shut down activities before the
        /// application closes.
        /// </summary>
        public void ShutDown()
        {
            if (secondTimer != null)
            {
                secondTimer.Dispose();
                secondTimer = null;
            }

            State = GameState.ShutDown;

            Debug.WriteLine("Logic component shut down");

            //Now that all other components have shut down, kill the game
            Game.Exit();
        }

        /// <summary>
        /// This function may be called from other game components whenever a 
        /// fatal error has occurred. By calling this function, the error
        /// message will be displayed to the user, and the program will then be
        /// shutdown.
        /// </summary>
        /// <param name="message"></param>
        public void ReportSystemError(string message, string suggestions)
        {
            //The error message must be set before the state is changed
            _errorMessage = message;
            _fixMessage = suggestions;

            State = GameState.Error;
        }        

        #endregion
    }
}
