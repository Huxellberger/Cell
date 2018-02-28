// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Editor.UnitTests.Helpers;
using Assets.Scripts.Test.AI.Pathfinding;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Editor.UnitTests.AI.Pathfinding
{
    [TestFixture]
    public class PathfindingComponentTestFixture
    {
        private NavMeshAgent _agent;
        private TestPathfindingComponent _pathfinding;
        /*
        [SetUp]
        public void BeforeTest()
        {
            _agent = new GameObject().AddComponent<NavMeshAgent>();
            _agent.enabled = true;
            _pathfinding = _agent.gameObject.AddComponent<TestPathfindingComponent>();

            _pathfinding.TestStart();
        }

        [TearDown]
        public void AfterTest()
        {
            _pathfinding = null;
            _agent = null;
        }

        [Test]
        public void SetFollowTarget_ValidTarget_UpdatesAgentToTargetLocation()
        {
            var target = new GameObject();
            target.transform.position = new Vector3(100.0f, 200.0f, 30.0f);

            _pathfinding.SetFollowTarget(target);

            ExtendedAssertions.AssertVectorsNearlyEqual(_agent.destination, target.transform.position);
        }

        [Test]
        public void Update_ValidTarget_LessThanReplot_SameDestination()
        {
            var target = new GameObject();

            var originalLocation = new Vector3(100.0f, 200.0f, 30.0f);
            var newLocation = new Vector3(10.0f, 12.0f, 13.0f);

            target.transform.position = originalLocation;

            _pathfinding.SetFollowTarget(target);

            target.transform.position = newLocation;

            _pathfinding.TestUpdate(_pathfinding.FollowReplotDelay * 0.5f);

            ExtendedAssertions.AssertVectorsNearlyEqual(_agent.destination, originalLocation);
        }

        [Test]
        public void Update_ValidTarget_GreaterThanReplot_NewTargetDestination()
        {
            var target = new GameObject();

            var originalLocation = new Vector3(100.0f, 200.0f, 30.0f);
            var newLocation = new Vector3(10.0f, 12.0f, 13.0f);

            target.transform.position = originalLocation;

            _pathfinding.SetFollowTarget(target);

            target.transform.position = newLocation;

            _pathfinding.TestUpdate(_pathfinding.FollowReplotDelay + 0.1f);

            ExtendedAssertions.AssertVectorsNearlyEqual(_agent.destination, newLocation);
        }

        [Test]
        public void SetFollowTarget_Null_UpdatesAgentToCurrentLocation()
        {
            _pathfinding.SetFollowTarget(null);

            ExtendedAssertions.AssertVectorsNearlyEqual(_agent.destination, _pathfinding.gameObject.transform.position);
        }
        */
    }
}
