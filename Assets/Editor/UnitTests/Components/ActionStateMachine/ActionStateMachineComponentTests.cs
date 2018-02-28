// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using Assets.Editor.UnitTests.Components.ActionStateMachine.Builder;
using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.ActionStateMachine.Builder;
using Assets.Scripts.Test.Components.ActionStateMachine;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine
{
    [TestFixture]
    public class ActionStateMachineComponentTestFixture
    {
        TestActionStateMachineComponent _actionStateMachineComponent;

        [SetUp]
        public void BeforeTest()
        {
            _actionStateMachineComponent = new GameObject().AddComponent<TestActionStateMachineComponent>();
            _actionStateMachineComponent.Params = ScriptableObject.CreateInstance<ActionStateParams>();
            _actionStateMachineComponent.TestAwake();
            _actionStateMachineComponent.SetActionStateCreator(new MockActionStateCreator());
        }

        [TearDown]
        public void AfterTest()
        {
            _actionStateMachineComponent = null;
        }

        [Test]
        public void WhenCreated_AllTracksHaveNullActionStateId()
        {
            var tracks = Enum.GetValues(typeof(EActionStateMachineTrack));

            foreach(EActionStateMachineTrack track in tracks)
            {
                Assert.IsTrue(_actionStateMachineComponent.IsActionStateActiveOnTrack(track, EActionStateId.Null));
            }
        }

        [Test]
        public void IsActionStateActiveOnTrack_WrongTrackCorrectId_False()
        {
            const EActionStateId expectedStateId = EActionStateId.Locomotion;
            const EActionStateMachineTrack wrongTrack = EActionStateMachineTrack.None;
            const EActionStateMachineTrack changedTrack = EActionStateMachineTrack.Locomotion;

            _actionStateMachineComponent.RequestActionState(changedTrack, expectedStateId, new ActionStateInfo());

            Assert.IsFalse(_actionStateMachineComponent.IsActionStateActiveOnTrack(wrongTrack, expectedStateId));
        }

        [Test]
        public void IsActionStateActiveOnTrack_CorrectTrackWrongId_False()
        {
            const EActionStateId wrongId = EActionStateId.Null;
            const EActionStateId expectedStateId = EActionStateId.Locomotion;
            const EActionStateMachineTrack changedTrack = EActionStateMachineTrack.Locomotion;

            _actionStateMachineComponent.RequestActionState(changedTrack, expectedStateId, new ActionStateInfo());

            Assert.IsFalse(_actionStateMachineComponent.IsActionStateActiveOnTrack(changedTrack, wrongId));
        }

        [Test]
        public void RequestActionState_SetsTrackToNewIdAndIsActionStateActiveReturnsTrue()
        {
            const EActionStateId expectedStateId = EActionStateId.Locomotion;
            const EActionStateMachineTrack changedTrack = EActionStateMachineTrack.Locomotion;

            _actionStateMachineComponent.RequestActionState(changedTrack, expectedStateId, new ActionStateInfo());

            Assert.IsTrue(_actionStateMachineComponent.IsActionStateActiveOnTrack(changedTrack, expectedStateId));
        }

        [Test]
        public void RequestActionState_SetsCallsEndOnPriorActionStateAndStartOnNewOne()
        {
            const EActionStateId newStateId = EActionStateId.Null;
            const EActionStateId oldStateId = EActionStateId.Locomotion;
            const EActionStateMachineTrack changedTrack = EActionStateMachineTrack.Locomotion;

            _actionStateMachineComponent.RequestActionState(changedTrack, oldStateId, new ActionStateInfo());
            var oldActionState = (TestActionState)_actionStateMachineComponent.GetActionStateOnTrack(changedTrack);
            _actionStateMachineComponent.RequestActionState(changedTrack, newStateId, new ActionStateInfo());
            var newActionState = (TestActionState)_actionStateMachineComponent.GetActionStateOnTrack(changedTrack);

            Assert.IsTrue(oldActionState.OnEndCalled);
            Assert.IsTrue(newActionState.OnStartCalled);
        }

        [Test]
        public void Update_CallsUpdateOnActionState()
        {
            const EActionStateMachineTrack track = EActionStateMachineTrack.Locomotion;

            _actionStateMachineComponent.RequestActionState(track, EActionStateId.Locomotion, new ActionStateInfo());

            var actionState = (TestActionState) _actionStateMachineComponent.GetActionStateOnTrack(track);
            _actionStateMachineComponent.TestUpdate();

            Assert.IsTrue(actionState.OnUpdateCalled);
            Assert.AreEqual(actionState.OnUpdateValue, Time.deltaTime);
        }

        [Test]
        public void Destroyed_CallsEndOnAllStates()
        {
            var actionStates = new List<TestActionState>();

            var tracks = Enum.GetValues(typeof(EActionStateMachineTrack));
            foreach (EActionStateMachineTrack track in tracks)
            {
                _actionStateMachineComponent.RequestActionState(track, EActionStateId.Locomotion, new ActionStateInfo());
                actionStates.Add((TestActionState)_actionStateMachineComponent.GetActionStateOnTrack(track));
            }

            _actionStateMachineComponent.TestDestroy();

            foreach (var actionState in actionStates)
            {
                Assert.IsTrue(actionState.OnEndCalled);
            }
        }
    }
}

#endif
