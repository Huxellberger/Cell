// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Editor.UnitTests.Messaging;
using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.ActionStateMachine.States.Dead;
using Assets.Scripts.Components.Stamina;
using Assets.Scripts.Messaging;
using Assets.Scripts.Mode;
using Assets.Scripts.Test.Components.Equipment.Holdables;
using Assets.Scripts.Test.Components.Stamina;
using Assets.Scripts.Test.Input;
using Assets.Scripts.Test.Messaging;
using Assets.Scripts.Test.Mode;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine.States.Dead
{
    public class DeadActionStateTestFixture
    {
        private DeadActionState _deadActionState;
        private MockInputBinderComponent _playerBinder;
        private MockStaminaComponent _stamina;
        private MockHeldItemComponent _heldItem;
        private DeadActionStateParams _params;

        [SetUp]
        public void BeforeTest()
        {
            var gameMode = new GameObject();
            gameMode.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();

            GameModeComponent.RegisteredGameMode = gameMode.AddComponent<TestGameModeComponent>();

            var player = new GameObject();
            player.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();
            _playerBinder = player.AddComponent<MockInputBinderComponent>();
            _stamina = player.AddComponent<MockStaminaComponent>();
            _heldItem = player.AddComponent<MockHeldItemComponent>();
            _params = new DeadActionStateParams{CanRespawn = true};
        }

        [TearDown]
        public void AfterTest()
        {
            _deadActionState = null;

            _heldItem = null;
            _stamina = null;
            _playerBinder = null;

            GameModeComponent.RegisteredGameMode = null;
        }

        [Test]
        public void EnterState_IdIsDeadActionState()
        {
            _deadActionState = new DeadActionState(new ActionStateInfo(_playerBinder.gameObject), _params);

            Assert.AreEqual(EActionStateId.Dead, _deadActionState.ActionStateId);
        }

        [Test]
        public void Start_SendsEnterEventToOwner()
        {
            _deadActionState = new DeadActionState(new ActionStateInfo(_playerBinder.gameObject), _params);

            var eventSpy = new UnityTestMessageHandleResponseObject<EnterDeadActionStateMessage>();
            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<EnterDeadActionStateMessage>(_playerBinder.gameObject, eventSpy.OnResponse);

            _deadActionState.Start();

            Assert.IsTrue(eventSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_playerBinder.gameObject, handle);
        }

        [Test]
        public void Start_DisablesStaminaRegenForDeadReason()
        {
            _deadActionState = new DeadActionState(new ActionStateInfo(_playerBinder.gameObject), _params);

            _deadActionState.Start();

            Assert.IsFalse(_stamina.SetStaminaChangeEnabledResult);
            Assert.AreEqual(ELockStaminaReason.Dead, _stamina.SetStaminaChangeEnabledReason);
        }

        [Test]
        public void Start_DropsHoldable()
        {
            _deadActionState = new DeadActionState(new ActionStateInfo(_playerBinder.gameObject), _params);

            _deadActionState.Start();

            Assert.IsTrue(_heldItem.DropHoldableCalled);
        }

        [Test]
        public void End_SendsLeavingEventToOwner()
        {
            _deadActionState = new DeadActionState(new ActionStateInfo(_playerBinder.gameObject), _params);

            var eventSpy = new UnityTestMessageHandleResponseObject<LeftDeadActionStateMessage>();
            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<LeftDeadActionStateMessage>(_playerBinder.gameObject, eventSpy.OnResponse);

            _deadActionState.End();

            Assert.IsTrue(eventSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_playerBinder.gameObject, handle);
        }

        [Test]
        public void Start_EnablesStaminaRegenForDeadReason()
        {
            _deadActionState = new DeadActionState(new ActionStateInfo(_playerBinder.gameObject), _params);

            _deadActionState.Start();
            _deadActionState.End();

            Assert.IsTrue(_stamina.SetStaminaChangeEnabledResult);
            Assert.AreEqual(ELockStaminaReason.Dead, _stamina.SetStaminaChangeEnabledReason);
        }

        [Test]
        public void Update_ConditionsNotComplete_DoesNotFireRequestRespawnEventToGameMode()
        {
            _deadActionState = new DeadActionState(new ActionStateInfo(_playerBinder.gameObject), _params);

            var eventSpy = new UnityTestMessageHandleResponseObject<RequestRespawnMessage>();
            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<RequestRespawnMessage>(GameModeComponent.RegisteredGameMode.gameObject, eventSpy.OnResponse);

            _deadActionState.Start();
            _deadActionState.Update(1.0f);

            Assert.IsFalse(eventSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(GameModeComponent.RegisteredGameMode.gameObject, handle);
        }

        [Test]
        public void Update_ConditionsComplete_FiresRequestRespawnEventToGameMode()
        {
            _deadActionState = new DeadActionState(new ActionStateInfo(_playerBinder.gameObject), _params);

            var eventSpy = new UnityTestMessageHandleResponseObject<RequestRespawnMessage>();
            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<RequestRespawnMessage>(GameModeComponent.RegisteredGameMode.gameObject, eventSpy.OnResponse);

            _deadActionState.Start();
            _playerBinder.RegisteredHandlers[0].HandleButtonInput(_deadActionState.ValidProgressingInputs[0], true);
            _deadActionState.Update(1.0f);

            Assert.IsTrue(eventSpy.ActionCalled);
            Assert.AreSame(_playerBinder.gameObject, eventSpy.MessagePayload.RequestingPlayer);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(GameModeComponent.RegisteredGameMode.gameObject, handle);
        }

        [Test]
        public void Update_ConditionsCompleteCannotRespawn_DoesNotFireRequestRespawnEventToGameMode()
        {
            _params.CanRespawn = false;
            _deadActionState = new DeadActionState(new ActionStateInfo(_playerBinder.gameObject), _params);

            var eventSpy = new UnityTestMessageHandleResponseObject<RequestRespawnMessage>();
            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<RequestRespawnMessage>(GameModeComponent.RegisteredGameMode.gameObject, eventSpy.OnResponse);

            _deadActionState.Start();
            _playerBinder.RegisteredHandlers[0].HandleButtonInput(_deadActionState.ValidProgressingInputs[0], true);
            _deadActionState.Update(1.0f);

            Assert.IsFalse(eventSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(GameModeComponent.RegisteredGameMode.gameObject, handle);
        }
    }
}
