// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.AI.Goals;

namespace Assets.Scripts.Test.AI.Goals
{
    public class TestGoalPlannerComponent 
        : GoalPlannerComponent
    {
        private float GetTimeDeltaResult { get; set; }

        public IGoalBuilderInterface TestGoalBuilderInterface;

        public void TestStart ()
        {
            Start();
        }
	
        public void TestUpdate (float inDelta)
        {
            GetTimeDeltaResult = inDelta;

            Update();
        }

        protected override float GetDeltaTime()
        {
            return GetTimeDeltaResult;
        }

        protected override IGoalBuilderInterface GetGoalBuilderInterface()
        {
            return TestGoalBuilderInterface;
        }
    }
}

#endif // UNITY_EDITOR
