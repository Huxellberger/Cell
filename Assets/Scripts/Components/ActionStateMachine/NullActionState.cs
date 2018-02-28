// Copyright (C) Threetee Gang All Rights Reserved

namespace Assets.Scripts.Components.ActionStateMachine
{
    public class NullActionState : ActionState
    {
        public NullActionState()
            : base(EActionStateId.Null, new ActionStateInfo())
        {
        }

        protected override void OnStart() { }
        protected override void OnUpdate(float deltaTime) { }
        protected override void OnEnd() { }
    }
}
