using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using ViAppleGrab.Properties;
using ViAppleGrab;
using ViToolkit.Logging;
using System.Xml;

namespace ViAppleGrab
{
    /// <summary>
    /// This class stores the data of a controller and the target it is
    /// searching for. In addition, it logs key data about target acquisition
    /// times that can be used for analysis of program results
    /// </summary>
    public class Controller
    {
        #region CONTROLLER AND TARGET LOCATION DATA

        private ControllerIndex ci;
        private HapticFeedbackAxis _axisOfInterest;
        public HapticFeedbackAxis Axis { get { return _axisOfInterest; } }

        private int _locationX = Settings.Default.HAND_SPRITE_SIDE_LENGTH / 2;
        private int maxX, minX;
        public int x
        {
            get
            {
                return _locationX;
            }
            set
            {
                //If the value is going into or out of bounds then toggle the 
                //  IsOutOfBounds property
                if (value < minX || value > maxX)
                {
                    //Moving out of bounds
                    if (_locationX >= minX && _locationX <= maxX)
                        IsOutOfBoundsX = true;
                }

                else if (_locationX < minX || _locationX > maxX)
                {
                    //Moving into bounds
                    if (value >= minX && value <= maxX)
                        IsOutOfBoundsX = false;
                }

                //Set the value of the x location
                _locationX = value;
            }
        }
        private int _locationY = Settings.Default.HAND_SPRITE_SIDE_LENGTH / 2;
        private int maxY, minY;
        public int y
        {
            get
            {
                return _locationY;
            }
            set
            {
                //If the value is going into or out of bounds then toggle the 
                //  IsOutOfBounds property
                if (value < minY || value > maxY)
                {
                    //Moving out of bounds
                    if (_locationY >= minY && _locationY <= maxY)
                        IsOutOfBoundsY = true;
                }

                else if (_locationY < minY || _locationY > maxY)
                {
                    //Moving into bounds
                    if (value >= minY && value <= maxY)
                        IsOutOfBoundsY = false;
                }

                //Set the value of the y location
                _locationY = value;
            }
        }
        private int _locationZ;
        public int z
        {
            get
            {
                return _locationZ;
            }
            private set
            {
                _locationZ = value;
            }
        }

        private XmlNode locNode;
        private int locID;

        /// <summary>
        /// Gets a Point containing the x and y positions of the controller
        /// scaled to the screen
        /// </summary>
        public Point Location
        {
            get
            {
                return new Point(_locationX, _locationY);
            }
        }
        
        private bool _isOutOfBoundsX = false;
        private bool IsOutOfBoundsX
        {
            get
            {
                return _isOutOfBoundsX;
            }
            set
            {
                _isOutOfBoundsX = value;

                IsOutOfBounds = (_isOutOfBoundsX || IsOutOfBoundsY);
            }
        }
        private bool _isOutOfBoundsY = false;
        private bool IsOutOfBoundsY
        {
            get
            {
                return _isOutOfBoundsY;
            }
            set
            {
                _isOutOfBoundsY = value;

                IsOutOfBounds = (_isOutOfBoundsY || IsOutOfBoundsX);
            }
        }
        private bool _isOutOfBounds = false;
        public bool IsOutOfBounds
        {
            get
            {
                return (IsOutOfBoundsX || IsOutOfBoundsY);
            }
            set
            {
                _isOutOfBounds = value;

                if (!_isOutOfBounds)
                {
                    //Turn the tactile feedback back on to signal that the 
                    //  controller is back in bounds
                    _rumbleState = RumbleStates.TurningOn;
                }
                else if (_isOutOfBounds)
                {
                    //Turn off the tactile feedback to signal that the controller
                    //  is out of bounds
                    _rumbleState = RumbleStates.TurningOff;
                }

            }
        }

        public Target Target;
        private bool _trackingApple = false;

        #endregion

        //These fields and properties allow the logic component to detect
        //  whether or not a user successfully got an apple or missed it
        #region TRIGGER DATA

        private InputButtonState _triggerState = InputButtonState.NotPressed;
        public InputButtonState TriggerState
        {
            get
            {
                return _triggerState;
            }
        }

        private int _triggerVal = 0;
        public int TriggerVal
        {
            get
            {
                return _triggerVal;
            }
            set
            {
                if (value != _triggerVal)
                {
                    if (value > 0 && _triggerVal == 0)
                    {
                        _triggerState = InputButtonState.JustPressed;

                        _lastTrigStart.x = x;
                        _lastTrigStart.y = y;
                        _lastTrigStart.z = z;

                        _lastTrigYDelta = 0;
                        _lastTrigZDelta =  0;
                    }
                    else if (value > 0 && _triggerVal > 0)
                    {
                        _triggerState = InputButtonState.StillPressed;
                    }
                    else if (value == 0 && _triggerVal > 0)
                    {
                        _triggerState = InputButtonState.NotPressed;

                        _lastTrigYDelta = _lastTrigStart.y - y;
                        _lastTrigZDelta = _lastTrigStart.z - z;
                    }

                    _triggerVal = value;
                }
            }
        }

        private Point3 _lastTrigStart = new Point3();

        //This is the displacement of the y value from the start to the end of
        //  the "collection" phase...
        private int _lastTrigYDelta = 0;
        public int LastTrigYDelta { get { return _lastTrigYDelta; } }

        //This is the displacement of the x value from the midpoint of the screen
        //  at the end of the "collection" phase...
        private int _lastTrigZDelta = 0;
        public int LastTrigZDelta { get { return _lastTrigZDelta; } }

        #endregion

        #region RUMBLE DATA
        
        //State
        private RumbleStates _rumbleState = RumbleStates.Off;
        public RumbleStates RumbleState
        {
            get
            {
                return _rumbleState;
            }
            set
            {
                _rumbleState = value;
            }
        }

        //Timer
        public DateTime RumbleStageStart;

        //Duration (of both the rumble and pause)
        private TimeSpan _rumbleDuration;
        public TimeSpan RumbleDuration { get { return _rumbleDuration; } }

        private TimeSpan _waitDuration = new TimeSpan(0, 0, 0, 0, 0);
        private TimeSpan _nextWaitDuration;
        public TimeSpan WaitDuration { get { return _waitDuration; } }

        //Intensity (between 0 and 255)
        private uint _rumbleIntensity = 0;
        private uint _nextRumbleIntensity;
        public uint RumbleIntensity 
        { 
            get 
            {
                return _rumbleIntensity;
            }
        }

        #endregion

        #region CONSTRUCTOR AND CONSTR HELPERS

        public Controller(ControllerIndex c)
        {
            ci = c;

            //Assign the primary axis that the controller is concerned with
            AssignPrimaryAxis(c);

            //Set up the limits of the display for this controller
            AssignSpatialLimits(c);

            //Create a dummy target for this controller
            Target = new Target(null);
            Target.Deactivate();
        }

        public void AssignPrimaryAxis(ControllerIndex c)
        {
            ci = c;

            //If the controllers are working in conjunction, one will give y-axis 
            //feedback (frequency) and the other will give x-axis feedback
            //(pulse delay). Which controller is which can be configured in the
            //setttings file (RIGHT_GIVES_Y_FEEDBACK)
            if ((ControlType)Settings.Default.CONTROL_TYPE == ControlType.Alternating
                || ((ControlType)Settings.Default.CONTROL_TYPE == ControlType.Together && Settings.Default.SIMULTANEOUS_TARGETS))
            {
                _axisOfInterest = HapticFeedbackAxis.xy;
            }
            else if ((c == ControllerIndex.RightHand && Settings.Default.RIGHT_GIVES_Y_FEEDBACK) ||
                        (c != ControllerIndex.RightHand && !Settings.Default.RIGHT_GIVES_Y_FEEDBACK))
            {
                _axisOfInterest = HapticFeedbackAxis.y;
            }
            else
            {
                _axisOfInterest = HapticFeedbackAxis.x;
            }


            //Set the default rumble duration based on the required rumbling time
            if (_axisOfInterest == HapticFeedbackAxis.y)
            {
                _rumbleDuration = new TimeSpan(0, 0, 0, 0, Settings.Default.UPDATE_INTERVAL);
            }
            else
            {
                //This always buzzes for 200 ms, it is the wait time (delay) that varies
                _rumbleDuration = new TimeSpan(0, 0, 0, 0, Settings.Default.BUZZ_DURATION);
            }
        }

        public void AssignSpatialLimits(ControllerIndex c)
        {
            //Set up the limits of the display for this controller
            if ((ControlType)Settings.Default.CONTROL_TYPE == ControlType.Alternating
                || ((ControlType)Settings.Default.CONTROL_TYPE == ControlType.Together && Settings.Default.SIMULTANEOUS_TARGETS))
            {
                //Is this the right or left controller?
                if (c == ControllerIndex.RightHand)
                {
                    minX = Settings.Default.SCREEN_WIDTH / 2;
                    maxX = Settings.Default.SCREEN_WIDTH;
                }
                else
                {
                    minX = 0;
                    maxX = Settings.Default.SCREEN_WIDTH - Settings.Default.SCREEN_WIDTH / 2;
                }
            }
            else
            {
                minX = 0;
                maxX = Settings.Default.SCREEN_WIDTH;
            }

            minY = 0;
            maxY = Settings.Default.SCREEN_HEIGHT;
        }

        #endregion

        #region TARGET METHODS

        /// <summary>
        /// Create and activate a new target for either controller (left or right)
        /// </summary>
        /// <returns>Target ID</returns>
        public int NewTarget(Point otherController)
        {
            if (ci == ControllerIndex.RightHand)
            {
                Target = new Target(true);

                XmlNode node = XMLTrace.FindTargetDataNode(Target.ID);
                locNode = XMLTrace.AppendSubchild(node, "RightController", "");
                locID = 0;
            }
            else
            {
                Target = new Target(false);

                XmlNode node = XMLTrace.FindTargetDataNode(Target.ID);
                locNode = XMLTrace.AppendSubchild(node, "LeftController", "");
                locID = 0;
            }

            Target.Activate();
            RumbleState = RumbleStates.TurningOn;
            _trackingApple = true;

            return Target.ID;
        }

        /// <summary>
        /// Create a duplicate target with the same properties as the target with
        /// the input ID. This should only ever be called by the left controller (secondary controller)
        /// </summary>
        /// <param name="ID">Target ID to duplicate</param>
        public void NewTarget(int ID)
        {
            Target = new Target(ID);

            XmlNode node = XMLTrace.FindTargetDataNode(Target.ID);
            locNode = XMLTrace.AppendSubchild(node, "LeftController", "");
            locID = 0;

            Target.Activate();
            RumbleState = RumbleStates.TurningOn;
            _trackingApple = true;
        }

        #endregion

        #region LOCATION UPDATE METHODS
        public void RecordPosition(TimeSpan t)
        {
            if (!_trackingApple)
                return;

            if (locID == 0)
                Target._targetSpawnTime.AddMilliseconds(t.TotalMilliseconds);

            XmlNode n = XMLTrace.AppendSubchild(locNode, "Position", "");
            XMLTrace.AddAttributes(n, new Dictionary<string, string>
            {
                { "X", _locationX.ToString() },
                { "Y", _locationY.ToString() },
                { "ElapsedTime", t.TotalMilliseconds.ToString() },
                { "ID", locID.ToString() },
                { "TriggerState", _triggerState.ToString() },
                { "TargetState", Target.State.ToString() }
            });

            locID++;
        }

        /// <summary>
        /// Sets the location of the controller. Before setting the location, 
        /// the x and y values passed in will be scaled to the display screen 
        /// if the game has been calibrated.
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        public void UpdateFromMove(int X, int Y, int Z, GameState gs)
        {
            //Correct the X and Y 
            if (Settings.Default.IS_CALIBRATED)
            {
                X = _correctX(X);
                Y = _correctY(Y);
            }

            //Update the location
            x = X;
            y = Y;
            z = Z;

            if (Target.IsCollecting)
                _trackingApple = false;

            //The game is active and this is an active controller...
            if (gs == GameState.Active && Target.State != TargetState.Inactive)
            {
                //Check for collisions - this will update the Target.State property...
                if (Target.State != TargetState.Collecting && Target.State != TargetState.Collected)
                {
                    Target.IsFound = _checkForCollisions();

                    if (Target.State == TargetState.Active || Target.State == TargetState.Found)
                    {
                        //Update the rumble...
                        _updateRumbleInfo();
                    }

                    if (Target.State == TargetState.Found)
                    {
                        if (_triggerVal > 200)
                        {
                            //The target is being collected
                            Target.IsCollecting = true;
                        }
                    }
                    else
                    {
                        if (_triggerVal > 200)
                        {
                            //TODO: Play a target missed sound
                            Target.Missed();
                        }
                    }
                }
            }
        }

        public void UpdateFromKeyboard(GameState gs)
        {
            //The game is active and this is an active controller...
            if (gs == GameState.Active && Target.State != TargetState.Inactive)
            {
                //Check for collisions - this will update the Target.State property...
                if (Target.State != TargetState.Collecting && Target.State != TargetState.Collected)
                {
                    Target.IsFound = _checkForCollisions();

                    if (Target.State == TargetState.Found)
                    {
                        if (_triggerVal == 255)
                        {
                            //The target is being collected
                            Target.IsCollecting = true;
                        }
                    }
                }
            }
        }

        private void _updateRumbleInfo()
        {
            float maxRumble = 180.0f;
            float maxRange = 140.0f;
            float totalX = (float)Settings.Default.SCREEN_WIDTH;
            float totalY = (float)Settings.Default.SCREEN_HEIGHT;

            if (_axisOfInterest == HapticFeedbackAxis.y) //Right hand controller by default when ControlType == ControlType.Alternating
            {
                //For the controller in charge of the y-axis, keep the vibration
                //  constant and only vary the frequency.
                int pixelsFromEdgeY = PixelsFromRectY(Target.Area, _locationY);

                if (!IsOutOfBounds)
                {
                    //Frequency - y axis
                    if (pixelsFromEdgeY < 0)
                    {
                        _nextRumbleIntensity = 255;
                    }
                    else
                    {
                        float percent = pixelsFromEdgeY / totalY;
                        float scale = maxRange * percent;
                        _nextRumbleIntensity = (uint)Math.Abs(maxRumble - scale);
                    }
                }
                else
                {
                    _nextRumbleIntensity = 0;
                }
            }
            else if (_axisOfInterest == HapticFeedbackAxis.x) //Left hand controller by default when ControlType == ControlType.Alternating
            {
                //For the controller in charge of the x-axis, vary both the pulse
                //  delay and the frequency so that it is very obvious when they 
                //  find the target band
                int pixelsFromEdgeX = PixelsFromRectX(Target.Area, _locationX);

                if (!IsOutOfBounds)
                {
                    if (pixelsFromEdgeX < 0)
                    {
                        _nextRumbleIntensity = 255;
                    }
                    else
                    {
                        float percent = pixelsFromEdgeX / totalX;
                        float scale = maxRange * percent;
                        _nextRumbleIntensity = (uint)Math.Abs(maxRumble - scale);
                    }
                }
                else
                {
                    _nextRumbleIntensity = 0;
                }
            }
            else //Both hands -> ControlType == ControlType.Together
            {
                //When only one controller is being used at any given time, both
                //  the pulse delay and frequency have to be varied for that controller
                //  at the same time.
                int pixelsFromEdgeX = PixelsFromRectX(Target.Area, _locationX);
                int pixelsFromEdgeY = PixelsFromRectY(Target.Area, _locationY);

                //Frequency - y axis
                if (!IsOutOfBounds)
                {
                    if (pixelsFromEdgeY < 0)
                    {
                        _rumbleDuration = new TimeSpan(0, 0, 0, 0, Settings.Default.UPDATE_INTERVAL);
                        _nextRumbleIntensity = 255;
                    }
                    else
                    {
                        _rumbleDuration = new TimeSpan(0, 0, 0, 0, Settings.Default.BUZZ_DURATION);

                        float percent = pixelsFromEdgeY / totalY;
                        float scale = maxRange * percent;
                        _nextRumbleIntensity = (uint)Math.Abs(maxRumble - scale);
                    }
                }
                else
                {
                    _nextRumbleIntensity = 0;
                }

                if (RumbleState == RumbleStates.On)
                {
                    //Pulse Delay - x axis
                    if (pixelsFromEdgeX < 0)
                    {
                        //If pixelsFromEdge is negative then a collision has occurred in the x-axis
                        _nextWaitDuration = new TimeSpan(0, 0, 0, 0, 0);
                    }
                    else
                    {
                        //200 ms pulse delay at the edge, increasing by 3 ms per pixel
                        _nextWaitDuration = new TimeSpan(0, 0, 0, 0, 200 + 3 * pixelsFromEdgeX);
                    }
                }
            }
        }

        /// <summary>
        /// Update the rumble duration and intensity information
        /// </summary>
        public void NextRumble()
        {
            _rumbleIntensity = _nextRumbleIntensity;
        }

        /// <summary>
        /// Update the next wait duration
        /// </summary>
        public void NextWait()
        {
            _waitDuration = _nextWaitDuration;
        }

        /// <summary>
        /// Check for target collision. Treat the Location and the Target as
        /// circles. If they are close enough to intersect then this will
        /// count as target collision.
        /// </summary>
        /// <returns>True if target and location have collided</returns>
        private bool _checkForCollisions()
        {
            //The user has "found" the target when the position of their
            //  controller is within the circle inscribed (imaginarily) within 
            //  the target sprite image's square
            Rectangle r = Target.Area;

            if (x >= r.Left && x < r.Right && y > r.Top && y < r.Bottom)
                return true;
            else
                return false;

            //int radius = Settings.Default.TARGET_SPRITE_SIDE_LENGTH;

            //if (x > Target.x - radius
            //    && x < Target.x + radius
            //    && y > Target.y - radius
            //    && y < Target.y + radius)
            //{
            //    return true;
            //}

            //return false;
        }

        #endregion

        #region STATIC HELPER FUNCTIONS

        /// <summary>
        /// Using the max and min x values from the game calibration, this
        /// scales the Move controller's current position onto the screen
        /// </summary>
        /// <param name="x">The controller's current x position</param>
        /// <returns>The corrected x position on the screen</returns>
        private static int _correctX(int x)
        {
            //Find the ratio of the controller position within the x range
            int diff = x - Settings.Default.CAL_X_MIN;
            float percent = (float)diff / (float)(Settings.Default.CAL_X_RANGE);

            //Use the ratio to scale the x value to the window width
            return (int)(percent * (float)Settings.Default.SCREEN_WIDTH);
        }

        /// <summary>
        /// Using the max and min y values from the game calibration, this
        /// scales the Move controller's current position onto the screen
        /// </summary>
        /// <param name="x">The controller's current y position</param>
        /// <returns>The corrected y position on the screen</returns>
        private static int _correctY(int y)
        {
            //Find the ratio of the controller position within the y range
            int diff = Settings.Default.CAL_Y_MAX - y;
            float percent = (float)diff / (float)(Settings.Default.CAL_Y_RANGE);

            //Use the ratio to scale the y value to the window width
            return (int)(percent * (float)Settings.Default.SCREEN_HEIGHT);
        }

        /// <summary>
        /// Distance between two points
        /// </summary>
        /// <param name="p1">Point 1</param>
        /// <param name="p2">Point 2</param>
        /// <returns>Distance</returns>
        public static double Distance(Point p1, Point p2)
        {
            return Math.Sqrt((p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y));
        }

        public static double Distance(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
        }

        public static bool WithinDist(Point p1, Point p2, Double dist)
        {
            return (dist > Distance(p1, p2));
        }

        public static bool WithinRange(Point p1, Point p2, Double max, Double min)
        {
            Double distance = Distance(p1, p2);
            return (distance <= max && distance >= min);
        }

        public static int PixelsFromRectX(Rectangle r, int xLoc)
        {
            if (xLoc > r.Left && xLoc < r.Right)
                return (-1);
            else if (xLoc <= r.Left)
            {
                return r.Left - xLoc;
            }
            else //xLoc > r.Right
            {
                return xLoc - r.Right;
            }
        }

        public static int PixelsFromRectY(Rectangle r, int yLoc)
        {
            if (yLoc > r.Top && yLoc < r.Bottom)
                return (-1);
            else if (yLoc <= r.Top)
            {
                return r.Top - yLoc;
            }
            else //yLoc > r.Bottom
            {
                return yLoc - r.Bottom;
            }
        }

        #endregion
    }
}
