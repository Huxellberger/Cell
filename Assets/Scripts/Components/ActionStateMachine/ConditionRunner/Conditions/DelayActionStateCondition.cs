// Copyright (C) Threetee Gang All Rights Reserved

namespace Assets.Scripts.Components.ActionStateMachine.ConditionRunner.Conditions
{
    public class DelayActionStateCondition 
        : ActionStateCondition
    {
        private float CompletionDelay { get; set; }

        public DelayActionStateCondition(float inDelay)
            : base()
        {
            CompletionDelay = inDelay;
        }

        public override void Start()
        {
        }

        public override void Update(float deltaTime)
        {
            CompletionDelay -= deltaTime;
            if (CompletionDelay <= 0)
            {
                Complete = true;
            }
        }

        public override void End()
        {
        }
    }
}
