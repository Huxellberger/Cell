// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.AI.Goals;
using Assets.Scripts.AI.Goals.CustomGoals;
using Assets.Scripts.Test.AI.Pathfinding;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.AI.Goals.CustomGoals
{
    [TestFixture]
    public class MoveToTargetGoalTestFixture
    {
        private MockPathfindingComponent _pathfinding;

        [SetUp]
        public void BeforeTest()
        {
            _pathfinding = new GameObject().AddComponent<MockPathfindingComponent>();
        }
	
        [TearDown]
        public void AfterTest()
        {
            _pathfinding = null;
        }
	
        [Test]
        public void Update_Creation_StatusIsInactive()
        {
            var goal = new MoveToTargetGoal(_pathfinding.gameObject, Vector3.down);

            Assert.AreEqual(EGoalStatus.Inactive, goal.Update(1.0f));
        }

        [Test]
        public void Update_Initialised_StatusIsInProgress()
        {
            var goal = new MoveToTargetGoal(_pathfinding.gameObject, Vector3.down);

            goal.Initialise();

            Assert.AreEqual(EGoalStatus.InProgress, goal.Update(1.0f));
        }

        [Test]
        public void Update_Initialised_SetsExpectedVectorAsTargetLocation()
        {
            var expectedVector = new Vector3(1.0f, 2.0f, 4.0f);
            var goal = new MoveToTargetGoal(_pathfinding.gameObject, expectedVector);

            goal.Initialise();

            Assert.AreEqual(expectedVector, _pathfinding.TargetLocation);
        }

        [Test]
        public void Update_NoPathfinding_StatusIsFailed()
        {
            var goal = new MoveToTargetGoal(new GameObject(), Vector3.down);

            goal.Initialise();

            Assert.AreEqual(EGoalStatus.Failed, goal.Update(1.0f));
        }

        [Test]
        public void Update_CompletesPathfinding_StatusIsCompleted()
        {
            var goal = new MoveToTargetGoal(_pathfinding.gameObject, Vector3.down);

            goal.Initialise();

            _pathfinding.CompleteDelegate();

            Assert.AreEqual(EGoalStatus.Completed, goal.Update(1.0f));
        }

        [Test]
        public void Update_Terminated_StatusIsInactive()
        {
            var goal = new MoveToTargetGoal(_pathfinding.gameObject, Vector3.down);

            goal.Initialise();

            goal.Terminate();

            Assert.AreEqual(EGoalStatus.Inactive, goal.Update(1.0f));
        }
    }
}
