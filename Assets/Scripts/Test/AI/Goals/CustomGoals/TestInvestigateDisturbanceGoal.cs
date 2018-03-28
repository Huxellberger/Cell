// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.AI.Goals.CustomGoals;
using UnityEngine;

namespace Assets.Scripts.Test.AI.Goals.CustomGoals
{
    public class TestInvestigateDisturbanceGoal 
        : InvestigateDisturbanceGoal
    {
        public bool RegisterForDisturbanceCalled = false;
        public bool UnregisterForDisturbanceCalled = false;

        public TestInvestigateDisturbanceGoal(GameObject inOwner, InvestigateDisturbanceGoalParams inParams)
            : base(inOwner, inParams)
        {
        }

        protected override void RegisterForDisturbance()
        {
            RegisterForDisturbanceCalled = true;
        }

        protected override void UnregisterForDisturbance()
        {
            UnregisterForDisturbanceCalled = true;
        }

        public void TestRecordDisturbance(Vector3 setPosition)
        {
            RecordDisturbance(setPosition);
        }
    }
}

#endif // UNITY_EDITOR
