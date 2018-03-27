// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.ActionStateMachine.States.PushObjectActionState;
using Assets.Scripts.Components.Objects.Pushable;
using Assets.Scripts.Components.Species;
using Assets.Scripts.Test.Components.ActionStateMachine;
using Assets.Scripts.Test.Components.Equipment.Holdables;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Objects.Pushable
{
    [TestFixture]
    public class OrganicPushObjectPointComponentTestFixture
    {
        private MockActionStateMachineComponent _actionStateMachine;
        private MockHeldItemComponent _heldItem;

        private GameObject _pushable;
        private MockActionStateMachineComponent _pushableASM;
        private OrganicPushObjectPointComponent _pushPoint;

        [SetUp]
        public void BeforeTest()
        {
            _actionStateMachine = new GameObject().AddComponent<MockActionStateMachineComponent>();
            _heldItem = _actionStateMachine.gameObject.AddComponent<MockHeldItemComponent>();

            _pushable = new GameObject();
            _pushableASM = _pushable.AddComponent<MockActionStateMachineComponent>();
            _pushableASM.IsActionStateActiveResult = true;
            
            _pushPoint = new GameObject().AddComponent<OrganicPushObjectPointComponent>();

            _pushPoint.PushableObject = _pushable;
        }

        [TearDown]
        public void AfterTest()
        {
            _pushPoint = null;
            _pushableASM = null;
            _pushable = null;

            _heldItem = null;
            _actionStateMachine = null;
        }

        [Test]
        public void CanInteract_QueriesDeadOnPushable()
        {
            _actionStateMachine.IsActionStateActiveResult = true;
            _pushPoint.CanInteract(_actionStateMachine.gameObject);

            Assert.AreEqual(EActionStateMachineTrack.Locomotion, _pushableASM.IsActionStateActiveTrackQuery);
            Assert.AreEqual(EActionStateId.Dead, _pushableASM.IsActionStateActiveIdQuery);
        }

        [Test]
        public void CanInteract_DeadInactiveOnPushable_True()
        {
            _pushableASM.IsActionStateActiveResult = false;
            _actionStateMachine.IsActionStateActiveResult = true;

            Assert.IsFalse(_pushPoint.CanInteract(_actionStateMachine.gameObject));
        }

        [Test]
        public void CanInteract_DeadActiveOnPushable_True()
        {
            _actionStateMachine.IsActionStateActiveResult = true;

            Assert.IsTrue(_pushPoint.CanInteract(_actionStateMachine.gameObject));
        }
    }
}
