// Copyright (C) Threetee Gang All Rights Reserved 

#if UNITY_EDITOR

using Assets.Scripts.AI.Goals;
using UnityEngine;

namespace Assets.Scripts.Test.AI.Goals
{
    public class TestCompositeGoal 
        : CompositeGoal
    {
        public TestCompositeGoal(GameObject inOwner) : base(inOwner)
        {
        }

        public void TestAddSubGoal(Goal inGoal)
        {
            AddSubGoal(inGoal);
        }
    }
}

#endif // UNITY_EDITOR
