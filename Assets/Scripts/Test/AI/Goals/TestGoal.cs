// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.AI.Goals;
using UnityEngine;

namespace Assets.Scripts.Test.AI.Goals
{
    public class TestGoal : Goal
    {
        public bool Initialised { get; private set; }

        public bool Updated { get; private set; }
        public float ? UpdateValue { get; private set; }
        public EGoalStatus UpdateResult { get; set; }

        public bool Terminated { get; private set; }

        public bool OverrideDesirabilityFunction { get; set; }
        public float CalculateDesirabilityOverride { get; set; }

        public TestGoal(GameObject inOwner) 
            : base(inOwner)
        {
            Initialised = false;
            Updated = false;
            UpdateResult = EGoalStatus.InProgress;
            Terminated = false;

            OverrideDesirabilityFunction = false;
            CalculateDesirabilityOverride = 0.0f;
        }

        public override void Initialise()
        {
            Initialised = true;
        }

        public override EGoalStatus Update(float inDeltaTime)
        {
            Updated = true;
            UpdateValue = inDeltaTime;
            return UpdateResult;
        }

        public override void Terminate()
        {
            Terminated = true;
        }

        public GameObject GetOwner()
        {
            return Owner;
        }

        public override float CalculateDesirability()
        {
            if (OverrideDesirabilityFunction)
            {
                return CalculateDesirabilityOverride;
            }

            return base.CalculateDesirability();
        }
    }
}

#endif // UNITY_EDITOR
