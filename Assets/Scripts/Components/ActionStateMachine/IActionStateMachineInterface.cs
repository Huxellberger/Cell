// Copyright (C) Threetee Gang All Rights Reserved

namespace Assets.Scripts.Components.ActionStateMachine
{
    public interface IActionStateMachineInterface
    {
        void RequestActionState(EActionStateMachineTrack selectedTrack, EActionStateId inId, ActionStateInfo inInfo);

        bool IsActionStateActiveOnTrack(EActionStateMachineTrack selectedTrack, EActionStateId expectedId);
    }
}
