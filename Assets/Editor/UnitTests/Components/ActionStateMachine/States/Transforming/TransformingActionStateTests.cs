// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.ActionStateMachine.States.Transforming;
using Assets.Scripts.Instance;
using Assets.Scripts.Test.Components.ActionStateMachine;
using Assets.Scripts.Test.Components.Character;
using Assets.Scripts.Test.Components.Controller;
using Assets.Scripts.Test.Components.Equipment.Holdables;
using Assets.Scripts.Test.Input;
using Assets.Scripts.Test.Instance;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine.States.Transforming
{
    [TestFixture]
    public class TransformingActionStateTestFixture
    {
        private TestCharacterComponent _character;
        private MockHeldItemComponent _heldItem;

        [SetUp]
        public void BeforeTest()
        {
            var instance = new GameObject().AddComponent<MockInputComponent>();
            instance.gameObject.AddComponent<TestGameInstance>().TestAwake();

            var prefabObject = new GameObject();
            prefabObject.AddComponent<MockActionStateMachineComponent>();
            prefabObject.AddComponent<MockInputBinderComponent>();
            _character = prefabObject.AddComponent<TestCharacterComponent>();
            _character.TestStart();

            _heldItem = _character.gameObject.AddComponent<MockHeldItemComponent>();
        }

        [TearDown]
        public void AfterTest()
        {
            _heldItem = null;
            _character = null;
            
            GameInstance.ClearGameInstance();
        }

        [Test]
        public void Created_HasTransformingActionStateId()
        {
            var actionState = new TransformingActionState(new TransformingActionStateInfo());

            Assert.AreEqual(EActionStateId.Transforming, actionState.ActionStateId);
        }

        [Test]
        public void Start_CreatesAndAddsNewPawnToController()
        {
            var characterObject = new GameObject();

            characterObject.AddComponent<MockActionStateMachineComponent>();
            characterObject.AddComponent<MockInputBinderComponent>();

            var characterComponent = characterObject.AddComponent<TestCharacterComponent>();
            characterComponent.TestStart();
            characterComponent.ActiveController = new GameObject().AddComponent<TestControllerComponent>();
            characterComponent.ActiveController.SetPawn(characterObject);

            var actionState = new TransformingActionState(new TransformingActionStateInfo(characterObject, _character.gameObject));

            actionState.Start();

            Assert.AreNotSame(characterObject, characterComponent.ActiveController.PawnInstance);
        }

        [Test]
        public void Start_DropsHeldItem()
        {
            _character.ActiveController = new GameObject().AddComponent<TestControllerComponent>();

            var actionState = new TransformingActionState(new TransformingActionStateInfo(_character.gameObject, _character.gameObject));

            actionState.Start();

            Assert.IsTrue(_heldItem.DropHoldableCalled);
        }

        [Test]
        public void End_NewPawnRequestsLocomotion()
        {
            var characterObject = new GameObject();

            characterObject.AddComponent<MockActionStateMachineComponent>();
            characterObject.AddComponent<MockInputBinderComponent>();

            var characterComponent = characterObject.AddComponent<TestCharacterComponent>();
            characterComponent.TestStart();
            characterComponent.ActiveController = new GameObject().AddComponent<TestControllerComponent>();
            characterComponent.ActiveController.SetPawn(characterObject);

            var actionState = new TransformingActionState(new TransformingActionStateInfo(characterObject, _character.gameObject));

            actionState.Start();
            actionState.End();

            var actionStateMachine = characterComponent.ActiveController.PawnInstance
                .GetComponent<MockActionStateMachineComponent>();

            Assert.AreEqual(EActionStateMachineTrack.Locomotion, actionStateMachine.RequestedTrack);
            Assert.AreEqual(EActionStateId.Locomotion, actionStateMachine.RequestedId);
            Assert.AreSame(characterComponent.ActiveController.PawnInstance, actionStateMachine.RequestedInfo.Owner);
        }
    }
}
