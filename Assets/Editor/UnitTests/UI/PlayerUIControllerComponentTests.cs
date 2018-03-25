// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Editor.UnitTests.Messaging;
using Assets.Scripts.AI.Companion;
using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.ActionStateMachine.States.CinematicCamera;
using Assets.Scripts.Components.ActionStateMachine.States.Dead;
using Assets.Scripts.Components.Health;
using Assets.Scripts.Components.Interaction;
using Assets.Scripts.Components.Stamina;
using Assets.Scripts.Instance;
using Assets.Scripts.Localisation;
using Assets.Scripts.Messaging;
using Assets.Scripts.Services.Persistence;
using Assets.Scripts.Services.Time;
using Assets.Scripts.Test.Components.ActionStateMachine;
using Assets.Scripts.Test.Components.Interaction;
using Assets.Scripts.Test.Input;
using Assets.Scripts.Test.Instance;
using Assets.Scripts.Test.Localisation;
using Assets.Scripts.Test.Messaging;
using Assets.Scripts.Test.UI;
using Assets.Scripts.UI;
using Assets.Scripts.UI.HUD;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.UI
{
    [TestFixture]
    public class PlayerUIControllerComponentTestFixture
    {
        private TestPlayerUIControllerComponent _playerUi;
        private MockActionStateMachineComponent _actionStateMachineComponent;

        [SetUp]
        public void BeforeTest()
        {
            var locInstance = new MockLocalisationInterface
            {
                GetTextForLocalisationKeyResult = new LocalisedText(new LocalisedTextEntries())
            };
            LocalisationManager.CurrentLocalisationInterface = locInstance;

            var instance = new GameObject().AddComponent<MockInputComponent>();
            instance.gameObject.AddComponent<TestGameInstance>().TestAwake();

            var player = new GameObject();
            player.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();

            _actionStateMachineComponent = player.AddComponent<MockActionStateMachineComponent>();

            _playerUi = player.gameObject.AddComponent<TestPlayerUIControllerComponent>();
            _playerUi.TestStart();
        }

        [TearDown]
        public void AfterTest()
        {
            _playerUi.TestDestroy();
            _playerUi = null;
            
            _actionStateMachineComponent = null;

            GameInstance.ClearGameInstance();

            LocalisationManager.CurrentLocalisationInterface = null;
        }

        [Test]
        public void ReceiveStaminaChangedMessage_ForwardsToInstanceDispatcher()
        {
            var eventSpy = new UnityTestMessageHandleResponseObject<StaminaChangedUIMessage>();

            var dispatcher = GameInstance.CurrentInstance.GetUIMessageDispatcher();
            var handle = dispatcher
                .RegisterForMessageEvent<StaminaChangedUIMessage>(eventSpy.OnResponse);

            const int newStamina = 10;

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_playerUi.gameObject, new StaminaChangedMessage(newStamina));

            Assert.AreEqual(newStamina, eventSpy.MessagePayload.NewStamina);

            dispatcher.UnregisterForMessageEvent(handle);
        }

        [Test]
        public void ReceiveMaxStaminaChangedMessage_ForwardsToInstanceDispatcher()
        {
            var eventSpy = new UnityTestMessageHandleResponseObject<MaxStaminaChangedUIMessage>();
            var dispatcher = GameInstance.CurrentInstance.GetUIMessageDispatcher();

            var handle = dispatcher
                .RegisterForMessageEvent<MaxStaminaChangedUIMessage>(eventSpy.OnResponse);

            const int newStamina = 10;

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_playerUi.gameObject, new MaxStaminaChangedMessage(newStamina));

            Assert.AreEqual(newStamina, eventSpy.MessagePayload.NewMaxStamina);

            dispatcher.UnregisterForMessageEvent(handle);
        }

        [Test]
        public void ReceiveHealthChangedMessage_ForwardsToInstanceDispatcher()
        {
            var eventSpy = new UnityTestMessageHandleResponseObject<HealthChangedUIMessage>();

            var dispatcher = GameInstance.CurrentInstance.GetUIMessageDispatcher();
            var handle = dispatcher
                .RegisterForMessageEvent<HealthChangedUIMessage>(eventSpy.OnResponse);

            const int newHealth = 10;

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_playerUi.gameObject, new HealthChangedMessage(11, newHealth));

            Assert.AreEqual(newHealth, eventSpy.MessagePayload.NewHealth);

            dispatcher.UnregisterForMessageEvent(handle);
        }

        [Test]
        public void ReceiveMaxHealthChangedMessage_ForwardsToInstanceDispatcher()
        {
            var eventSpy = new UnityTestMessageHandleResponseObject<MaxHealthChangedUIMessage>();

            var dispatcher = GameInstance.CurrentInstance.GetUIMessageDispatcher();
            var handle = dispatcher
                .RegisterForMessageEvent<MaxHealthChangedUIMessage>(eventSpy.OnResponse);

            const int newHealth = 10;

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_playerUi.gameObject, new MaxHealthChangedMessage(newHealth));

            Assert.AreEqual(newHealth, eventSpy.MessagePayload.MaxHealth);

            dispatcher.UnregisterForMessageEvent(handle);
        }

        [Test]
        public void ReceiveEnterDeadActionStateMessage_ForwardsTextNotificationToUIDispatcher()
        {
            var eventSpy = new UnityTestMessageHandleResponseObject<TextNotificationSentUIMessage>();

            var dispatcher = GameInstance.CurrentInstance.GetUIMessageDispatcher();
            var handle = dispatcher
                .RegisterForMessageEvent<TextNotificationSentUIMessage>(eventSpy.OnResponse);

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_playerUi.gameObject, new EnterDeadActionStateMessage());

            Assert.IsTrue(eventSpy.ActionCalled);
            Assert.IsTrue(eventSpy.MessagePayload.Message.Equals(_playerUi.DeathMessage.ToString()));

            dispatcher.UnregisterForMessageEvent(handle);
        }

        [Test]
        public void ReceiveLeftDeadActionStateMessage_ForwardsTextNotificationClearedToUIDispatcher()
        {
            var eventSpy = new UnityTestMessageHandleResponseObject<TextNotificationClearedUIMessage>();

            var dispatcher = GameInstance.CurrentInstance.GetUIMessageDispatcher();
            var handle = dispatcher
                .RegisterForMessageEvent<TextNotificationClearedUIMessage>(eventSpy.OnResponse);

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_playerUi.gameObject, new LeftDeadActionStateMessage());

            Assert.IsTrue(eventSpy.ActionCalled);

            dispatcher.UnregisterForMessageEvent(handle);
        }

        [Test]
        public void ReceiveActiveInteractableUpdatedMessage_ForwardsTextNotificationClearedToUIDispatcher()
        {
            var eventSpy = new UnityTestMessageHandleResponseObject<ActiveInteractableUpdatedUIMessage>();

            var dispatcher = GameInstance.CurrentInstance.GetUIMessageDispatcher();
            var handle = dispatcher
                .RegisterForMessageEvent<ActiveInteractableUpdatedUIMessage>(eventSpy.OnResponse);

            var expectedInteractable = new GameObject().AddComponent<MockInteractableComponent>();

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_playerUi.gameObject, new ActiveInteractableUpdatedMessage(expectedInteractable));

            Assert.IsTrue(eventSpy.ActionCalled);
            Assert.AreSame(expectedInteractable, eventSpy.MessagePayload.UpdatedInteractable);

            dispatcher.UnregisterForMessageEvent(handle);
        }

        [Test]
        public void ReceiveInteractionStatusUpdatedMessage_ForwardsTextNotificationClearedToUIDispatcher()
        {
            var eventSpy = new UnityTestMessageHandleResponseObject<InteractionStatusUpdatedUIMessage>();

            var dispatcher = GameInstance.CurrentInstance.GetUIMessageDispatcher();
            var handle = dispatcher
                .RegisterForMessageEvent<InteractionStatusUpdatedUIMessage>(eventSpy.OnResponse);

            const bool expectedStatus = true;

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_playerUi.gameObject, new InteractionStatusUpdatedMessage(expectedStatus));

            Assert.IsTrue(eventSpy.ActionCalled);
            Assert.AreEqual(expectedStatus, eventSpy.MessagePayload.Interactable);

            dispatcher.UnregisterForMessageEvent(handle);
        }

        [Test]
        public void ReceiveEnterCinematicCameraActionStateMessage_ForwardsUpdateUIEnabledMessage()
        {
            var eventSpy = new UnityTestMessageHandleResponseObject<UpdateUIEnabledMessage>();

            var dispatcher = GameInstance.CurrentInstance.GetUIMessageDispatcher();
            var handle = dispatcher
                .RegisterForMessageEvent<UpdateUIEnabledMessage>(eventSpy.OnResponse);

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_playerUi.gameObject, new EnterCinematicCameraActionStateMessage());

            Assert.IsTrue(eventSpy.ActionCalled);
            Assert.IsFalse(eventSpy.MessagePayload.IsEnabled);

            dispatcher.UnregisterForMessageEvent(handle);
        }

        [Test]
        public void ReceiveExitCinematicCameraActionStateMessage_ForwardsUpdateUIEnabledMessage()
        {
            var eventSpy = new UnityTestMessageHandleResponseObject<UpdateUIEnabledMessage>();

            var dispatcher = GameInstance.CurrentInstance.GetUIMessageDispatcher();
            var handle = dispatcher
                .RegisterForMessageEvent<UpdateUIEnabledMessage>(eventSpy.OnResponse);

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_playerUi.gameObject, new ExitCinematicCameraActionStateMessage());

            Assert.IsTrue(eventSpy.ActionCalled);
            Assert.IsTrue(eventSpy.MessagePayload.IsEnabled);

            dispatcher.UnregisterForMessageEvent(handle);
        }

        [Test]
        public void ReceivePauseStatusChangedMessage_Paused_RequestOpenUIActionState()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_playerUi.gameObject, new PauseStatusChangedMessage(EPauseStatus.Paused));

            Assert.AreEqual(EActionStateMachineTrack.UI, _actionStateMachineComponent.RequestedTrack);
            Assert.AreEqual(EActionStateId.OpenMenuUI, _actionStateMachineComponent.RequestedId);
            Assert.AreEqual(_playerUi.gameObject, _actionStateMachineComponent.RequestedInfo.Owner);
        }

        [Test]
        public void ReceivePauseStatusChangedMessage_Unpaused_RequestNullActionState()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_playerUi.gameObject, new PauseStatusChangedMessage(EPauseStatus.Unpaused));

            Assert.AreEqual(EActionStateMachineTrack.UI, _actionStateMachineComponent.RequestedTrack);
            Assert.AreEqual(EActionStateId.Null, _actionStateMachineComponent.RequestedId);
            Assert.AreEqual(_playerUi.gameObject, _actionStateMachineComponent.RequestedInfo.Owner);
        }

        [Test]
        public void ReceiveCompanionSlotsUpdatedMessage_ForwardsUpdateUIEnabledMessage()
        {
            var eventSpy = new UnityTestMessageHandleResponseObject<CompanionSlotsUpdatedUIMessage>();

            var dispatcher = GameInstance.CurrentInstance.GetUIMessageDispatcher();
            var handle = dispatcher
                .RegisterForMessageEvent<CompanionSlotsUpdatedUIMessage>(eventSpy.OnResponse);

            var payload = new Dictionary<ECompanionSlot, PriorCompanionSlotState>();

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_playerUi.gameObject, new CompanionSlotsUpdatedMessage(payload));

            Assert.IsTrue(eventSpy.ActionCalled);
            Assert.AreSame(payload, eventSpy.MessagePayload.Updates);

            dispatcher.UnregisterForMessageEvent(handle);
        }

        [Test]
        public void ReceiveSaveMessage_ForwardsAppropriateToastNotificationToHUD()
        {
            var eventSpy = new UnityTestMessageHandleResponseObject<DisplayToastUIMessage>();

            _playerUi.SavedNoise = new AudioClip();

            var dispatcher = GameInstance.CurrentInstance.GetUIMessageDispatcher();
            var handle = dispatcher
                .RegisterForMessageEvent<DisplayToastUIMessage>(eventSpy.OnResponse);

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_playerUi.gameObject, new SaveGameTriggerActivatedMessage());

            Assert.IsTrue(eventSpy.ActionCalled);
            Assert.IsTrue(eventSpy.MessagePayload.ToastText.Equals(_playerUi.DeathMessage.ToString()));
            Assert.AreSame(_playerUi.SavedNoise, eventSpy.MessagePayload.ToastAudio);

            dispatcher.UnregisterForMessageEvent(handle);
        }
    }
}
