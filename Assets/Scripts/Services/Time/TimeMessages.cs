// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Messaging;

namespace Assets.Scripts.Services.Time
{
    public class PauseStatusChangedMessage
        : UnityMessagePayload
    {
        public readonly EPauseStatus NewPauseStatus;

        public PauseStatusChangedMessage(EPauseStatus inNewPauseStatus)
            : base()
        {
            NewPauseStatus = inNewPauseStatus;
        }
    }
}
