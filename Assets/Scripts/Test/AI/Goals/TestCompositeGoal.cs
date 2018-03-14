// Copyright (C) Threetee Gang All Rights Reserved 

#if UNITY_EDITOR

using Assets.Scripts.AI.Goals;
using UnityEngine;

namespace Assets.Scripts.Test.AI.Goals
{
    public class TestCompositeGoal 
        : CompositeGoal
    {
        public bool OnInitialisedCalled { get; private set; }
        public bool OnTerminatedCalled { get; private set; }

        public TestCompositeGoal(GameObject inOwner) : base(inOwner)
        {
            OnInitialisedCalled = false;
            OnTerminatedCalled = false;
        }

        public void TestAddSubGoal(Goal inGoal)
        {
            AddSubGoal(inGoal);
        }

        protected override void OnInitialised()
        {
            OnInitialisedCalled = true;
        }

        protected override void OnTerminated()
        {
            OnTerminatedCalled = true;
        }
    }
}

#endif // UNITY_EDITOR
