using System.Diagnostics;
using Microsoft.Xna.Framework;
using ViAppleGrab.Properties;
using ViToolkit.SoundManagement;
using System.Collections.Generic;
using System.Threading;
using System;

namespace ViAppleGrab
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ViAppleGrabSound : Microsoft.Xna.Framework.GameComponent
    {
        private ViAppleGrabLogic _logic;
        private static List<Sound> _backgrounds = new List<Sound>();
        private static List<Sound> _calInstructions = new List<Sound>();
        private static List<Sound> _welcomeSequence = new List<Sound>();
        private static List<Sound> _tutorial = new List<Sound>();
        private static List<Sound> _levels = new List<Sound>();
        private static Sound _backgroundSound;
        private static float _backgroundVolHigh = 0.2f;
        private static float _backgroundVolLow = 0.05f;
        private static float _defaultVol = 0.5f;
        private static Sound _previousBackground = null;
        private static Sound _timeAlertSound;
        private static Sound _scoreDecreasedSound;
        private static Sound _scoreIncreasedSound;
        private static Sound _targetFound;
        private static Sound _rottenFound;
        private static Sound _targetLost;
        private static Sound _targetCollected;
        private static Sound _targetMissed;
        private static Sound _nextTarget;

        public static bool TutorialStageComplete = false;
        public static bool TutorialStageStarted = false;

        private bool _canPlayTargetFound = true;
        private bool _canPlayTargetLost = false;

        private static Thread watcher = null;

        private Random rand = new Random();

        public ViAppleGrabSound(Game game)
            : base(game)
        {
            Enabled = true;
            UpdateOrder = 3;

            Debug.WriteLine("Sound component created");
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            _logic = (ViAppleGrabLogic)this.Game.Components[(int)ComponentIndex.Logic];

            //Subscribe to events within other game components
            _logic.GameLevelChanged += new ViAppleGrabLogic.GameLevelChangedEventHandler(GameLevelChanged);
            _logic.GamePaused += new ViAppleGrabLogic.GamePausedEventHandler(GamePausedEvent);
            _logic.GameResumed += new ViAppleGrabLogic.GameResumedEventHandler(GameUnpausedEvent);
            _logic.GameOverEvent += new ViAppleGrabLogic.GameOverEventHandler(GameOverEvent);
            _logic.GameShutDownEvent += new ViAppleGrabLogic.GameShutDownEventHandler(GameShutDownEvent);
            _logic.ScoreIncreased += new ViAppleGrabLogic.ScoreIncreasedEventHandler(ScoreIncreased);
            _logic.ScoreDecreased += new ViAppleGrabLogic.ScoreDecreasedEventHandler(ScoreDecreased);
            _logic.TimeIsLowAlert += new ViAppleGrabLogic.TimeIsLowAlertEvent(TimeIsLowAlert);
            _logic.GameCalibrating += new ViAppleGrabLogic.GameCalibratingEventHandler(GameCalibrating);
            //_logic.TargetCollecting += new ViAppleGrabLogic.TargetCollectingEventHandler(TargetCollecting);
            //_logic.TargetCollected += new ViAppleGrabLogic.TargetCollectedEventHandler(TargetCollected);
            //_logic.TargetFound += new ViAppleGrabLogic.TargetFoundEventHandler(TargetFound);
            //_logic.TargetLost += new ViAppleGrabLogic.TargetLostEventHandler(TargetLost);
            _logic.TargetMissed += new ViAppleGrabLogic.TargetMissedEventHandler(TargetMissed);

            LoadContent();

            base.Initialize();

            Debug.WriteLine("Sound component initialized");
        }

        /// <summary>
        /// This function is responsible for loading all the game's sound
        /// content.
        /// </summary>
        public void LoadContent()
        {
            for (int i = 1; i < Settings.Default.MAX_LEVELS + 1; i++)
                _backgrounds.Add(new Sound(@"Backgrounds\Background" + i.ToString(), Game.Content, _backgroundVolHigh));

            for (int i = 0; i < 7; i++)
                _calInstructions.Add(new Sound(@"Calibration\Calibration" + i.ToString(), Game.Content, _defaultVol));

            for (int i = 0; i < 4; i++)
                _welcomeSequence.Add(new Sound(@"WelcomeSequence\Welcome" + i.ToString(), Game.Content, _defaultVol));

            for (int i = 0; i < 20; i++)
                _tutorial.Add(new Sound(@"Tutorial\Tutorial" + i.ToString(), Game.Content, _defaultVol));

            for (int i = 1; i < 5; i++)
                _levels.Add(new Sound(@"Levels\Level" + i.ToString(), Game.Content, _defaultVol));

            _backgroundSound = _backgrounds[0];
            _timeAlertSound = new Sound(@"GameEvents\3Beeps", Game.Content, _defaultVol);
            _scoreDecreasedSound = new Sound(@"GameEvents\NegativeBeep", Game.Content, _defaultVol);
            _scoreIncreasedSound = new Sound(@"GameEvents\AppleCrunch3", Game.Content, _defaultVol);
            _targetCollected = new Sound(@"GameEvents\WayToGo", Game.Content, _defaultVol);
            _targetFound = new Sound(@"GameEvents\TargetFound", Game.Content, _defaultVol);
            _rottenFound = new Sound(@"GameEvents\YuckItsRotten", Game.Content, _defaultVol);
            _targetLost = new Sound(@"GameEvents\SadWhistle", Game.Content, _defaultVol);
            _targetMissed = new Sound(@"GameEvents\OopsYouMissedIt", Game.Content, _defaultVol);
            _nextTarget = new Sound(@"Levels\NextTarget", Game.Content, _defaultVol);
            
            Debug.WriteLine("Sound content has been loaded");
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //Update the fading in or out of the background music
            if (_previousBackground != null && _previousBackground.IsFadingOut)
                _previousBackground.UpdateFade();

            if( _backgroundSound != null && _backgroundSound.IsFadingIn)
                _backgroundSound.UpdateFade();

            base.Update(gameTime);
        }

        #region EVENT HANDLERS
        /// <summary>
        /// Event handler called when the level of the game changes. This changes the background
        /// music based on what the current level is
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GameLevelChanged(int newLevel)
        {
            if (_logic.State == GameState.Active)
            {
                _levels[newLevel - 1].BlockPlay();

                //Fade out the previous background music
                if (_backgrounds[newLevel - 1] != _backgroundSound)
                {
                    if (_backgroundSound != null)
                    {
                        _previousBackground = _backgroundSound;

                        _previousBackground.Stop();
                    }

                    //Fade in the new background music
                    _backgroundSound = _backgrounds[newLevel - 1];

                    _backgroundSound.PlayLoop();
                }
                else
                {
                    _backgroundSound.Restart();
                }               

                Debug.WriteLine("Level " + newLevel + " background music is now playing");
            }
        }

        /// <summary>
        /// Event handler called when the game is paused. This pauses the background music and plays 
        /// the game paused music.
        /// </summary>
        void GamePausedEvent()
        {
            _backgroundSound.Pause();

            Debug.WriteLine(@"The background music is now paused...");

            Debug.WriteLine(@"'Game Paused' music is now playing...");
        }

        /// <summary>
        /// Event handler called whent the game is unpaused. This turns the background music back on.
        /// </summary>
        void GameUnpausedEvent()
        {
            Debug.WriteLine(@"The 'Game Paused' music is now off...");

            _backgroundSound.Resume();

            Debug.WriteLine(@"The background music is now playing...");
        }

        /// <summary>
        /// Event handler called when the game is over. This changes the background music to the
        /// game over music.
        /// </summary>
        void GameOverEvent()
        {
            _previousBackground = _backgroundSound;
            _previousBackground.Stop();

            //This should be set to the game over music
            _backgroundSound = null;

            for (int i = 0; i < 7; i++)
            {
                if (_calInstructions[i].IsPlaying)
                    _calInstructions[i].StopNoFade();
            }

            for (int i = 0; i < 4; i++)
            {
                if (_welcomeSequence[i].IsPlaying)
                    _welcomeSequence[i].StopNoFade();
            }

            Debug.WriteLine(@"Sad 'Game Over' music is now playing...");
        }

        /// <summary>
        /// Event handler called when the game is shutting down. This disposes of all sound objects
        /// that need to be physically shut down before the application exits.
        /// </summary>
        void GameShutDownEvent()
        {
            //Dispose of all sound objects that need to be physically shut down...

            Debug.WriteLine(@"Sound component shut down");
        }

        void ScoreDecreased()
        {
            watcher = new Thread(new ParameterizedThreadStart(WatcherCallback));
            watcher.Start(_scoreDecreasedSound);

            Debug.WriteLine("Score decreased sound played.");
        }

        void ScoreIncreased(int increase)
        {
            if (_backgroundSound != null)
                _backgroundSound.Volume = _backgroundVolLow;

            _scoreIncreasedSound.BlockPlay();

            Speech.Speak(100, increase.ToString() + " points");

            Speech.Speak(100, "Next Target");

            if (_backgroundSound != null)
                _backgroundSound.Volume = _backgroundVolHigh;

            Debug.WriteLine("Score increased sound played.");
        }

        void TargetCollecting()
        {
            //Stop any other target sounds if any are playing
            StopTargetSounds();
        }

        void TargetCollected()
        {
            //Play the target collected sound

            if (rand.Next(5) % 4 == 0)
            {
                if (_backgroundSound != null)
                {
                    float prevVol = _backgroundSound.Volume;

                    _backgroundSound.Volume = _backgroundVolLow;
                }

                _targetCollected.BlockPlay();

                if (_backgroundSound != null)
                    _backgroundSound.Volume = _backgroundVolHigh;
            }


            _canPlayTargetLost = false;
            _canPlayTargetFound = true;

            Debug.WriteLine("Target collected sound played.");
        }

        void TargetMissed()
        {
            //Play the target missed sound

            watcher = new Thread(new ParameterizedThreadStart(WatcherCallback));
            watcher.Start(_targetMissed);

            _canPlayTargetLost = false;
            _canPlayTargetFound = true;

            Debug.WriteLine("Target missed sound played.");
        }

        void TargetFound(bool IsRotten)
        {
            if (_canPlayTargetFound)
            {
                //Stop any other target sounds if any are playing

                //StopTargetSounds();

                //watcher = new Thread(new ParameterizedThreadStart(WatcherCallback));

                //if (!IsRotten)
                //    watcher.Start(_targetFound);
                //else
                //    watcher.Start(_rottenFound);

                //_canPlayTargetFound = false;
                //_canPlayTargetLost = true;

                //Debug.WriteLine("Target found sound played.");
            }
        }

        void TargetLost()
        {
            if (_canPlayTargetLost)
            {
                //Stop any other target sounds if any are playing

                //StopTargetSounds();

                //watcher = new Thread(new ParameterizedThreadStart(WatcherCallback));
                //watcher.Start(_targetLost);

                //_canPlayTargetLost = false;
                //_canPlayTargetFound = true;

                //Debug.WriteLine("Target lost sound played.");
            }
        }

        private void StopTargetSounds()
        {
            if (watcher != null && !watcher.IsAlive)
            {
                if (_targetCollected.IsPlaying)
                    _targetCollected.Stop();

                if (_targetFound.IsPlaying)
                    _targetFound.Stop();

                if (_targetLost.IsPlaying)
                    _targetLost.Stop();
            }
        }

        void TimeIsLowAlert(int secondsRemaining)
        {
            watcher = new Thread(new ParameterizedThreadStart(WatcherCallback));
            watcher.Start(_timeAlertSound);

            Debug.WriteLine("Time warning sound played. " 
                            + secondsRemaining.ToString() 
                            + " seconds left in this round!");
        }

        void GameCalibrating()
        {
            if (_backgroundSound != null)
                _backgroundSound.Stop();

            Debug.WriteLine("Game calibrating sound is now playing...");
        }

        public void CalInstructions(int stage)
        {
            _calInstructions[stage].BlockPlay();
        }

        public void GameWelcome(int stage, bool playBlock)
        {
            if (playBlock)
            {
                _welcomeSequence[stage].BlockPlay();
            }
            else
            {
                _welcomeSequence[stage].PlayOnce();
                Thread.Sleep(500);
            }
        }

        public void GameWelcomeStop(int stage)
        {
            if (_welcomeSequence[stage].IsPlaying)
                _welcomeSequence[stage].StopNoFade();
        }

        public void Tutorial(int stage)
        {
            TutorialStageComplete = false;
            TutorialStageStarted = true;

            watcher = new Thread(new ParameterizedThreadStart(TutorialCallback));
            watcher.Start(new TutorialCallbackObject {foreground = _tutorial[stage], stageNum = stage});
        }

        static void TutorialCallback(object o)
        {
            //float prevVol = _backgroundSound.Volume;

            //_backgroundSound.Volume = _backgroundVolLow;
            
            TutorialCallbackObject obj = (TutorialCallbackObject)o;

            obj.foreground.PlayOnce();

            while (obj.foreground.IsPlaying) { };

            //_backgroundSound.Volume = _backgroundVolHigh;

            switch (obj.stageNum)
            {
                case 4:
                    Thread.Sleep(500);
                    _targetFound.BlockPlay();
                    break;
                case 6:
                    Thread.Sleep(500);
                    _targetLost.BlockPlay();
                    break;
                case 7:
                    Thread.Sleep(500);
                    _rottenFound.BlockPlay();
                    break;
                case 13:
                    Thread.Sleep(500);
                    _scoreIncreasedSound.BlockPlay();
                    break;
                case 14:
                    Thread.Sleep(500);
                    _scoreDecreasedSound.BlockPlay();
                    break;
                case 15:
                    Thread.Sleep(500);
                    _timeAlertSound.BlockPlay();
                    break;
            }

            Thread.Sleep(750);

            TutorialStageComplete = true;
        }

        public void StopTutorial(int stage)
        {
            if (_tutorial[stage].IsPlaying)
                _tutorial[stage].StopNoFade();

            watcher.Abort();
        }

        static void WatcherCallback(object foreground)
        {
            if (_backgroundSound != null)
            {
                float prevVol = _backgroundSound.Volume;

                _backgroundSound.Volume = _backgroundVolLow;
            }

            ((Sound)foreground).PlayOnce();

            while (((Sound)foreground).IsPlaying) { };

            if(_backgroundSound != null)
                _backgroundSound.Volume = _backgroundVolHigh;
        }

        #endregion
        
    }

    public class TutorialCallbackObject
    {
        public Sound foreground;
        public int stageNum;
    }
}
