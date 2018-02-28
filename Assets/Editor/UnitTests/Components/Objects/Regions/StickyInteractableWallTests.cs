// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.ActionStateMachine.States.SurfaceSticking;
using Assets.Scripts.Components.Objects.Regions;
using Assets.Scripts.Components.Species;
using Assets.Scripts.Test.Components.ActionStateMachine;
using Assets.Scripts.Test.Components.Species;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Objects.Regions
{
    [TestFixture]
    public class StickyInteractableWallTestFixture
    {
        private StickyInteractableWall _interactableWall;
        private MockActionStateMachineComponent _actionStateMachine;
        private MockSpeciesComponent _species;
        private ESpeciesType _validSpecies;

        [SetUp]
        public void BeforeTest()
        {
            var interactor = new GameObject();
            _actionStateMachine = interactor.AddComponent<MockActionStateMachineComponent>();
            _species = interactor.AddComponent<MockSpeciesComponent>();

            _interactableWall = new GameObject().AddComponent<StickyInteractableWall>();
            _validSpecies = ESpeciesType.Human;
            _interactableWall.StickySpeciesTypes.Add(_validSpecies);
        }

        [TearDown]
        public void AfterTest()
        {
            _interactableWall = null;

            _species = null;
            _actionStateMachine = null;
        }

        [Test]
        public void CanInteract_Null_False()
        {
            Assert.IsFalse(_interactableWall.CanInteract(null));
        }

        [Test]
        public void CanInteract_WrongSpecies_False()
        {
            _species.GetCurrentSpeciesTypeResult = ESpeciesType.Rat;
            Assert.IsFalse(_interactableWall.CanInteract(_species.gameObject));
        }

        [Test]
        public void CanInteract_CorrectSpecies_True()
        {
            _species.GetCurrentSpeciesTypeResult = _validSpecies;
            Assert.IsTrue(_interactableWall.CanInteract(_species.gameObject));
        }

        [Test]
        public void OnInteract_InStickyState_RequestsLocomotion()
        {
            _species.GetCurrentSpeciesTypeResult = _validSpecies;

            _actionStateMachine.IsActionStateActiveResult = true;

            _interactableWall.OnInteract(_species.gameObject);

            Assert.AreEqual(EActionStateMachineTrack.Locomotion, _actionStateMachine.RequestedTrack);
            Assert.AreEqual(EActionStateId.Locomotion, _actionStateMachine.RequestedId);
            Assert.AreSame(_species.gameObject, _actionStateMachine.RequestedInfo.Owner);
        }

        [Test]
        public void OnInteract_InStickyState_QueriesStickyState()
        {
            _species.GetCurrentSpeciesTypeResult = _validSpecies;

            _actionStateMachine.IsActionStateActiveResult = true;

            _interactableWall.OnInteract(_species.gameObject);

            Assert.AreEqual(EActionStateMachineTrack.Locomotion, _actionStateMachine.IsActionStateActiveTrackQuery);
            Assert.AreEqual(EActionStateId.SurfaceSticking, _actionStateMachine.IsActionStateActiveIdQuery);
        }

        [Test]
        public void OnInteract_NotInStickyState_RequestsStickyState()
        {
            _species.GetCurrentSpeciesTypeResult = _validSpecies;

            _actionStateMachine.IsActionStateActiveResult = false;

            _interactableWall.OnInteract(_species.gameObject);

            var info = (SurfaceStickingActionStateInfo)_actionStateMachine.RequestedInfo;

            Assert.AreEqual(EActionStateMachineTrack.Locomotion, _actionStateMachine.RequestedTrack);
            Assert.AreEqual(EActionStateId.SurfaceSticking, _actionStateMachine.RequestedId);
            Assert.AreSame(_species.gameObject, info.Owner);
            Assert.AreSame(_interactableWall.gameObject, info.Surface);
        }
    }
}
