using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViAppleGrab.Properties;
using ViToolkit.PSMoveSharp;
using System.Diagnostics;

namespace ViAppleGrab.Collections
{
    /// <summary>
    /// Simple array based collection of Controllers. The purpose of this 
    /// collection is to provide ControllerIndex based indexing into the 
    /// controllers. This only makes the code easier to read and is meant
    /// to be more cosmetic than functional
    /// </summary>
    public class ControllerCollection
    {
        private Controller[] _controllers;
        public int Count;
        private PSMoveClient _moveClient;

        public ControllerIndex CurrController { get; private set; }

        public ControllerCollection(int s)
        {
            if ((ControlType)Settings.Default.CONTROL_TYPE == ControlType.Together)
            {
                CurrController = ControllerIndex.BothHands;
            }
            else
            {
                CurrController = ControllerIndex.LeftHand; //Default to the right hand - spawning the first apple will make it the right hand
            }

            Count = s;
            _controllers = new Controller[Count];

            for (int i = 0; i < Count; i++)
            {
                _controllers[i] = new Controller((ControllerIndex)i);
            }
        }

        public void Init(PSMoveClient m)
        {
            _moveClient = m;
        }

        public Controller this[ControllerIndex ci]
        {
            get
            {
                return _controllers[(int)ci];
            }
            set
            {
                _controllers[(int)ci] = value;
            }
        }

        public Controller Current
        {
            get
            {
                if (CurrController != ControllerIndex.BothHands)
                    return _controllers[(int)CurrController];
                else
                    return _controllers[(int)ControllerIndex.RightHand];
            }
        }

        public TimeSpan TargetAliveTime
        {
            get
            {
                if (CurrController != ControllerIndex.BothHands)
                {
                    return _controllers[(int)CurrController].Target.AliveTime;
                }
                else
                {
                    if (_controllers[(int)ControllerIndex.RightHand].Target.State != TargetState.Active)
                        return _controllers[(int)ControllerIndex.RightHand].Target.AliveTime;
                    else
                        return _controllers[(int)ControllerIndex.LeftHand].Target.AliveTime;
                }
            }
        }

        public bool IsTargetCollecting
        {
            get
            {
                if (CurrController != ControllerIndex.BothHands)
                    return _controllers[(int)CurrController].Target.IsCollecting;
                else
                {
                    return (_controllers[(int)ControllerIndex.RightHand].Target.IsCollecting
                            || _controllers[(int)ControllerIndex.LeftHand].Target.IsCollecting);
                }
            }
        }

        public bool _isTargetFound = false;
        public bool IsTargetFound
        {
            get
            {
                if (CurrController != ControllerIndex.BothHands)
                    return _controllers[(int)CurrController].Target.IsFound;
                else
                {
                    return (_controllers[(int)ControllerIndex.RightHand].Target.IsFound
                            || _controllers[(int)ControllerIndex.LeftHand].Target.IsFound);
                }
            }
        }

        public bool WasTargetLost
        {
            get
            {
                bool temp1, temp2;
                Controller c;

                if (CurrController != ControllerIndex.BothHands)
                {
                    c = _controllers[(int)CurrController];
                    temp1 = c.Target.WasLost;
                    temp1 = temp1 && !Controller.WithinRange(c.Location, c.Target.Location, 60, 84);
                    return temp1;
                }
                else
                {
                    c = _controllers[(int)ControllerIndex.RightHand];
                    temp1 = c.Target.WasLost;
                    temp1 = temp1 && !Controller.WithinRange(c.Location, c.Target.Location, 60, 84);

                    c = _controllers[(int)ControllerIndex.LeftHand];
                    temp2 = c.Target.WasLost;
                    temp2 = temp2 && !Controller.WithinRange(c.Location, c.Target.Location, 60, 84);

                    return (_controllers[(int)ControllerIndex.RightHand].Target.WasLost
                            || _controllers[(int)ControllerIndex.LeftHand].Target.WasLost);
                }
            }
        }

        public bool WasTargetCollected
        {
            get
            {
                if (CurrController != ControllerIndex.BothHands)
                {
                    return _controllers[(int)CurrController].Target.WasCollected;
                }
                else
                {
                    if (Settings.Default.SIMULTANEOUS_TARGETS)
                    {
                        return (_controllers[(int)ControllerIndex.LeftHand].Target.WasCollected
                            && _controllers[(int)ControllerIndex.RightHand].Target.WasCollected);
                    }
                    else
                    {
                        return (_controllers[(int)ControllerIndex.LeftHand].Target.WasCollected
                            || _controllers[(int)ControllerIndex.RightHand].Target.WasCollected);
                    }
                }
            }
        }

        public bool WasTargetMissed
        {
            get
            {
                if (CurrController != ControllerIndex.BothHands)
                {
                    return _controllers[(int)CurrController].Target.State == TargetState.Missed;
                }
                else
                {
                    return (_controllers[(int)ControllerIndex.LeftHand].Target.State == TargetState.Missed
                        || _controllers[(int)ControllerIndex.RightHand].Target.State == TargetState.Missed);
                }
            }
        }

        public void SwitchControlType()
        {
            //Switch the current controller
            if (CurrController == ControllerIndex.BothHands)
            {
                CurrController = ControllerIndex.LeftHand;
            }
            else
                CurrController = ControllerIndex.BothHands;

            //Reinitialize the controllers
            for (int i = 0; i < Count; i++)
            {
                _controllers[i].AssignPrimaryAxis((ControllerIndex)i);
                _controllers[i].AssignSpatialLimits((ControllerIndex)i);
            }

            //Create new targets which correspond to the type of game
            Target.GenerateNewTargets();
        }

        public void ReinitializeControllers()
        {
            //Set the current controller
            if ((ControlType)Settings.Default.CONTROL_TYPE == ControlType.Alternating)
            {
                CurrController = ControllerIndex.LeftHand;
            }
            else
                CurrController = ControllerIndex.BothHands;

            //Reinitialize the controllers
            for (int i = 0; i < Count; i++)
            {
                _controllers[i].AssignPrimaryAxis((ControllerIndex)i);
                _controllers[i].AssignSpatialLimits((ControllerIndex)i);
            }

            //Create new targets which correspond to the type of game
            Target.LoadTargets();
        }

        public void RecordPositions(TimeSpan t)
        {
            if (CurrController == ControllerIndex.BothHands)
            {
                _controllers[(int)ControllerIndex.RightHand].RecordPosition(t);
                _controllers[(int)ControllerIndex.LeftHand].RecordPosition(t);
            }
            else
            {
                _controllers[(int)CurrController].RecordPosition(t);
            }
        }

        public void SpawnApple()
        {
            Debug.WriteLine("");

            switch (CurrController)
            {
                case ControllerIndex.RightHand:
                    StopRumble(ControllerIndex.RightHand, RumbleStates.Off);

                    this[ControllerIndex.RightHand].Target.Deactivate();

                    //If we are using both controllers
                    if (!Settings.Default.SINGLE_TARGET)
                    {
                        CurrController = ControllerIndex.LeftHand;

                        this[ControllerIndex.LeftHand].NewTarget(this[ControllerIndex.RightHand].Location);
                    }
                    else
                    {
                        //Otherwise
                        this[ControllerIndex.RightHand].NewTarget(this[ControllerIndex.LeftHand].Location);
                    }

                    //_updateRumble(ControllerIndex.LeftHand);

                    Debug.WriteLine("[New Target] - " 
                        + this[ControllerIndex.LeftHand].Target.ID.ToString() 
                        + " - Left Hand - " + DateTime.Now);
                    break;

                case ControllerIndex.LeftHand:
                    StopRumble(ControllerIndex.LeftHand, RumbleStates.Off);

                    this[ControllerIndex.LeftHand].Target.Deactivate();

                    CurrController = ControllerIndex.RightHand;

                    this[ControllerIndex.RightHand].NewTarget(this[ControllerIndex.LeftHand].Location);

                    //_updateRumble(ControllerIndex.RightHand);
                    Debug.WriteLine("[New Target] - " 
                        + this[ControllerIndex.RightHand].Target.ID.ToString() 
                        + " - Right Hand - " + DateTime.Now);
                    break;

                case ControllerIndex.BothHands:
                    int id = this[ControllerIndex.RightHand].NewTarget(this[ControllerIndex.LeftHand].Location);

                    Debug.WriteLine("");
                    Debug.WriteLine("[New Target] - " 
                        + this[ControllerIndex.RightHand].Target.ID.ToString() 
                        + " - Right Hand - " + DateTime.Now);

                    if (!Settings.Default.SIMULTANEOUS_TARGETS)
                    {
                        this[ControllerIndex.LeftHand].NewTarget(id);

                        //UpdateRumbles();

                        Debug.WriteLine("[Duplicate Target] - "
                            + this[ControllerIndex.LeftHand].Target.ID.ToString()
                            + " - Left Hand - " + DateTime.Now);
                    }
                    else
                    {
                        this[ControllerIndex.LeftHand].NewTarget(this[ControllerIndex.RightHand].Location);

                        Debug.WriteLine("");
                        Debug.WriteLine("[New Target] - "
                            + this[ControllerIndex.LeftHand].Target.ID.ToString()
                            + " - Left Hand - " + DateTime.Now);
                    }

                    break;
            }
        }

        public void TimeOutTargets()
        {
            if (CurrController == ControllerIndex.BothHands)
            {
                _controllers[(int)ControllerIndex.RightHand].Target.TimedOut();
            }
            else
            {
                _controllers[(int)CurrController].Target.TimedOut();
            }
        }

        public void DeactivateTargets()
        {
            if (_moveClient != null)
            {
                for (int i = 0; i < Settings.Default.MAX_CONTROLLERS; i++)
                {
                    _controllers[i].Target.Deactivate();
                }
            }
        }

        public void StartRumble(ControllerIndex ci, uint intensity)
        {
            if (_moveClient != null)
            {
                _moveClient.SendRequestPacket(PSMoveClient.ClientRequest.PSMoveClientRequestSetRumble, (uint)ci, intensity);
                _controllers[(int)ci].RumbleState = RumbleStates.On;
                _controllers[(int)ci].RumbleStageStart = DateTime.Now;
            }
        }

        public void StopRumble(ControllerIndex ci, RumbleStates rs)
        {
            if (_moveClient != null)
            {
                _controllers[(int)ci].RumbleState = rs;
                _moveClient.SendRequestPacket(PSMoveClient.ClientRequest.PSMoveClientRequestSetRumble, (uint)ci, 0);

                if(rs == RumbleStates.Waiting)
                    _controllers[(int)ci].RumbleStageStart = DateTime.Now;
            }
        }

        public void StopRumbles()
        {
            if (_moveClient != null)
            {
                for (int i = 0; i < Count; i++)
                {
                    StopRumble((ControllerIndex)i, RumbleStates.Off);
                }
            }
        }

        public void PauseRumbles()
        {
            if (_moveClient != null)
            {
                if (CurrController != ControllerIndex.BothHands)
                {
                    _controllers[(int)CurrController].Target.Pause();
                    _moveClient.SendRequestPacket(PSMoveClient.ClientRequest.PSMoveClientRequestSetRumble, (uint)CurrController, 0);

                }
                else
                {
                    for (int i = 0; i < Count; i++)
                    {
                        _controllers[i].Target.Pause();
                        _moveClient.SendRequestPacket(PSMoveClient.ClientRequest.PSMoveClientRequestSetRumble, (uint)i, 0);
                    }
                }
            }
        }

        public void ResumeRumbles()
        {
            if (_moveClient != null)
            {
                if (CurrController != ControllerIndex.BothHands)
                {
                    _controllers[(int)CurrController].Target.Resume();
                }
                else
                {
                    for (int i = 0; i < Count; i++)
                    {
                        _controllers[i].Target.Resume();
                    }
                }
            }
        }

        public void UpdateRumbles()
        {
            if (_moveClient != null)
            {
                if (CurrController != ControllerIndex.BothHands)
                {
                    _updateRumble(CurrController);
                }
                else
                {
                    for (int i = 0; i < Count; i++)
                    {
                        _updateRumble((ControllerIndex)i);
                    }
                }
            }
        }

        private void _updateRumble(ControllerIndex ci)
        {
            uint intensity; 
            TimeSpan elapsed = DateTime.Now - _controllers[(int)ci].RumbleStageStart;
            TimeSpan rumbleDuration = _controllers[(int)ci].RumbleDuration;
            TimeSpan waitDuration = _controllers[(int)ci].WaitDuration;
            TargetState targetState = _controllers[(int)ci].Target.State;

            switch (_controllers[(int)ci].RumbleState)
            {
                case RumbleStates.Off:
                    break;

                case RumbleStates.TurningOff:
                    StopRumble(ci, RumbleStates.Off);
                    break;

                case RumbleStates.TurningOn:
                    //Update the intensity so that we know if it can be turned on...
                    _controllers[(int)ci].NextRumble();
                    intensity = _controllers[(int)ci].RumbleIntensity;
                    if (_controllers[(int)ci].RumbleIntensity > 0)
                        StartRumble(ci, intensity);
                    break;

                case RumbleStates.On:
                    switch (targetState)
                    {
                        case TargetState.Inactive:
                            _controllers[(int)ci].RumbleState = RumbleStates.TurningOff;
                            break;

                        case TargetState.Active:
                            if (elapsed >= rumbleDuration)
                            {
                                if (_controllers[(int)ci].Axis != HapticFeedbackAxis.xy) //The controllers working together should not stop
                                {
                                    _controllers[(int)ci].NextRumble();
                                    intensity = _controllers[(int)ci].RumbleIntensity;
                                    StartRumble(ci, intensity);
                                }
                                else //the individual controller should use pulse delay and frequency
                                {
                                    _controllers[(int)ci].NextWait();

                                    if(_controllers[(int)ci].WaitDuration.TotalMilliseconds == 0)
                                    {
                                        _controllers[(int)ci].NextRumble();
                                        intensity = _controllers[(int)ci].RumbleIntensity;
                                        StartRumble(ci, intensity);
                                    }
                                    else
                                    {
                                        StopRumble(ci, RumbleStates.Waiting);
                                    }
                                }
                            }
                            break;

                        case TargetState.Paused:
                            _controllers[(int)ci].RumbleState = RumbleStates.TurningOff;
                            break;

                        case TargetState.Found:
                            if (elapsed >= rumbleDuration)
                            {
                                _controllers[(int)ci].NextRumble();
                                intensity = _controllers[(int)ci].RumbleIntensity;
                                StartRumble(ci, intensity);
                            }
                            break;

                        case TargetState.Collected:
                            _controllers[(int)ci].RumbleState = RumbleStates.TurningOff;
                            break;
                    }
                    break;

                case RumbleStates.Waiting:
                    _controllers[(int)ci].NextRumble();
                    intensity = _controllers[(int)ci].RumbleIntensity;
                    if (elapsed >= waitDuration && intensity > 0)
                    {
                        StartRumble(ci, intensity);
                    }
                    break;
            }
        }
    }
}
