// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine.Builder
{
    public class MockActionStateCreator 
        : IActionStateCreatorInterface
    {
        public ActionState CreateActionState(EActionStateId inId, ActionStateInfo inInfo)
        {
            return new TestActionState(inId, inInfo);
        }
    }
}
