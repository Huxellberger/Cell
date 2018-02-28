// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine;

public interface IActionStateCreatorInterface
{
    ActionState CreateActionState(EActionStateId inId, ActionStateInfo inInfo);
}
