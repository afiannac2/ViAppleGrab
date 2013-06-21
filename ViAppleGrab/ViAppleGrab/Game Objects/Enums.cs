using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ViAppleGrab
{
    public enum StudyStages
    {
        None = 0,
        Warmup = 1,
        Single = 2,
        Warmup2 = 3,
        Simultaneous = 4
    }

    public class Point3
    {
        public int x;
        public int y;
        public int z;
    }

    public enum ControllerIndex
    {
        RightHand = 0, //By default we will call the right hand the first controller
        LeftHand = 1,
        BothHands = 3
    }

    public enum ControlType
    {
        Alternating = 0,
        Together = 1
    }

    public enum GameType
    {
        ApplesOnly = 0,
        ApplesAndRottenApples = 1
    }

    public enum GameState
    {
        Loading = 0,
        Calibration = 1,
        Instructions = 2,
        Active = 3,
        Paused = 4,
        GameOver = 5,
        ShutDown = 6,
        Error = 7
    }

    public enum LoadingState
    {
        Initializing = 0,
        Welcome = 1,
        Calibration = 2,
        Instructions = 3,
        Completing = 4,
        GatheringUserData = 5
    }

    public enum ComponentIndex
    {
        Input = 0,
        Logic = 1,
        Sound = 2
    }

    public enum InputButtonState
    {
        NotPressed = 0,
        JustPressed = 1,
        StillPressed = 2
    }

    public enum TargetState
    {
        Inactive = 0,
        Active = 1,
        Paused = 2,
        Found = 3,
        Collecting = 4,
        Collected = 5,
        Missed = 6
    }

    public enum RumbleStates
    {
        Off = 0,
        TurningOff = 1,
        TurningOn = 2,
        Waiting = 3,
        On = 4,
    }

    public enum HapticFeedbackAxis
    {
        x = 0,
        y = 1,
        xy = 2
    }
}
