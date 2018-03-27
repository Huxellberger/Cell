// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.ActionStateMachine.States.PushObjectActionState;
using Assets.Scripts.Components.Species;
using Assets.Scripts.Test.Components.ActionStateMachine;
using Assets.Scripts.Test.Components.Equipment.Holdables;
using Assets.Scripts.Test.Components.Objects.Pushable;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Objects.Pushable
{
    [TestFixture]
    public class PushObjectPointComponentTestFixture 
    {
        private MockActionStateMachineComponent _actionStateMachine;
        private MockHeldItemComponent _heldItem;

        private GameObject _pushable;
        private TestPushObjectPointComponent _pushPoint;

        [SetUp]
        public void BeforeTest()
        {
            _actionStateMachine = new GameObject().AddComponent<MockActionStateMachineComponent>();
            _heldItem = _actionStateMachine.gameObject.AddComponent<MockHeldItemComponent>();

            _pushable = new GameObject();
            _pushPoint = new GameObject().AddComponent<TestPushObjectPointComponent>();

            _pushPoint.PushableObject = _pushable;
        }

        [TearDown]
        public void AfterTest()
        {
            _pushPoint = null;
            _pushable = null;

            _heldItem = null;
            _actionStateMachine = null;
        }

        [Test]
        public void CanInteract_Null_False()
        {
            Assert.IsFalse(_pushPoint.CanInteract(null));
        }

        [Test]
        public void CanInteract_NotInLocomotion_False()
        {
            Assert.IsFalse(_pushPoint.CanInteract(_actionStateMachine.gameObject));
        }

        [Test]
        public void CanInteract_QueriesLocomotion()
        {
            _pushPoint.CanInteract(_actionStateMachine.gameObject);

            Assert.AreEqual(EActionStateMachineTrack.Locomotion, _actionStateMachine.IsActionStateActiveTrackQuery);
            Assert.AreEqual(EActionStateId.Locomotion, _actionStateMachine.IsActionStateActiveIdQuery);
        }

        [Test]
        public void CanInteract_IsOwner_True()
        {
            _actionStateMachine.IsActionStateActiveResult = true;

            _pushPoint.OnInteract(_actionStateMachine.gameObject);

            Assert.IsTrue(_pushPoint.CanInteract(_actionStateMachine.gameObject));
        }

        [Test]
        public void CanInteract_NotOwnerAndOwner_False()
        {
            var owner = new GameObject().AddComponent<MockActionStateMachineComponent>();

            owner.IsActionStateActiveResult = true;

            _actionStateMachine.IsActionStateActiveResult = true;

            _pushPoint.OnInteract(owner.gameObject);

            Assert.IsFalse(_pushPoint.CanInteract(_actionStateMachine.gameObject));
        }

        [Test]
        public void CanInteract_NullPushable_False()
        {
            _actionStateMachine.IsActionStateActiveResult = true;

            _pushPoint.PushableObject = null;

            Assert.IsFalse(_pushPoint.CanInteract(_actionStateMachine.gameObject));
        }

        [Test]
        public void CanInteract_HeldItem_False()
        {
            _actionStateMachine.IsActionStateActiveResult = true;

            _heldItem.GetHeldItemResult = new GameObject().AddComponent<MockHoldableComponent>();

            Assert.IsFalse(_pushPoint.CanInteract(_actionStateMachine.gameObject));
        }

        [Test]
        public void CanInteract_ExtendedPushConditionInvalid_False()
        {
            _actionStateMachine.IsActionStateActiveResult = true;
            _pushPoint.ExtendedPushConditionsValidResult = false;

            Assert.IsFalse(_pushPoint.CanInteract(_actionStateMachine.gameObject));
        }

        [Test]
        public void CanInteract_ValidStateAndSpecies_True()
        {
            _actionStateMachine.IsActionStateActiveResult = true;

            Assert.IsTrue(_pushPoint.CanInteract(_actionStateMachine.gameObject));
        }

        [Test]
        public void OnInteract_NotOwner_PushesInteractorIntoPushObject()
        {
            _actionStateMachine.IsActionStateActiveResult = true;

            const ESpeciesType validSpecies = ESpeciesType.Human;

            _pushPoint.OnInteract(_actionStateMachine.gameObject);

            Assert.NotNull((PushObjectActionStateInfo)_actionStateMachine.RequestedInfo);
            Assert.AreEqual(EActionStateMachineTrack.Locomotion, _actionStateMachine.RequestedTrack);
            Assert.AreEqual(EActionStateId.PushObject, _actionStateMachine.RequestedId);
        }

        [Test]
        public void OnInteract_Owner_PushesInteractorIntoLocomotion()
        {
            _actionStateMachine.IsActionStateActiveResult = true;

            _pushPoint.OnInteract(_actionStateMachine.gameObject);
            _pushPoint.OnInteract(_actionStateMachine.gameObject);

            Assert.AreEqual(EActionStateMachineTrack.Locomotion, _actionStateMachine.RequestedTrack);
            Assert.AreEqual(EActionStateId.Locomotion, _actionStateMachine.RequestedId);
        }
    }
}
