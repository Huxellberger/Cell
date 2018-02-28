// Copyright (C) Threetee Gang All Rights Reserved

namespace Assets.Scripts.Services.Time
{
    public interface IPauseListenerInterface
    {
        void UpdatePauseStatus(EPauseStatus inStatus);
    }
}
