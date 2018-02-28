// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Messaging;

namespace Assets.Scripts.UI.HUD
{
    public class TextNotificationSentUIMessage 
        : UnityMessagePayload
    {
        public readonly string Message;

        public TextNotificationSentUIMessage(string inMessage)
            : base()
        {
            Message = inMessage;
        }
    }

    public class TextNotificationClearedUIMessage
        : UnityMessagePayload
    {
    }
}
