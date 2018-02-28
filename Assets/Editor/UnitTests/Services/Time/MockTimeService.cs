// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Services.Time;

#if UNITY_EDITOR

namespace Assets.Editor.UnitTests.Services.Time
{
    public class MockTimeService
        : ITimeServiceInterface
    {
        public IPauseListenerInterface AddPauseListenerResult { get; set; }
        public IPauseListenerInterface RemovePauseListenerResult { get; set; }
        public EPauseStatus ? SetPauseStatusResult { get; private set; }
        public EPauseStatus GetPauseStatusResult = EPauseStatus.Unpaused;

        // ITimeServiceInterface
        public void AddPauseListener(IPauseListenerInterface inListener)
        {
            AddPauseListenerResult = inListener;
        }

        public void RemovePauseListener(IPauseListenerInterface inListener)
        {
            RemovePauseListenerResult = inListener;
        }

        public void SetPauseStatus(EPauseStatus inPauseStatus)
        {
            SetPauseStatusResult = inPauseStatus;
        }

        public EPauseStatus GetPauseStatus()
        {
            return GetPauseStatusResult;
        }
        // ~ITimeServiceInterface
    }
}

#endif // UNITY_EDITOR
