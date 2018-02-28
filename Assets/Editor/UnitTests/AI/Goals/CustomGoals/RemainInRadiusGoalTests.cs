// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.AI.Goals;
using Assets.Scripts.AI.Goals.CustomGoals;
using Assets.Scripts.Test.AI.Pathfinding;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.AI.Goals.CustomGoals
{
    [TestFixture]
    public class RemainInRadiusGoalTestFixture
    {
        private RemainInRadiusGoal _goal;
        private MockPathfindingComponent _pathfinding;
        private GameObject _owner;
        private RemainInRadiusGoalParams _params;
        private Vector3 _startPosition;

        [SetUp]
        public void BeforeTest()
        {
            _startPosition = new Vector3(12.0f, 100.0f, 120.0f);

            _owner = new GameObject();
            _owner.transform.position = _startPosition;
            _pathfinding = _owner.AddComponent<MockPathfindingComponent>();

            _params = new RemainInRadiusGoalParams{LeftRadiusDesirability = 0.5f, MaxRadius = 10.0f};

            _goal = new RemainInRadiusGoal(_owner, _params);
        }

        [TearDown]
        public void AfterTest()
        {
            _goal = null;

            _pathfinding = null;
            _owner = null;
        }

        [Test]
        public void Update_NoTarget_Inactive()
        {
            Assert.AreEqual(EGoalStatus.Inactive, _goal.Update(12.0f));
        }

        [Test]
        public void CalculateDesirability_WithinRadius_Zero()
        {
            Assert.AreEqual(0.0f, _goal.CalculateDesirability());
        }

        [Test]
        public void CalculateDesirability_OutsideRadius_LeftRadiusDesirability()
        {
            _owner.transform.position += new Vector3(_params.MaxRadius, _params.MaxRadius, _params.MaxRadius);

            Assert.AreEqual(_params.LeftRadiusDesirability, _goal.CalculateDesirability());
        }

        [Test]
        public void CalculateDesirability_WithinRadiusButPathfinding_LeftRadiusDesirability()
        {
            _goal.Initialise();

            Assert.AreEqual(_params.LeftRadiusDesirability, _goal.CalculateDesirability());
        }

        [Test]
        public void Initialise_StartsPathfindingForInitialLocation()
        {
            _owner.transform.position = new Vector3(12.0f, 1.0f, 2.0f);
            _goal.Initialise();

            Assert.AreEqual(_startPosition, _pathfinding.TargetLocation);
        }

        [Test]
        public void Update_PathfindingInProgress_ReturnsInProgress()
        {
            _owner.transform.position = new Vector3(12.0f, 1.0f, 2.0f);
            _goal.Initialise();

            Assert.AreEqual(EGoalStatus.InProgress, _goal.Update(1.0f));
        }

        [Test]
        public void Update_PathfindingCompletes_GoalCompletes()
        {
            _owner.transform.position = new Vector3(12.0f, 1.0f, 2.0f);
            _goal.Initialise();

            _pathfinding.CompleteDelegate();

            Assert.AreEqual(EGoalStatus.Completed, _goal.Update(1.0f));
        }

        [Test]
        public void Terminate_GoalSetToInactive()
        {
            _owner.transform.position = new Vector3(12.0f, 1.0f, 2.0f);
            _goal.Initialise();

            _pathfinding.CompleteDelegate();

            _goal.Terminate();

            Assert.AreEqual(EGoalStatus.Inactive, _goal.Update(1.0f));
        }
    }
}
