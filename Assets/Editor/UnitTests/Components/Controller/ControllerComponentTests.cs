// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.Character;
using Assets.Scripts.Test.Components.ActionStateMachine;
using Assets.Scripts.Test.Components.Controller;
using Assets.Scripts.Test.Input;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Controller
{
    [TestFixture]
    public class ControllerComponentTestFixture
    {
        [Test]
        public void CreatePawnOfType_InstantiatesPawnOfType()
        {
            var gameObject = new GameObject();

            var controllerComponent = new GameObject().AddComponent<TestControllerComponent>();

            controllerComponent.CreatePawnOfType(gameObject);

            Assert.IsNotNull(controllerComponent.PawnInstance);
        }

        [Test]
        public void CreatePawnOfType_SetsTransformCorrectly()
        {
            var gameObject = new GameObject();

            var controllerComponent = new GameObject().AddComponent<TestControllerComponent>();

            controllerComponent.CreatePawnOfType(gameObject);

            Assert.AreEqual(controllerComponent.PawnInstance.transform, controllerComponent.gameObject.transform.parent);
            Assert.AreEqual(new Vector3(0, 0, 0), controllerComponent.gameObject.transform.localPosition);
        }

        [Test]
        public void CreatePawnOfType_SetInitialTransform_UsedOnCreation()
        {
            var gameObject = new GameObject();
            var expectedTransform = new GameObject().transform;
            expectedTransform.localPosition = Vector3.forward;

            var controllerComponent = new GameObject().AddComponent<TestControllerComponent>();

            controllerComponent.PawnInitialTransform = expectedTransform;

            controllerComponent.CreatePawnOfType(gameObject);

            Assert.AreEqual(expectedTransform.localPosition, controllerComponent.PawnInstance.transform.localPosition);
        }

        [Test]
        public void CreatePawnOfType_SetsInitialActionStateToSpawning()
        {
            var gameObject = new GameObject();
            gameObject.AddComponent<MockActionStateMachineComponent>();

            var controllerComponent = new GameObject().AddComponent<TestControllerComponent>();

            controllerComponent.CreatePawnOfType(gameObject);

            var stateMachine = controllerComponent.PawnInstance.GetComponent<MockActionStateMachineComponent>();

            Assert.AreEqual(EActionStateMachineTrack.Locomotion, stateMachine.RequestedTrack);
            Assert.AreEqual(EActionStateId.Spawning, stateMachine.RequestedId);
            Assert.AreSame(controllerComponent.PawnInstance, stateMachine.RequestedInfo.Owner);
        }

        [Test]
        public void SetPawn_OverridesCurrentPawn()
        {
            var gameObject = new GameObject();

            var controllerComponent = new GameObject().AddComponent<TestControllerComponent>();

            controllerComponent.CreatePawnOfType(gameObject);
            var otherPawn = new GameObject();

            controllerComponent.SetPawn(otherPawn);

            Assert.AreEqual(otherPawn, controllerComponent.PawnInstance);
        }

        [Test]
        public void SetPawn_OriginalPawnHasAControllerOfNull()
        {
            var gameObject = new GameObject();

            gameObject.AddComponent<MockActionStateMachineComponent>();
            gameObject.AddComponent<MockInputBinderComponent>();
            gameObject.AddComponent<CharacterComponent>();

            var controllerComponent = new GameObject().AddComponent<TestControllerComponent>();

            controllerComponent.CreatePawnOfType(gameObject);

            var originalPawn = controllerComponent.PawnInstance;

            controllerComponent.SetPawn(gameObject);

            Assert.IsNull(originalPawn.GetComponent<CharacterComponent>().ActiveController);
        }

        [Test]
        public void SetPawn_UpdatesController()
        {
            var gameObject = new GameObject();

            gameObject.AddComponent<MockActionStateMachineComponent>();
            gameObject.AddComponent<MockInputBinderComponent>();
            var character = gameObject.AddComponent<CharacterComponent>();

            var controllerComponent = new GameObject().AddComponent<TestControllerComponent>();

            controllerComponent.SetPawn(gameObject);

            Assert.AreEqual(character.ActiveController, controllerComponent);
        }

        [Test]
        public void SetPawn_SetsTransformCorrectly()
        {
            var gameObject = new GameObject();

            var controllerComponent = new GameObject().AddComponent<TestControllerComponent>();

            controllerComponent.SetPawn(gameObject);

            Assert.AreEqual(controllerComponent.PawnInstance.transform, controllerComponent.gameObject.transform.parent);
            Assert.AreEqual(new Vector3(0, 0, 0), controllerComponent.gameObject.transform.localPosition);
        }
    }
}

#endif
