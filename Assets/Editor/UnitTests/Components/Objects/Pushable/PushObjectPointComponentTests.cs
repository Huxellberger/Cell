// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.ActionStateMachine.States.PushObjectActionState;
using Assets.Scripts.Components.Objects.Pushable;
using Assets.Scripts.Components.Species;
using Assets.Scripts.Test.Components.ActionStateMachine;
using Assets.Scripts.Test.Components.Equipment.Holdables;
using Assets.Scripts.Test.Components.Species;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Objects.Pushable
{
    [TestFixture]
    public class PushObjectPointComponentTestFixture 
        : MonoBehaviour
    {
        private MockActionStateMachineComponent _actionStateMachine;
        private MockSpeciesComponent _species;
        private MockHeldItemComponent _heldItem;

        private GameObject _pushable;
        private PushObjectPointComponent _pushPoint;

        [SetUp]
        public void BeforeTest()
        {
            _actionStateMachine = new GameObject().AddComponent<MockActionStateMachineComponent>();
            _species = _actionStateMachine.gameObject.AddComponent<MockSpeciesComponent>();
            _heldItem = _actionStateMachine.gameObject.AddComponent<MockHeldItemComponent>();

            _pushable = new GameObject();
            _pushPoint = new GameObject().AddComponent<PushObjectPointComponent>();

            _pushPoint.PushableObject = _pushable;
        }

        [TearDown]
        public void AfterTest()
        {
            _pushPoint = null;
            _pushable = null;

            _heldItem = null;
            _species = null;
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
            const ESpeciesType validSpecies = ESpeciesType.Human;

            _species.GetCurrentSpeciesTypeResult = validSpecies;
            _pushPoint.PushableSpeciesTypes.Add(validSpecies);

            Assert.IsFalse(_pushPoint.CanInteract(_actionStateMachine.gameObject));
        }

        [Test]
        public void CanInteract_NotValidSpecies_False()
        {
            _actionStateMachine.IsActionStateActiveResult = true;

            _species.GetCurrentSpeciesTypeResult = ESpeciesType.Human;
            _pushPoint.PushableSpeciesTypes.Add(ESpeciesType.Rat);

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

            const ESpeciesType validSpecies = ESpeciesType.Human;

            _species.GetCurrentSpeciesTypeResult = validSpecies;
            _pushPoint.PushableSpeciesTypes.Add(validSpecies);

            _pushPoint.OnInteract(_actionStateMachine.gameObject);

            Assert.IsTrue(_pushPoint.CanInteract(_actionStateMachine.gameObject));
        }

        [Test]
        public void CanInteract_NotOwnerAndOwner_False()
        {
            const ESpeciesType validSpecies = ESpeciesType.Human;

            var owner = new GameObject().AddComponent<MockActionStateMachineComponent>();
            var ownerSpecies = owner.gameObject.AddComponent<MockSpeciesComponent>();

            owner.IsActionStateActiveResult = true;
            ownerSpecies.GetCurrentSpeciesTypeResult = validSpecies;

            _actionStateMachine.IsActionStateActiveResult = true;

            _species.GetCurrentSpeciesTypeResult = validSpecies;
            _pushPoint.PushableSpeciesTypes.Add(validSpecies);

            _pushPoint.OnInteract(owner.gameObject);

            Assert.IsFalse(_pushPoint.CanInteract(_actionStateMachine.gameObject));
        }

        [Test]
        public void CanInteract_NullPushable_False()
        {
            _actionStateMachine.IsActionStateActiveResult = true;

            const ESpeciesType validSpecies = ESpeciesType.Human;

            _species.GetCurrentSpeciesTypeResult = validSpecies;
            _pushPoint.PushableSpeciesTypes.Add(validSpecies);

            _pushPoint.PushableObject = null;

            Assert.IsFalse(_pushPoint.CanInteract(_actionStateMachine.gameObject));
        }

        [Test]
        public void CanInteract_HeldItem_False()
        {
            _actionStateMachine.IsActionStateActiveResult = true;

            _heldItem.GetHeldItemResult = new GameObject().AddComponent<MockHoldableComponent>();

            const ESpeciesType validSpecies = ESpeciesType.Human;

            _species.GetCurrentSpeciesTypeResult = validSpecies;
            _pushPoint.PushableSpeciesTypes.Add(validSpecies);

            Assert.IsFalse(_pushPoint.CanInteract(_actionStateMachine.gameObject));
        }

        [Test]
        public void CanInteract_ValidStateAndSpecies_True()
        {
            _actionStateMachine.IsActionStateActiveResult = true;

            const ESpeciesType validSpecies = ESpeciesType.Human;

            _species.GetCurrentSpeciesTypeResult = validSpecies;
            _pushPoint.PushableSpeciesTypes.Add(validSpecies);

            Assert.IsTrue(_pushPoint.CanInteract(_actionStateMachine.gameObject));
        }

        [Test]
        public void OnInteract_NotOwner_PushesInteractorIntoPushObject()
        {
            _actionStateMachine.IsActionStateActiveResult = true;

            const ESpeciesType validSpecies = ESpeciesType.Human;

            _species.GetCurrentSpeciesTypeResult = validSpecies;
            _pushPoint.PushableSpeciesTypes.Add(validSpecies);

            _pushPoint.OnInteract(_actionStateMachine.gameObject);

            Assert.NotNull((PushObjectActionStateInfo)_actionStateMachine.RequestedInfo);
            Assert.AreEqual(EActionStateMachineTrack.Locomotion, _actionStateMachine.RequestedTrack);
            Assert.AreEqual(EActionStateId.PushObject, _actionStateMachine.RequestedId);
        }

        [Test]
        public void OnInteract_Owner_PushesInteractorIntoLocomotion()
        {
            _actionStateMachine.IsActionStateActiveResult = true;

            const ESpeciesType validSpecies = ESpeciesType.Human;

            _species.GetCurrentSpeciesTypeResult = validSpecies;
            _pushPoint.PushableSpeciesTypes.Add(validSpecies);

            _pushPoint.OnInteract(_actionStateMachine.gameObject);
            _pushPoint.OnInteract(_actionStateMachine.gameObject);

            Assert.AreEqual(EActionStateMachineTrack.Locomotion, _actionStateMachine.RequestedTrack);
            Assert.AreEqual(EActionStateId.Locomotion, _actionStateMachine.RequestedId);
        }
    }
}
