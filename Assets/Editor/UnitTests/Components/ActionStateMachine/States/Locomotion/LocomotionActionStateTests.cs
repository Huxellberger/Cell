// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Editor.UnitTests.Messaging;
using Assets.Editor.UnitTests.Services.Time;
using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.ActionStateMachine.States.Locomotion;
using Assets.Scripts.Components.Character;
using Assets.Scripts.Components.Controller;
using Assets.Scripts.Input.Handlers;
using Assets.Scripts.Messaging;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Time;
using Assets.Scripts.Test.Components.ActionStateMachine;
using Assets.Scripts.Test.Input;
using Assets.Scripts.Test.Messaging;
using Assets.Scripts.Test.Services;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine.States.Locomotion
{
    [TestFixture]
    public class LocomotionActionStateTestFixture
    {
        private MockInputBinderComponent _inputBinder;
        private CharacterComponent _characterComponent;

        [SetUp]
        public void BeforeTest()
        {
            _inputBinder = new GameObject().AddComponent<MockInputBinderComponent>();
            _inputBinder.gameObject.AddComponent<MockActionStateMachineComponent>();
            _characterComponent = _inputBinder.gameObject.AddComponent<CharacterComponent>();
            _characterComponent.ActiveController = _inputBinder.gameObject.AddComponent<ControllerComponent>();
            _characterComponent.gameObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();

            new GameObject().AddComponent<TestGameServiceProvider>().TestAwake();
            GameServiceProvider.CurrentInstance.AddService<ITimeServiceInterface>(new MockTimeService());
        }

        [TearDown]
        public void AfterTest()
        {
            GameServiceProvider.ClearGameServiceProvider();

            _characterComponent = null;
            _inputBinder = null;
        }

        [Test]
        public void Created_HasLocomotionActionStateId()
        {
            var locomotion = new LocomotionActionState(new ActionStateInfo(_inputBinder.gameObject));

            Assert.AreEqual(EActionStateId.Locomotion, locomotion.ActionStateId);
        }

        [Test]
        public void Start_RegistersLocomotionInputHandler()
        {
            var locomotion = new LocomotionActionState(new ActionStateInfo(_inputBinder.gameObject));

            locomotion.Start();

            Assert.IsTrue(_inputBinder.IsHandlerOfTypeRegistered<LocomotionInputHandler>());
        }

        [Test]
        public void Start_RegistersInteractionInputHandler()
        {
            var locomotion = new LocomotionActionState(new ActionStateInfo(_inputBinder.gameObject));

            locomotion.Start();

            Assert.IsTrue(_inputBinder.IsHandlerOfTypeRegistered<InteractionInputHandler>());
        }

        [Test]
        public void Start_RegistersPauseInputHandler()
        {
            var locomotion = new LocomotionActionState(new ActionStateInfo(_inputBinder.gameObject));

            locomotion.Start();

            Assert.IsTrue(_inputBinder.IsHandlerOfTypeRegistered<PauseInputHandler>());
        }

        [Test]
        public void Start_SendsEnterLocomotionStateMessage()
        {
            var locomotion = new LocomotionActionState(new ActionStateInfo(_inputBinder.gameObject));

            var messageSpy = new UnityTestMessageHandleResponseObject<EnterLocomotionStateMessage>();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<EnterLocomotionStateMessage>(
                    _characterComponent.gameObject, messageSpy.OnResponse);

            locomotion.Start();

            Assert.IsTrue(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_characterComponent.gameObject, handle);
        }

        [Test]
        public void End_UnregistersLocomotionInputHandler()
        {
            var locomotion = new LocomotionActionState(new ActionStateInfo(_inputBinder.gameObject));

            locomotion.Start();

            locomotion.End();

            Assert.IsTrue(_inputBinder.IsHandlerOfTypeUnregistered<LocomotionInputHandler>());
        }

        [Test]
        public void End_UnregistersInteractionInputHandler()
        {
            var locomotion = new LocomotionActionState(new ActionStateInfo(_inputBinder.gameObject));

            locomotion.Start();

            locomotion.End();

            Assert.IsTrue(_inputBinder.IsHandlerOfTypeUnregistered<InteractionInputHandler>());
        }

        [Test]
        public void End_UnregistersPauseInputHandler()
        {
            var locomotion = new LocomotionActionState(new ActionStateInfo(_inputBinder.gameObject));

            locomotion.Start();

            locomotion.End();

            Assert.IsTrue(_inputBinder.IsHandlerOfTypeUnregistered<PauseInputHandler>());
        }

        [Test]
        public void Start_SendsLeaveLocomotionStateMessage()
        {
            var locomotion = new LocomotionActionState(new ActionStateInfo(_inputBinder.gameObject));

            var messageSpy = new UnityTestMessageHandleResponseObject<LeaveLocomotionStateMessage>();

            locomotion.Start();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<LeaveLocomotionStateMessage>(
                    _characterComponent.gameObject, messageSpy.OnResponse);

            locomotion.End();

            Assert.IsTrue(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_characterComponent.gameObject, handle);
        }
    }
}
