// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Emote;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Emote
{
    public class MockEmoteComponent 
        : MonoBehaviour 
        , IEmoteInterface
    { 
        public EEmoteState ? SetEmoteStateResult { get; private set; }
        public EEmoteState GetEmoteStateResult = EEmoteState.None;

        public void SetEmoteState(EEmoteState inState)
        {
            SetEmoteStateResult = inState;
        }

        public EEmoteState GetEmoteState()
        {
            return GetEmoteStateResult;
        }
    }
}

#endif // UNITY_EDITOR
