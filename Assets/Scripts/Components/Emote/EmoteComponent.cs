// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Messaging;
using UnityEngine;

namespace Assets.Scripts.Components.Emote
{
    public class EmoteComponent 
        : MonoBehaviour 
        , IEmoteInterface
    {
        private EEmoteState _emoteState = EEmoteState.None;

        // IEmoteInterface
        public void SetEmoteState(EEmoteState inState)
        {
            if (_emoteState != inState)
            {
                _emoteState = inState;
                UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(gameObject, new EmoteStatusChangedMessage(_emoteState));
            }
        }

        public EEmoteState GetEmoteState()
        {
            return _emoteState;
        }
        // ~IEmoteInterface
    }
}
