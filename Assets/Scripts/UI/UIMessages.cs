// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Messaging;

namespace Assets.Scripts.UI
{
    public class UpdateUIEnabledMessage 
        : UnityMessagePayload
    {
        public readonly bool IsEnabled;

        public UpdateUIEnabledMessage(bool inIsEnabled)
        {
            IsEnabled = inIsEnabled;
        }
    }
}
