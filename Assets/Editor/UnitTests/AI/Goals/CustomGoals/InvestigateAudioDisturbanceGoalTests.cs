// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.AI.Goals.CustomGoals;
using Assets.Scripts.Components.Emote;
using Assets.Scripts.Messaging;
using Assets.Scripts.Services.Noise;
using Assets.Scripts.Test.AI.Pathfinding;
using Assets.Scripts.Test.Components.Emote;
using Assets.Scripts.Test.Messaging;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.AI.Goals.CustomGoals
{
    [TestFixture]
    public class InvestigateAudioDisturbanceGoalTestFixture
    {
        private MockPathfindingComponent _pathfinding;
        private MockEmoteComponent _emote;

        private readonly InvestigateDisturbanceGoalParams _params = new InvestigateDisturbanceGoalParams { DesirabilityOnDetection = 0.6f, IdleDelayOnDetection = 2.0f, IdleDelayOnObservation = 3.0f};
        private InvestigateAudioDisturbanceGoal _goal;

        [SetUp]
        public void BeforeTest()
        {
            _pathfinding = new GameObject().AddComponent<MockPathfindingComponent>();
            _emote = _pathfinding.gameObject.AddComponent<MockEmoteComponent>();

            _pathfinding.gameObject.transform.position = new Vector3(2.0f, 100.0f, 1.0f);

            _pathfinding.gameObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();

            _goal = new InvestigateAudioDisturbanceGoal(_pathfinding.gameObject, _params);
            _goal.RegisterGoal();
        }

        [TearDown]
        public void AfterTest()
        {
            _goal.UnregisterGoal();
            _goal = null;

            _emote = null;
            _pathfinding = null;
        }

        [Test]
        public void Desirability_NoNoiseData_Zero()
        {
            Assert.AreEqual(0.0f, _goal.CalculateDesirability());
        }

        [Test]
        public void Desirability_NoiseData_ParamSpecified()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_pathfinding.gameObject, new NoiseHeardMessage(new NoiseData()));
            Assert.AreEqual(_params.DesirabilityOnDetection, _goal.CalculateDesirability());
        }

        [Test]
        public void Initialised_SetsEmoteStateToSuspicious()
        {
            _goal.Initialise();

            Assert.AreEqual(EEmoteState.Suspicious, _emote.SetEmoteStateResult);
        }

        [Test]
        public void Initialised_IdleTimeDoesNotPass_DoesNotSetTargetToNoiseLocation()
        {
            var expectedNoise = new NoiseData { NoiseLocation = new Vector3(2.0f, 3.0f, -1.0f) };

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_pathfinding.gameObject, new NoiseHeardMessage(expectedNoise));
            _goal.Initialise();

            Assert.IsNull(_pathfinding.TargetLocation);
        }

        [Test]
        public void Initialised_IdleTimePasses_SetsTargetToNoiseLocation()
        {
            var expectedNoise = new NoiseData { NoiseLocation = new Vector3(2.0f, 3.0f, -1.0f) };

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_pathfinding.gameObject, new NoiseHeardMessage(expectedNoise));
            _goal.Initialise();

            _goal.Update(_params.IdleDelayOnDetection + 0.1f);

            Assert.AreEqual(expectedNoise.NoiseLocation, _pathfinding.TargetLocation);
        }

        [Test]
        public void CompletesMovement_TimePassesLessThanDelay_StartingPositionNotSetAsTarget()
        {
            var initialLocation = _pathfinding.gameObject.transform.position;
            var expectedNoise = new NoiseData { NoiseLocation = new Vector3(2.0f, 3.0f, -1.0f) };

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_pathfinding.gameObject, new NoiseHeardMessage(expectedNoise));
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
            var expectedNoise = new NoiseData { NoiseLocation = new Vector3(2.0f, 3.0f, -1.0f) };

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_pathfinding.gameObject, new NoiseHeardMessage(expectedNoise));
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
            var expectedNoise = new NoiseData { NoiseLocation = new Vector3(2.0f, 3.0f, -1.0f) };

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_pathfinding.gameObject, new NoiseHeardMessage(expectedNoise));
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
            var expectedNoise = new NoiseData { NoiseLocation = new Vector3(2.0f, 3.0f, -1.0f) };

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_pathfinding.gameObject, new NoiseHeardMessage(expectedNoise));
            _goal.Initialise();

            _pathfinding.gameObject.transform.eulerAngles = new Vector3(200.0f, 100.0f, 30.0f);

            _goal.Terminate();

            Assert.AreEqual(initialRotation, _pathfinding.gameObject.transform.eulerAngles);
        }

        [Test]
        public void Desirability_Terminated_Zero()
        {
            var expectedNoise = new NoiseData { NoiseLocation = new Vector3(2.0f, 3.0f, -1.0f) };

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_pathfinding.gameObject, new NoiseHeardMessage(expectedNoise));
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
