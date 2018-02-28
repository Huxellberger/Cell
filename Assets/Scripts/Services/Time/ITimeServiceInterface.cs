// Copyright (C) Threetee Gang All Rights Reserved

namespace Assets.Scripts.Services.Time
{
    public interface ITimeServiceInterface
    {
        void AddPauseListener(IPauseListenerInterface inListener);
        void RemovePauseListener(IPauseListenerInterface inListener);
        void SetPauseStatus(EPauseStatus inPauseStatus);
        EPauseStatus GetPauseStatus();
    }
}
