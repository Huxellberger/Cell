// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.ActionStateMachine;
using UnityEngine;

namespace Assets.Scripts.Test.Components.ActionStateMachine
{
    public class MockActionStateMachineComponent 
        : MonoBehaviour
        , IActionStateMachineInterface
    {
        public EActionStateMachineTrack ? RequestedTrack { get; private set; }
        public EActionStateId ? RequestedId { get; private set; }
        public ActionStateInfo RequestedInfo = null;

        public EActionStateMachineTrack ? IsActionStateActiveTrackQuery { get; private set; }
        public EActionStateId ? IsActionStateActiveIdQuery { get; private set; }
        public bool IsActionStateActiveResult = false;

        public void RequestActionState(EActionStateMachineTrack selectedTrack, EActionStateId inId, ActionStateInfo inInfo)
        {
            RequestedTrack = selectedTrack;
            RequestedId = inId;
            RequestedInfo = inInfo;
        }

        public bool IsActionStateActiveOnTrack(EActionStateMachineTrack selectedTrack, EActionStateId expectedId)
        {
            IsActionStateActiveTrackQuery = selectedTrack;
            IsActionStateActiveIdQuery = expectedId;
            return IsActionStateActiveResult;
        }
    }
}

#endif
