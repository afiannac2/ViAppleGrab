using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using ViAppleGrab.Properties;
using Microsoft.Xna.Framework;
using ViToolkit.Logging;
using System.Xml;

namespace ViAppleGrab
{
    public class Target
    {
        #region STATIC FIELDS AND CONSTRUCTOR

        public static int _uniqueTargetID = 0;
        private static Point[] _targetLocations;
        private static bool[] _rottenState;
        private static int _totalTargets = 41;

        /// <summary>
        /// This instanciates the static _targets array
        /// </summary>
        static Target()
        {
            //The PREDEFINED_TARGETS setting needs to be true and the targets file
            //  needs to exist, otherwise the targets will be generated randomly
            if (Settings.Default.PREDEFINED_TARGETS)
            {
                if(ViAppleGrabInput.GameHasFocus)
                    LoadTargets();
            }
            else
            {
                GenerateNewTargets();
            }
        }

        public static void LoadTargets()
        {
            if ((File.Exists(Settings.Default.ALTERNATING_FILE) && (ControlType)Settings.Default.CONTROL_TYPE == ControlType.Alternating && !Settings.Default.SINGLE_TARGET)
                || (File.Exists(Settings.Default.TOGETHER_FILE) && (ControlType)Settings.Default.CONTROL_TYPE == ControlType.Together && !Settings.Default.SIMULTANEOUS_TARGETS)
                || (File.Exists(Settings.Default.SINGLE_FILE) && Settings.Default.SINGLE_TARGET)
                || (File.Exists(Settings.Default.SIMULTANEOUS_FILE) && Settings.Default.SIMULTANEOUS_TARGETS))
            {
                int x, y;
                bool r;

                TextReader reader;

                if ((ControlType)Settings.Default.CONTROL_TYPE == ControlType.Alternating && !Settings.Default.SINGLE_TARGET)
                    reader = new StreamReader(Settings.Default.ALTERNATING_FILE);
                else if((ControlType)Settings.Default.CONTROL_TYPE == ControlType.Alternating && Settings.Default.SINGLE_TARGET)
                    reader = new StreamReader(Settings.Default.SINGLE_FILE);
                else if(Settings.Default.SIMULTANEOUS_TARGETS)
                    reader = new StreamReader(Settings.Default.SIMULTANEOUS_FILE);
                else
                    reader = new StreamReader(Settings.Default.TOGETHER_FILE);

                string text = reader.ReadLine();

                if (text != ((ControlType)Settings.Default.CONTROL_TYPE).ToString())
                {
                    reader.Close();
                    GenerateNewTargets();
                }
                else
                {
                    text = reader.ReadLine();

                    List<string[]> targetList = new List<string[]>();

                    while (text != null)
                    {
                        targetList.Add(text.Split(new[] { ' ' }));
                        text = reader.ReadLine();
                    }

                    _totalTargets = targetList.Count;
                    Settings.Default.TARGETS_PER_LEVEL = _totalTargets / Settings.Default.MAX_LEVELS;

                    _targetLocations = new Point[_totalTargets];
                    _rottenState = new bool[_totalTargets];

                    for (int i = 0; i < _totalTargets; i++)
                    {
                        x = Convert.ToInt32((targetList[i])[0]);
                        y = Convert.ToInt32((targetList[i])[1]);
                        r = Convert.ToBoolean((targetList[i])[2]);

                        _targetLocations[i] = new Point(x, y);
                        _rottenState[i] = r;
                    }

                    reader.Close();

                    _uniqueTargetID = 0;
                }
            }
            else
            {
                GenerateNewTargets();
            }
        }

        public static void GenerateNewTargets()
        {            
            Random rand = new Random();
            int mid = Settings.Default.SCREEN_WIDTH / 2;
            int minX = 40;
            int minY = 50;
            int maxX = Settings.Default.SCREEN_WIDTH - minX;
            int maxY = Settings.Default.SCREEN_HEIGHT - minY;

            _targetLocations = new Point[_totalTargets];
            _rottenState = new bool[_totalTargets];

            if ((ControlType)Settings.Default.CONTROL_TYPE == ControlType.Alternating)
            {
                if (!Settings.Default.SINGLE_TARGET)
                {
                    for (int i = 0; i < _totalTargets; i++)
                    {
                        if (i % 2 == 0) //Start with the right hand by default
                        {
                            _targetLocations[i] = new Point((int)rand.Next(mid, maxX), (int)rand.Next(minY, maxY));
                        }
                        else //then the left hand
                        {
                            _targetLocations[i] = new Point((int)rand.Next(minX, mid), (int)rand.Next(minY, maxY));
                        }

                        //33.3% chance of an apple being rotten
                        _rottenState[i] = ((int)rand.Next(0, 3) == 2) ? true : false;
                    }
                }
                else
                {
                    for (int i = 0; i < _totalTargets; i++)
                    {
                        if (Settings.Default.DOMINANT_ARM == "right") //Start with the right hand by default
                        {
                            _targetLocations[i] = new Point((int)rand.Next(mid, maxX), (int)rand.Next(minY, maxY));
                        }
                        else //then the left hand
                        {
                            _targetLocations[i] = new Point((int)rand.Next(minX, mid), (int)rand.Next(minY, maxY));
                        }

                        //33.3% chance of an apple being rotten
                        _rottenState[i] = ((int)rand.Next(0, 3) == 2) ? true : false;
                    }
                }

                using (TextWriter writer = new StreamWriter("Targets_Alternating.txt"))
                {
                    writer.WriteLine(((ControlType)Settings.Default.CONTROL_TYPE).ToString());

                    for (int i = 0; i < _totalTargets; i++)
                        writer.WriteLine(_targetLocations[i].X.ToString() + " "
                            + _targetLocations[i].Y.ToString() + " "
                            + _rottenState[i].ToString());

                    writer.Close(); 
                }
            }
            else
            {
                if (Settings.Default.SIMULTANEOUS_TARGETS)
                {
                    _totalTargets = 81;
                    _targetLocations = new Point[_totalTargets];
                    _rottenState = new bool[_totalTargets];

                    for (int i = 0; i < _totalTargets; i++)
                    {
                        if (i % 2 == 0) //Start with the right hand by default
                        {
                            _targetLocations[i] = new Point((int)rand.Next(mid, maxX), (int)rand.Next(minY, maxY));
                        }
                        else //then the left hand
                        {
                            _targetLocations[i] = new Point((int)rand.Next(minX, mid), (int)rand.Next(minY, maxY));
                        }

                        //33.3% chance of an apple being rotten
                        _rottenState[i] = ((int)rand.Next(0, 3) == 2) ? true : false;
                    }
                }
                else
                {
                    for (int i = 0; i < _totalTargets; i++)
                    {
                        _targetLocations[i] = new Point((int)rand.Next(minX, maxX), (int)rand.Next(minY, maxY));

                        //33.3% chance of an apple being rotten
                        _rottenState[i] = ((int)rand.Next(0, 3) == 2) ? true : false;
                    }
                }

                using (TextWriter writer = new StreamWriter("Targets_Together.txt"))
                {
                    writer.WriteLine(((ControlType)Settings.Default.CONTROL_TYPE).ToString());

                    for (int i = 0; i < _totalTargets; i++)
                        writer.WriteLine(_targetLocations[i].X.ToString() + " "
                            + _targetLocations[i].Y.ToString() + " "
                            + _rottenState[i].ToString());

                    writer.Close();
                }
            }            
        }

        #endregion

        private int _targetID;
        public int ID { get { return _targetID; } }

        #region LOCATION DATA

        private Point _location;
        
        public Point Location
        {
            get
            {
                return _location;
            }
        }

        public int x
        {
            get
            {
                return _location.X;
            }
            private set
            {
                _location.X = value;
            }
        }

        public int y
        {
            get
            {
                return _location.Y;
            }
            set
            {
                _location.Y = value;
            }
        }

        private Rectangle _area;

        public Rectangle Area
        {
            get
            {
                return _area;
            }
        }

        #endregion

        #region TARGET STATE

        private TargetState _state = TargetState.Inactive;
        public TargetState State { get { return _state; } }

        public bool IsFound
        {
            get
            {
                return (_state == TargetState.Found);
            }
            set
            {
                if (value)
                {
                    WasLost = false;
                    Found();
                }
                else
                {
                    if (_state == TargetState.Found)
                    {
                        WasLost = true;
                        _state = TargetState.Active;
                    }
                    else
                        WasLost = false;
                }
            }
        }

        public bool WasLost { get; private set; }

        private bool _isCollecting = false;
        public bool IsCollecting
        {
            get
            {
                return (_state == TargetState.Collecting);
            }
            set
            {
                if (value && value != _isCollecting)
                {
                    _isCollecting = value;
                    Collecting();
                }
            }
        }

        private bool _wasCollected = false;
        public bool WasCollected
        {
            get
            {
                return (_state == TargetState.Collected);
            }
            set
            {
                if (value)
                {
                    Collected();
                }
                else
                {
                    Missed();
                }

                _wasCollected = value;
            }
        }
        private bool _isDuplicate = false;
        public bool IsDuplicate { get { return _isDuplicate; } }

        private bool _isRightHand = true;
        public bool IsRightHand { get { return _isRightHand; } }

        private bool _isRotten = false;
        public bool IsRotten { get { return _isRotten; } }

        #endregion
        
        #region ANALYTICS

        public DateTime _targetSpawnTime;
        private TimeSpan _scanningTime;
        private TimeSpan _collectingTime;
        private TimeSpan _TotalAliveTime;
        private DateTime _pauseStartTime;
        private TimeSpan _pausedTime;
        public TimeSpan AliveTime
        {
            get
            {
                return _scanningTime + _collectingTime;
            }
        }

        #endregion

        #region TARGET METHODS

        public Target(object o)
        {
            _targetID = -1;
        }

        /// <summary>
        /// This creates a new target for the controller and returns the ID of 
        /// the target. If two controllers are going for the same target then
        /// simply call the overload of the NewTarget function which accepts
        /// a target ID and pass it the target ID returned from this function
        /// </summary>
        /// <returns>Target ID</returns>
        public Target(bool right)
        {
            _targetID = _uniqueTargetID;
            _isRightHand = right;

            x = _targetLocations[_targetID].X;
            y = _targetLocations[_targetID].Y;

            WasLost = false;

            _area = new Rectangle(x - 40, y - 50, 80, 100);

            //Only set to rotten if the game type allows for rotten apples
            _isRotten = _rottenState[_targetID] && (GameType)Settings.Default.GAME_TYPE == GameType.ApplesAndRottenApples;

            _targetSpawnTime = DateTime.Now;

            //There are only so many targets precreated so mod around them
            //_uniqueTargetID = (_uniqueTargetID + 1) % _totalTargets;
            _uniqueTargetID++;

            //Write the trace data for this target
            XmlNode node = XMLTrace.FindLastTargetNode();
            XmlNode child = XMLTrace.AppendSubchild(node, "TargetData", "");
            XMLTrace.AddAttributes(child, new Dictionary<string, string> 
                { 
                    { "ID", _targetID.ToString() }, 
                    { "Controller", "" }, 
                    { "Time", _targetSpawnTime.ToString("O") } ,
                    { "X", x.ToString() },
                    { "Y", y.ToString() },
                    { "IsRotten", _isRotten.ToString() }
                });
        }

        /// <summary>
        /// This sets the controller's target to the same target as another
        /// controller
        /// </summary>
        /// <param name="targetID">The id of the target to duplicate</param>
        public Target(int ID)
        {
            _isDuplicate = true;
            _isRightHand = false;

            _targetID = ID;

            x = _targetLocations[_targetID].X;
            y = _targetLocations[_targetID].Y;

            WasLost = false;

            _area = new Rectangle(x - 40, y - 50, 80, 100);

            //Only set to rotten if the game type allows for rotten apples
            _isRotten = _rottenState[_targetID] && (GameType)Settings.Default.GAME_TYPE == GameType.ApplesAndRottenApples;

            _targetSpawnTime = DateTime.Now;
        }

        /// <summary>
        /// This turns the target on
        /// </summary>
        public void Activate()
        {
            _state = TargetState.Active;
        }

        /// <summary>
        /// This turns the target off
        /// </summary>
        public void Deactivate()
        {
            _state = TargetState.Inactive;
        }
        
        /// <summary>
        /// This pauses the target when the game is paused
        /// </summary>
        public void Pause()
        {
            if (_state == TargetState.Active)
            {
                //Take record of the time the target was paused at
                _state = TargetState.Paused;
                _pauseStartTime = DateTime.Now;
            }
        }

        /// <summary>
        /// This resumes the target when the game is resumed
        /// </summary>
        public void Resume()
        {
            if (_state == TargetState.Paused)
            {
                //Correct for the time that the target was paused
                _state = TargetState.Active;
                _pausedTime = DateTime.Now - _pauseStartTime;
                _targetSpawnTime += _pausedTime;
            }
        }

        /// <summary>
        /// Call this function when a controller is pointing at the target and
        /// the trigger has been pulled
        /// </summary>
        private void Found()
        {
            if (_state != TargetState.Found)
            {
                _state = TargetState.Found;
                _scanningTime = DateTime.Now - _targetSpawnTime;
            }
        }

        /// <summary>
        /// Call this function after the target has been acquired and the 
        /// controller has been pulled down
        /// </summary>
        private void Collecting()
        {
            if (_state != TargetState.Collecting)
            {
                _state = TargetState.Collecting;
                _collectingTime = (DateTime.Now - _targetSpawnTime) - _scanningTime;

                //Write the target trace data
                XmlNode node = XMLTrace.FindTargetDataNode(_targetID);

                XmlNode child = XMLTrace.AppendSubchild(node, "ScanningTime", "");
                XMLTrace.AddText(child, _scanningTime.TotalSeconds.ToString());
                XMLTrace.AddAttributes(child, new Dictionary<string, string> { { "Units", "seconds" } });

                child = XMLTrace.AppendSubchild(node, "RecognitionTime", "");
                XMLTrace.AddText(child, _collectingTime.TotalSeconds.ToString());
                XMLTrace.AddAttributes(child, new Dictionary<string, string> { { "Units", "seconds" } });
            }
        }

        private void Collected()
        {
            if (_state != TargetState.Collected)
            {
                _state = TargetState.Collected;

                _TotalAliveTime = DateTime.Now - _targetSpawnTime;

                string c = (_isRightHand) ? "RightHand" : "LeftHand";

                XmlNode node = XMLTrace.FindTargetDataNode(_targetID);
                node.Attributes["Controller"].Value = c;

                XmlNode child = XMLTrace.AppendSubchild(node, "TotalAliveTime", "");
                XMLTrace.AddText(child, _TotalAliveTime.TotalSeconds.ToString());
                XMLTrace.AddAttributes(child, new Dictionary<string, string> 
                { 
                    { "Units", "seconds" }, 
                    { "FinalStatus", "collected" }
                });
            }
        }

        private void Missed()
        {
            if (_state != TargetState.Missed)
            {
                _state = TargetState.Missed;

                _TotalAliveTime = DateTime.Now - _targetSpawnTime;

                string c = (_isDuplicate) ? "LeftHand" : "RightHand";

                XmlNode node = XMLTrace.FindTargetDataNode(_targetID);
                node.Attributes["Controller"].Value = c;

                XmlNode child = XMLTrace.AppendSubchild(node, "TotalAliveTime", "");
                XMLTrace.AddText(child, _TotalAliveTime.TotalSeconds.ToString());
                XMLTrace.AddAttributes(child, new Dictionary<string, string> 
                { 
                    { "Units", "seconds" }, 
                    { "FinalStatus", "missed" } 
                });
            }
        }

        public void TimedOut()
        {
            _TotalAliveTime = DateTime.Now - _targetSpawnTime;

            string c = "N/A";

            XmlNode node = XMLTrace.FindTargetDataNode(_targetID);
            node.Attributes["Controller"].Value = c;

            XmlNode child = XMLTrace.AppendSubchild(node, "TotalAliveTime", "");
            XMLTrace.AddText(child, _TotalAliveTime.TotalSeconds.ToString());
            XMLTrace.AddAttributes(child, new Dictionary<string, string> 
                { 
                    { "Units", "seconds" }, 
                    { "FinalStatus", "timed_out" } 
                });
        }

        #endregion
    }
}
