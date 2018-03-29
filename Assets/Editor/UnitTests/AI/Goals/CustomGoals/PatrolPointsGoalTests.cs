// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.AI.Goals;
using Assets.Scripts.AI.Goals.CustomGoals;
using Assets.Scripts.Test.AI.Pathfinding.Patrol;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.AI.Goals.CustomGoals
{
    [TestFixture]
    public class PatrolPointsGoalTestFixture
    {
        private MockPatrolComponent _patrol;
        private PatrolPointsGoalParams _params;

        [SetUp]
        public void BeforeTest()
        {
            _patrol = new GameObject().AddComponent<MockPatrolComponent>();

            _params = new PatrolPointsGoalParams{PatrolDesirability = 0.5f};
        }
	
        [TearDown]
        public void AfterTest()
        {
            _patrol = null;
        }
	
        [Test]
        public void CalculateDesirability_ReturnsParamDesirability() 
        {
            Assert.AreEqual(_params.PatrolDesirability, new PatrolPointsGoal(_patrol.gameObject, _params).CalculateDesirability());
        }

        [Test]
        public void Initialised_StartsPatrolling()
        {
            var goal = new PatrolPointsGoal(_patrol.gameObject, _params);

            goal.Initialise();

            Assert.IsTrue(_patrol.StartPatrollingCalled);
        }

        [Test]
        public void Update_PatrolInterface_InProgress()
        {
            var goal = new PatrolPointsGoal(_patrol.gameObject, _params);

            goal.Initialise();

            Assert.AreEqual(EGoalStatus.InProgress, goal.Update(1.0f));
        }

        [Test]
        public void Update_NoPatrolInterface_Failed()
        {
            var goal = new PatrolPointsGoal(new GameObject(), _params);

            goal.Initialise();

            Assert.AreEqual(EGoalStatus.Failed, goal.Update(1.0f));
        }

        [Test]
        public void Terminated_StopsPatrolling()
        {
            var goal = new PatrolPointsGoal(_patrol.gameObject, _params);

            goal.Initialise();
            goal.Terminate();

            Assert.IsTrue(_patrol.StopPatrollingCalled);
        }
    }
}
