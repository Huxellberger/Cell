// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.AI.Goals;
using UnityEngine;

namespace Assets.Scripts.Test.AI.Goals
{
    public class MockGoalBuilder 
        : IGoalBuilderInterface
    {
        public GameObject GoalOwner { get; set; }

        public IList<TestGoal> CreatedGoals { get; protected set; }

        public IDictionary<EGoalID, int> GoalIdCount { get; set; }

        public MockGoalBuilder(GameObject inOwner)
        {
            CreatedGoals = new List<TestGoal>();
            GoalIdCount = new Dictionary<EGoalID, int>();
            GoalOwner = inOwner;
        }

        public Goal CreateGoalForId(EGoalID inId)
        {
            if (!GoalIdCount.ContainsKey(inId))
            {
                GoalIdCount.Add(inId, 0);
            }

            GoalIdCount[inId]++;

            CreatedGoals.Add(new TestGoal(GoalOwner));
            return CreatedGoals.Last();
        }
    }
}

#endif // UNITY_EDITOR
