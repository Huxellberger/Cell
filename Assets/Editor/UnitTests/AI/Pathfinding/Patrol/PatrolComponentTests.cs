// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Test.AI.Pathfinding;
using Assets.Scripts.Test.AI.Pathfinding.Patrol;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Editor.UnitTests.AI.Pathfinding.Patrol
{
    [TestFixture]
    public class PatrolComponentTestFixture
    {
        private TestPatrolComponent _patrol;
        private MockPathfindingComponent _pathfinding;

        [SetUp]
        public void BeforeTest()
        {
            _pathfinding = new GameObject().AddComponent<MockPathfindingComponent>();
            _patrol = _pathfinding.gameObject.AddComponent<TestPatrolComponent>();

            _patrol.TestStart();
        }
	
        [TearDown]
        public void AfterTest()
        {
            _patrol = null;

            _pathfinding = null;
        }
	
        [Test]
        public void StartPatrolling_NoPoints_ErrorsAndDoesNotStart() 
        {
            LogAssert.Expect(LogType.Error, "Tried to patrol without a valid patrol point available!");

            _patrol.StartPatrolling();

            Assert.IsNull(_pathfinding.TargetLocation);
        }

        [Test]
        public void StartPatrolling_Points_StartsPathfindingToClosestPoint()
        {
            var closestPoint = new Vector3(1.0f, 2.0f, 0.0f);
            var furthestPoint = new Vector3(20.0f, 30.0f, 0.0f);

            _patrol.PatrolPoints.Add(furthestPoint);
            _patrol.PatrolPoints.Add(closestPoint);

            _patrol.StartPatrolling();

            Assert.AreEqual(closestPoint, _pathfinding.TargetLocation);
        }

        [Test]
        public void StopPatrolling_PatrolInProgress_CancelsPathfinding()
        {
            _patrol.PatrolPoints.Add(new Vector2(1.0f, 2.0f));

            _patrol.StartPatrolling();
            _patrol.StopPatrolling();

            Assert.IsTrue(_pathfinding.CancelPathfindingCalled);
        }

        [Test]
        public void StopPatrolling_NotPatrolling_DoesNotCancelPathfinding()
        {
            _patrol.StopPatrolling();

            Assert.IsFalse(_pathfinding.CancelPathfindingCalled);
        }
    }
}
