// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.AI.Goals.CustomGoals;
using Assets.Scripts.Components.Emote;
using Assets.Scripts.Components.Health;
using Assets.Scripts.Messaging;
using Assets.Scripts.Test.AI.Pathfinding;
using Assets.Scripts.Test.Components.Emote;
using Assets.Scripts.Test.Messaging;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.AI.Goals.CustomGoals
{
    [TestFixture]
    public class InvestigateDamageGoalTestFixture
    {
        private MockPathfindingComponent _pathfinding;
        private MockEmoteComponent _emote;

        private readonly InvestigateDisturbanceGoalParams _params = new InvestigateDisturbanceGoalParams { DesirabilityOnDetection = 0.6f, IdleDelayOnDetection = 2.0f, IdleDelayOnObservation = 3.0f};
        private InvestigateDamageGoal _goal;
        private GameObject _targetObject;

        [SetUp]
        public void BeforeTest()
        {
            _pathfinding = new GameObject().AddComponent<MockPathfindingComponent>();
            _emote = _pathfinding.gameObject.AddComponent<MockEmoteComponent>();

            _pathfinding.gameObject.transform.position = new Vector3(2.0f, 100.0f, 1.0f);

            _pathfinding.gameObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();

            _goal = new InvestigateDamageGoal(_pathfinding.gameObject, _params);
            _goal.RegisterGoal();

            _targetObject = new GameObject();

            _targetObject.transform.position = new Vector3(12.0f, 13.0f, -11.0f);
        }

        [TearDown]
        public void AfterTest()
        {
            _targetObject = null;

            _goal.UnregisterGoal();
            _goal = null;

            _emote = null;
            _pathfinding = null;
        }

        [Test]
        public void Desirability_NoDamage_Zero()
        {
            Assert.AreEqual(0.0f, _goal.CalculateDesirability());
        }

        [Test]
        public void Desirability_DamageAuthorMissing_Zero()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_pathfinding.gameObject, new HealthChangedMessage(-2, 1, null));
            Assert.AreEqual(0.0f, _goal.CalculateDesirability());
        }

        [Test]
        public void Desirability_PositiveHealthChange_Zero()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_pathfinding.gameObject, new HealthChangedMessage(1, 1, new GameObject()));
            Assert.AreEqual(0.0f, _goal.CalculateDesirability());
        }

        [Test]
        public void Desirability_ZeroHealthChange_Zero()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_pathfinding.gameObject, new HealthChangedMessage(0, 1, new GameObject()));
            Assert.AreEqual(0.0f, _goal.CalculateDesirability());
        }

        [Test]
        public void Desirability_Damage_ParamSpecified()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_pathfinding.gameObject, new HealthChangedMessage(-2, 1, new GameObject()));
            Assert.AreEqual(_params.DesirabilityOnDetection, _goal.CalculateDesirability());
        }

        [Test]
        public void Initialised_SetsEmoteStateToSuspicious()
        {
            _goal.Initialise();

            Assert.AreEqual(EEmoteState.Suspicious, _emote.SetEmoteStateResult);
        }

        [Test]
        public void Initialised_IdleTimeDoesNotPass_DoesNotSetTargetToAuthorLocation()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_pathfinding.gameObject, new HealthChangedMessage(-2, 1, new GameObject()));
            _goal.Initialise();

            Assert.IsNull(_pathfinding.TargetLocation);
        }

        [Test]
        public void Initialised_IdleTimePasses_SetsTargetToAuthorLocation()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_pathfinding.gameObject, new HealthChangedMessage(-2, 1, _targetObject));
            _goal.Initialise();

            _goal.Update(_params.IdleDelayOnDetection + 0.1f);

            Assert.AreEqual(_targetObject.transform.position, _pathfinding.TargetLocation);
        }

        [Test]
        public void CompletesMovement_TimePassesLessThanDelay_StartingPositionNotSetAsTarget()
        {
            var initialLocation = _pathfinding.gameObject.transform.position;

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_pathfinding.gameObject, new HealthChangedMessage(-2, 1, _targetObject));
            _goal.Initialise();

            _goal.Update(_params.IdleDelayOnDetection + 0.1f);

            _pathfinding.CompleteDelegate();

            _goal.Update(0.0f);
            _goal.Update(_params.IdleDelayOnObservation * 0.5f);

            Assert.AreNotEqual(initialLocation, _pathfinding.TargetLocation);
        }

        [Test]
        public void CompletesMovement_TimePassesGreaterThanDelay_StartingPositionSetAsTarget()
        {
            var initialLocation = _pathfinding.gameObject.transform.position;

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_pathfinding.gameObject, new HealthChangedMessage(-2, 1, _targetObject));
            _goal.Initialise();

            _goal.Update(_params.IdleDelayOnDetection + 0.1f);

            _pathfinding.CompleteDelegate();

            _goal.Update(0.0f);
            _goal.Update(_params.IdleDelayOnObservation + 0.1f);

            Assert.AreEqual(initialLocation, _pathfinding.TargetLocation);
        }

        [Test]
        public void Desirability_InProgress_ParamSpecified()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_pathfinding.gameObject, new HealthChangedMessage(-2, 1, _targetObject));
            _goal.CalculateDesirability();
            _goal.Initialise();

            _goal.Update(_params.IdleDelayOnDetection + 0.1f);

            _pathfinding.CompleteDelegate();

            _goal.Update(0.0f);

            Assert.AreEqual(_params.DesirabilityOnDetection, _goal.CalculateDesirability());
        }

        [Test]
        public void Terminated_SetsRotationToInitialRotation()
        {
            var initialRotation = _pathfinding.gameObject.transform.eulerAngles;

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_pathfinding.gameObject, new HealthChangedMessage(-2, 1, _targetObject));
            _goal.Initialise();

            _pathfinding.gameObject.transform.eulerAngles = new Vector3(200.0f, 100.0f, 30.0f);

            _goal.Terminate();

            Assert.AreEqual(initialRotation, _pathfinding.gameObject.transform.eulerAngles);
        }

        [Test]
        public void Desirability_Terminated_Zero()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_pathfinding.gameObject, new HealthChangedMessage(-2, 1, _targetObject));
            _goal.CalculateDesirability();
            _goal.Initialise();

            _goal.Terminate();

            Assert.AreEqual(0.0f, _goal.CalculateDesirability());
        }

        [Test]
        public void Terminated_SetsEmoteStateToNone()
        {
            _goal.Initialise();
            _goal.Terminate();

            Assert.AreEqual(EEmoteState.None, _emote.SetEmoteStateResult);
        }
    }
}

