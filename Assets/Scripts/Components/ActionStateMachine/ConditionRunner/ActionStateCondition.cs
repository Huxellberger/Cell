// Copyright (C) Threetee Gang All Rights Reserved

namespace Assets.Scripts.Components.ActionStateMachine.ConditionRunner
{
    public abstract class ActionStateCondition
    {
        protected ActionStateCondition()
        {
            Complete = false;
        }

        public abstract void Start();
        public abstract void Update(float deltaTime);
        public abstract void End();

        public bool Complete { get; protected set; }
    }
}
