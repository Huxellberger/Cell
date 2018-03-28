// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Editor.UnitTests.Helpers;
using Assets.Scripts.AI.Goals.CustomGoals;
using Assets.Scripts.AI.Vision;
using Assets.Scripts.Components.Emote;
using Assets.Scripts.Messaging;
using Assets.Scripts.Test.AI.Pathfinding;
using Assets.Scripts.Test.Components.ActionStateMachine;
using Assets.Scripts.Test.Components.Character.Attack;
using Assets.Scripts.Test.Components.Emote;
using Assets.Scripts.Test.Messaging;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.AI.Goals.CustomGoals
{
    [TestFixture]
    public class PursuitTargetGoalTestFixture
    {
        private MockPathfindingComponent _pathfinding;
        private MockEmoteComponent _emote;
        private MockAttackComponent _attack;

        private GameObject _targetObject;

        private readonly PursuitTargetGoalParams _params = new PursuitTargetGoalParams { TargetDetectedDesirability = 0.6f, AbandonPursuitRadiusSquared = 100.0f};
        private PursuitTargetGoal _goal;

        [SetUp]
        public void BeforeTest()
        {
            _pathfinding = new GameObject().AddComponent<MockPathfindingComponent>();
            _emote = _pathfinding.gameObject.AddComponent<MockEmoteComponent>();

            _pathfinding.gameObject.transform.position = new Vector3(2.0f, 0.0f, 1.0f);

            _pathfinding.gameObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();
            _attack = _pathfinding.gameObject.AddComponent<MockAttackComponent>();
            _attack.CanAttackResult = true;

            _goal = new PursuitTargetGoal(_pathfinding.gameObject, _params);
            _goal.RegisterGoal();

            _targetObject = new GameObject();
            _targetObject.transform.position = new Vector3(1.0f, 2.0f, 0.0f);
            _targetObject.AddComponent<MockActionStateMachineComponent>();
        }

        [TearDown]
        public void AfterTest()
        {
            _targetObject = null;

            _goal.UnregisterGoal();
            _goal = null;

            _attack = null;
            _emote = null;
            _pathfinding = null;
        }

        [Test]
        public void Desirability_NoTarget_Zero()
        {
            Assert.AreEqual(0.0f, _goal.CalculateDesirability());
        }

        [Test]
        public void Desirability_Disturbance_ParamSpecified()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_pathfinding.gameObject, new SuspiciousObjectDetectedMessage(_targetObject));
            Assert.AreEqual(_params.TargetDetectedDesirability, _goal.CalculateDesirability());
        }

        [Test]
        public void Initialised_SetsEmoteStateToAlert()
        {
            _goal.Initialise();

            Assert.AreEqual(EEmoteState.Alerted, _emote.SetEmoteStateResult);
        }

        [Test]
        public void Initialised_RotatesToFaceGoal()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_pathfinding.gameObject, new SuspiciousObjectDetectedMessage(_targetObject));
            _goal.Initialise();

            ExtendedAssertions.AssertVectorsNearlyEqual(_pathfinding.gameObject.transform.up, (_targetObject.transform.position - _pathfinding.gameObject.transform.position).normalized);
        }

        [Test]
        public void Initialised_SetsFollowTargetToTargetObject()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_pathfinding.gameObject, new SuspiciousObjectDetectedMessage(_targetObject));
            _goal.Initialise();

            Assert.AreSame(_pathfinding.SetFollowTargetResult, _targetObject);
        }

        [Test]
        public void Update_TriesToAttackTarget()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_pathfinding.gameObject, new SuspiciousObjectDetectedMessage(_targetObject));
            _goal.Initialise();

            _goal.Update(1.0f);

            Assert.IsTrue(_attack.AttackCalled);
            Assert.AreSame(_targetObject, _attack.AttackedGameObject);
        }

        [Test]
        public void Desirability_InProgress_ParamSpecified()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_pathfinding.gameObject, new SuspiciousObjectDetectedMessage(_targetObject));
            _goal.CalculateDesirability();
            _goal.Initialise();

            Assert.AreEqual(_params.TargetDetectedDesirability, _goal.CalculateDesirability());
        }

        [Test]
        public void Desirability_OutsideFollowRadius_Zeroes()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_pathfinding.gameObject, new SuspiciousObjectDetectedMessage(_targetObject));
            _goal.Initialise();

            _targetObject.transform.position = new Vector3(_params.AbandonPursuitRadiusSquared, _params.AbandonPursuitRadiusSquared, 0.0f);

            Assert.AreEqual(0.0f, _goal.CalculateDesirability());
        }

        [Test]
        public void Terminated_SetsTargetDestinationToOriginalPosition()
        {
            var initialPosition = _pathfinding.gameObject.transform.position;

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_pathfinding.gameObject, new SuspiciousObjectDetectedMessage(_targetObject));
            _goal.Initialise();

            _goal.Terminate();

            Assert.AreEqual(initialPosition, _pathfinding.TargetLocation);
        }

        [Test]
        public void Terminated_SetsFollowTargetToNull()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_pathfinding.gameObject, new SuspiciousObjectDetectedMessage(_targetObject));
            _goal.Initialise();

            _goal.Terminate();

            Assert.IsNull(_pathfinding.SetFollowTargetResult);
        }

        [Test]
        public void Desirability_Terminated_Zero()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_pathfinding.gameObject, new SuspiciousObjectDetectedMessage(_targetObject));
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
