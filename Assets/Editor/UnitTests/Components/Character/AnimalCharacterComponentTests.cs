// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine.States.Locomotion;
using Assets.Scripts.Components.Character;
using Assets.Scripts.Instance;
using Assets.Scripts.Messaging;
using Assets.Scripts.Test.Components.ActionStateMachine;
using Assets.Scripts.Test.Components.Character;
using Assets.Scripts.Test.Components.Species;
using Assets.Scripts.Test.Input;
using Assets.Scripts.Test.Instance;
using Assets.Scripts.Test.Messaging;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Character
{
    [TestFixture]
    public class AnimalCharacterComponentTestFixture
    {
        private MockInputBinderComponent _inputBinder;
        private TestAnimalCharacterComponent _animal;

        [SetUp]
        public void BeforeTest()
        {
            new GameObject().AddComponent<MockInputComponent>().gameObject.AddComponent<TestGameInstance>().TestAwake();

            var characterObject = new GameObject();
            _inputBinder = characterObject.AddComponent<MockInputBinderComponent>();
            characterObject.AddComponent<MockActionStateMachineComponent>();
            characterObject.AddComponent<MockSpeciesComponent>();
            characterObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();

            _animal = characterObject.AddComponent<TestAnimalCharacterComponent>();
            _animal.TestStart();
        }

        [TearDown]
        public void AfterTest()
        {
            _animal.TestDestroy();
            _animal = null;

            _inputBinder = null;

            GameInstance.ClearGameInstance();
        }

        [Test]
        public void NoLocomotionMessage_DoesNotRegisterAnimalInputHandler()
        {
            Assert.IsFalse(_inputBinder.IsHandlerOfTypeRegistered<AnimalInputHandler>());
        }

        [Test]
        public void EnterLocomotionMessage_RegistersAnimalInputHandler()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_animal.gameObject, new EnterLocomotionStateMessage());

            Assert.IsTrue(_inputBinder.IsHandlerOfTypeRegistered<AnimalInputHandler>());
        }

        [Test]
        public void LeaveLocomotionMessage_UnregistersAnimalInputHandler()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_animal.gameObject, new EnterLocomotionStateMessage());
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_animal.gameObject, new LeaveLocomotionStateMessage());

            Assert.IsTrue(_inputBinder.IsHandlerOfTypeUnregistered<AnimalInputHandler>());
        }
    }
}
