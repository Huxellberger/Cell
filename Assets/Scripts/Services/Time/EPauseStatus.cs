// Copyright (C) Threetee Gang All Rights Reserved

namespace Assets.Scripts.Services.Time
{
    public enum EPauseStatus
    {
        Paused,
        Unpaused
    }

    public static class PauseStatusFunctions
    {
        public static EPauseStatus Invert(EPauseStatus inPauseStatus)
        {
            switch (inPauseStatus)
            {
                case EPauseStatus.Paused:
                    return EPauseStatus.Unpaused;
                case EPauseStatus.Unpaused:
                    return EPauseStatus.Paused;
                default:
                    return inPauseStatus;
            }
        }
    }
}
