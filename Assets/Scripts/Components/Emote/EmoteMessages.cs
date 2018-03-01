// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Messaging;

namespace Assets.Scripts.Components.Emote
{
    public class EmoteStatusChangedMessage 
        : UnityMessagePayload
    {
        public readonly EEmoteState State;

        public EmoteStatusChangedMessage(EEmoteState inState)
            : base()
        {
            State = inState;
        }
    }

    public class EmoteStatusChangedUIMessage
        : UnityMessagePayload
    {
        public readonly EEmoteState State;

        public EmoteStatusChangedUIMessage(EEmoteState inState)
            : base()
        {
            State = inState;
        }
    }
}
