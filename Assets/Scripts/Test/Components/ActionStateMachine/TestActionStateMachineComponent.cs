// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.ActionStateMachine;

namespace Assets.Scripts.Test.Components.ActionStateMachine
{
    public class TestActionStateMachineComponent 
        : ActionStateMachineComponent
    {
        public void TestAwake()
        {
            Awake();
        }

        public void SetActionStateCreator(IActionStateCreatorInterface inCreatorInterface)
        {
            Creator = inCreatorInterface;
        }

        public void TestUpdate()
        {
            Update();
        }

        public void TestDestroy()
        {
            OnDestroy();
        }

        public ActionState GetActionStateOnTrack(EActionStateMachineTrack inTrack)
        {
            return ActiveActionStates[inTrack];
        }
    }
}

#endif
