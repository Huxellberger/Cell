// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.ActionStateMachine.States.Transforming;
using Assets.Scripts.Components.Species;
using Assets.Scripts.Test.Components.ActionStateMachine;
using Assets.Scripts.Test.Components.Objects.Totem;
using Assets.Scripts.Test.Components.Species;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Editor.UnitTests.Components.Objects.Totem
{
    [TestFixture]
    public class TotemComponentTestFixture
    {
        private TestTotemComponent _totem;
        private MockActionStateMachineComponent _actionStateMachineComponent;
        private MockSpeciesComponent _species;

        private MockSpeciesComponent _transformSpecies;

        [SetUp]
        public void BeforeTest()
        {
            _totem = new GameObject().AddComponent<TestTotemComponent>();

            _actionStateMachineComponent = new GameObject().AddComponent<MockActionStateMachineComponent>();
            _species = _actionStateMachineComponent.gameObject.AddComponent<MockSpeciesComponent>();

            var transformObject = new GameObject();
            _transformSpecies = transformObject.AddComponent<MockSpeciesComponent>();
            _totem.TransformTypePrefab = transformObject;

            _totem.TestStart();
        }

        [TearDown]
        public void AfterTest()
        {
            _transformSpecies = null;
            _species = null;
            _actionStateMachineComponent = null;

            _totem = null;
        }

        [Test]
        public void CanInteract_SameSpecies_False()
        {
            _actionStateMachineComponent.IsActionStateActiveResult = true;

            _transformSpecies.GetCurrentSpeciesTypeResult = ESpeciesType.Human;
            _species.GetCurrentSpeciesTypeResult = ESpeciesType.Human;
            Assert.IsFalse(_totem.CanInteract(_actionStateMachineComponent.gameObject));
        }

        [Test]
        public void CanInteract_WrongState_False()
        {
            _actionStateMachineComponent.IsActionStateActiveResult = false;

            _transformSpecies.GetCurrentSpeciesTypeResult = ESpeciesType.Human;
            _species.GetCurrentSpeciesTypeResult = ESpeciesType.Rat;

            Assert.IsFalse(_totem.CanInteract(_actionStateMachineComponent.gameObject));
        }

        [Test]
        public void CanInteract_QueriesIfLocomotionOnLocomotionTrack()
        {
            _totem.CanInteract(_actionStateMachineComponent.gameObject);

            Assert.AreEqual(EActionStateId.Locomotion, _actionStateMachineComponent.IsActionStateActiveIdQuery);
            Assert.AreEqual(EActionStateMachineTrack.Locomotion, _actionStateMachineComponent.IsActionStateActiveTrackQuery);
        }

        [Test]
        public void CanInteract_CorrectStateAndDifferingSpecies_True()
        {
            _actionStateMachineComponent.IsActionStateActiveResult = true;

            _transformSpecies.GetCurrentSpeciesTypeResult = ESpeciesType.Human;
            _species.GetCurrentSpeciesTypeResult = ESpeciesType.Rat;

            Assert.IsTrue(_totem.CanInteract(_actionStateMachineComponent.gameObject));
        }

        [Test]
        public void Interact_NoTransformTypePrefab_ErrorThrownAndNoStateChange()
        {
            _actionStateMachineComponent.IsActionStateActiveResult = true;

            _transformSpecies.GetCurrentSpeciesTypeResult = ESpeciesType.Human;
            _species.GetCurrentSpeciesTypeResult = ESpeciesType.Rat;

            _totem.TransformTypePrefab = null;

            LogAssert.Expect(LogType.Error, "No Transform type assigned!");

            _totem.OnInteract(_actionStateMachineComponent.gameObject);

            Assert.IsNull(_actionStateMachineComponent.RequestedId);
        }

        [Test]
        public void Interact_TransformTypePrefab_RequestTransformingActionState()
        {
            _actionStateMachineComponent.IsActionStateActiveResult = true;

            _transformSpecies.GetCurrentSpeciesTypeResult = ESpeciesType.Human;
            _species.GetCurrentSpeciesTypeResult = ESpeciesType.Rat;

            _totem.OnInteract(_actionStateMachineComponent.gameObject);

            Assert.AreEqual(EActionStateId.Transforming, _actionStateMachineComponent.RequestedId);
            Assert.AreEqual(EActionStateMachineTrack.Locomotion, _actionStateMachineComponent.RequestedTrack);
            Assert.AreSame(_totem.TransformTypePrefab, ((TransformingActionStateInfo)_actionStateMachineComponent.RequestedInfo).TransformTypePrefab);
        }
    }
}
