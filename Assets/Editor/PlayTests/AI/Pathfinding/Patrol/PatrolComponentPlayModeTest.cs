// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections;
using Assets.Scripts.Test.AI.Pathfinding;
using Assets.Scripts.Test.AI.Pathfinding.Patrol;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Editor.PlayTests.AI.Pathfinding.Patrol
{
    [TestFixture]
    public class PatrolComponentPlayModeTestFixture
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

        [UnityTest]
        public IEnumerator Patrolling_PatrolCompletes_AfterIdleTimeAssignsNextPoint()
        {
            var closestPoint = new Vector3(1.0f, 2.0f, 0.0f);
            var furthestPoint = new Vector3(20.0f, 30.0f, 0.0f);

            _patrol.PatrolPoints.Add(furthestPoint);
            _patrol.PatrolPoints.Add(closestPoint);

            _patrol.StartPatrolling();

            _pathfinding.CompleteDelegate();

            yield return new WaitForSeconds(_patrol.IdleTimeBetweenPoints + 0.1f);

            Assert.AreEqual(_pathfinding.TargetLocation, furthestPoint);
        }

        [UnityTest]
        public IEnumerator Patrolling_PatrolCompletes_ListLoops()
        {
            var closestPoint = new Vector3(1.0f, 2.0f, 0.0f);
            var furthestPoint = new Vector3(20.0f, 30.0f, 0.0f);

            _patrol.PatrolPoints.Add(furthestPoint);
            _patrol.PatrolPoints.Add(closestPoint);

            _patrol.StartPatrolling();

            _pathfinding.CompleteDelegate();

            yield return new WaitForSeconds(_patrol.IdleTimeBetweenPoints + 0.1f);

            _pathfinding.CompleteDelegate();

            yield return new WaitForSeconds(_patrol.IdleTimeBetweenPoints + 0.1f);

            Assert.AreEqual(_pathfinding.TargetLocation, closestPoint);
        }

        [UnityTest]
        public IEnumerator Patrolling_IdleDoesNotComplete_DontAssignNextPoint()
        {
            var closestPoint = new Vector3(1.0f, 2.0f, 0.0f);
            var furthestPoint = new Vector3(20.0f, 30.0f, 0.0f);

            _patrol.PatrolPoints.Add(furthestPoint);
            _patrol.PatrolPoints.Add(closestPoint);

            _patrol.StartPatrolling();

            _pathfinding.CompleteDelegate();

            yield return new WaitForSeconds(_patrol.IdleTimeBetweenPoints - 0.1f);

            Assert.AreEqual(_pathfinding.TargetLocation, closestPoint);
        }

        [UnityTest]
        public IEnumerator Patrolling_IdleCompletes_Cancelled_NoNextPointAssigned()
        {
            var closestPoint = new Vector3(1.0f, 2.0f, 0.0f);
            var furthestPoint = new Vector3(20.0f, 30.0f, 0.0f);

            _patrol.PatrolPoints.Add(furthestPoint);
            _patrol.PatrolPoints.Add(closestPoint);

            _patrol.StartPatrolling();

            _pathfinding.CompleteDelegate();

            _patrol.StopPatrolling();

            yield return new WaitForSeconds(_patrol.IdleTimeBetweenPoints + 0.1f);

            Assert.AreEqual(_pathfinding.TargetLocation, closestPoint);
        }
    }
}
