// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.AI.Companion;
using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.ActionStateMachine.States.CinematicCamera;
using Assets.Scripts.Components.ActionStateMachine.States.Dead;
using Assets.Scripts.Components.Gadget;
using Assets.Scripts.Components.Health;
using Assets.Scripts.Components.Interaction;
using Assets.Scripts.Components.Stamina;
using Assets.Scripts.Instance;
using Assets.Scripts.Localisation;
using Assets.Scripts.Messaging;
using Assets.Scripts.Services.Persistence;
using Assets.Scripts.Services.Time;
using Assets.Scripts.UI.HUD;
using UnityEngine;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(IUnityMessageEventInterface), typeof(IActionStateMachineInterface))]
    public class PlayerUIControllerComponent 
        : MonoBehaviour
    {
        public LocalisedTextRef DeathMessage;
        public LocalisedTextRef SavedMessage;

        public AudioClip SavedNoise;

        private IActionStateMachineInterface _actionStateMachine;

        private UnityMessageEventDispatcher _localDispatcher;
        private UnityMessageEventDispatcher _uiDispatcher;

        private UnityMessageEventHandle<HealthChangedMessage> _healthChangedEventHandle;
        private UnityMessageEventHandle<MaxHealthChangedMessage> _maxHealthChangedEventHandle;

        private UnityMessageEventHandle<StaminaChangedMessage> _staminaChangedEventHandle;
        private UnityMessageEventHandle<MaxStaminaChangedMessage> _maxStaminaChangedEventHandle;

        private UnityMessageEventHandle<EnterDeadActionStateMessage> _enterDeadActionStateHandle;
        private UnityMessageEventHandle<LeftDeadActionStateMessage> _leftDeadActionStateHandle;

        private UnityMessageEventHandle<PauseStatusChangedMessage> _pauseStatusChangedHandle;

        private UnityMessageEventHandle<ActiveInteractableUpdatedMessage> _activeInteractableUpdatedHandle;
        private UnityMessageEventHandle<InteractionStatusUpdatedMessage> _interactionStatusUpdatedHandle;

        private UnityMessageEventHandle<EnterCinematicCameraActionStateMessage> _enterCinematicCameraHandle;
        private UnityMessageEventHandle<ExitCinematicCameraActionStateMessage> _exitCinematicCameraHandle;

        private UnityMessageEventHandle<CompanionSlotsUpdatedMessage> _companionSlotsUpdatedHandle;

        private UnityMessageEventHandle<SaveGameTriggerActivatedMessage> _saveTriggerActivatedHandle;

        private UnityMessageEventHandle<GadgetUpdatedMessage> _gadgetUpdatedHandle;

        protected void Start ()
        {
            DeathMessage = new LocalisedTextRef(new LocalisationKey("UIMessages", "DeathMessage"));
            SavedMessage = new LocalisedTextRef(new LocalisationKey("UIMessages", "SaveMessage"));
            _actionStateMachine = gameObject.GetComponent<IActionStateMachineInterface>();

            _localDispatcher = gameObject.GetComponent<IUnityMessageEventInterface>().GetUnityMessageEventDispatcher();
            _uiDispatcher = GameInstance.CurrentInstance.GetUIMessageDispatcher();

            RegisterForMessages();
        }

        protected void OnDestroy()
        {
            UnregisterForMessages();

            _uiDispatcher = null;
            _localDispatcher = null;
        }

        private void RegisterForMessages()
        {
            _healthChangedEventHandle = _localDispatcher.RegisterForMessageEvent<HealthChangedMessage>(OnHealthChangedMessage);
            _maxHealthChangedEventHandle = _localDispatcher.RegisterForMessageEvent<MaxHealthChangedMessage>(OnMaxHealthChangedMessage);

            _staminaChangedEventHandle = _localDispatcher.RegisterForMessageEvent<StaminaChangedMessage>(OnStaminaChangedMessage);
            _maxStaminaChangedEventHandle = _localDispatcher.RegisterForMessageEvent<MaxStaminaChangedMessage>(OnMaxStaminaChangedMessage);

            _enterDeadActionStateHandle = _localDispatcher.RegisterForMessageEvent<EnterDeadActionStateMessage>(OnEnterDeadActionStateMessage);
            _leftDeadActionStateHandle =_localDispatcher.RegisterForMessageEvent<LeftDeadActionStateMessage>(OnLeftDeadActionStateMessage);

            _activeInteractableUpdatedHandle = _localDispatcher.RegisterForMessageEvent<ActiveInteractableUpdatedMessage>(OnActiveInteractableUpdated);
            _interactionStatusUpdatedHandle = _localDispatcher.RegisterForMessageEvent<InteractionStatusUpdatedMessage>(OnInteractionStatusUpdated);

            _enterCinematicCameraHandle =_localDispatcher.RegisterForMessageEvent<EnterCinematicCameraActionStateMessage>(OnEnterCinematicCameraActionStateMessage);
            _exitCinematicCameraHandle = _localDispatcher.RegisterForMessageEvent<ExitCinematicCameraActionStateMessage>(OnExitCinematicCameraActionStateMessage);

            _pauseStatusChangedHandle = _localDispatcher.RegisterForMessageEvent<PauseStatusChangedMessage>(OnPauseStatusChanged);

            _companionSlotsUpdatedHandle =  _localDispatcher.RegisterForMessageEvent<CompanionSlotsUpdatedMessage>(OnCompanionSlotsUpdated);

            _saveTriggerActivatedHandle = _localDispatcher.RegisterForMessageEvent<SaveGameTriggerActivatedMessage>(OnSaveTriggerActivated);

            _gadgetUpdatedHandle = _localDispatcher.RegisterForMessageEvent<GadgetUpdatedMessage>(OnGadgetUpdated);
        }

        private void UnregisterForMessages()
        {
            _localDispatcher.UnregisterForMessageEvent(_gadgetUpdatedHandle);

            _localDispatcher.UnregisterForMessageEvent(_saveTriggerActivatedHandle);

            _localDispatcher.UnregisterForMessageEvent(_companionSlotsUpdatedHandle);

            _localDispatcher.UnregisterForMessageEvent(_pauseStatusChangedHandle);

            _localDispatcher.UnregisterForMessageEvent(_exitCinematicCameraHandle);
            _localDispatcher.UnregisterForMessageEvent(_enterCinematicCameraHandle);

            _localDispatcher.UnregisterForMessageEvent(_interactionStatusUpdatedHandle);
            _localDispatcher.UnregisterForMessageEvent(_activeInteractableUpdatedHandle);

            _localDispatcher.UnregisterForMessageEvent(_leftDeadActionStateHandle);
            _localDispatcher.UnregisterForMessageEvent(_enterDeadActionStateHandle);

            _localDispatcher.UnregisterForMessageEvent(_maxStaminaChangedEventHandle);
            _localDispatcher.UnregisterForMessageEvent(_staminaChangedEventHandle);

            _localDispatcher.UnregisterForMessageEvent(_maxHealthChangedEventHandle);
            _localDispatcher.UnregisterForMessageEvent(_healthChangedEventHandle);
        }

        private void OnHealthChangedMessage(HealthChangedMessage inMessage)
        {
            _uiDispatcher.InvokeMessageEvent(new HealthChangedUIMessage(inMessage.NewHealth));
        }

        private void OnMaxHealthChangedMessage(MaxHealthChangedMessage inMessage)
        {
            _uiDispatcher.InvokeMessageEvent(new MaxHealthChangedUIMessage(inMessage.MaxHealth));
        }

        private void OnStaminaChangedMessage(StaminaChangedMessage inMessage)
        {
            _uiDispatcher.InvokeMessageEvent(new StaminaChangedUIMessage(inMessage.NewStamina));
        }

        private void OnMaxStaminaChangedMessage(MaxStaminaChangedMessage inMessage)
        {
            _uiDispatcher.InvokeMessageEvent(new MaxStaminaChangedUIMessage(inMessage.NewMaxStamina));
        }

        private void OnEnterDeadActionStateMessage(EnterDeadActionStateMessage inMessage)
        {
            _uiDispatcher.InvokeMessageEvent(new TextNotificationSentUIMessage(DeathMessage.ToString()));
        }

        private void OnLeftDeadActionStateMessage(LeftDeadActionStateMessage inMessage)
        {
            _uiDispatcher.InvokeMessageEvent(new TextNotificationClearedUIMessage());
        }

        private void OnActiveInteractableUpdated(ActiveInteractableUpdatedMessage inMessage)
        {
            _uiDispatcher.InvokeMessageEvent(new ActiveInteractableUpdatedUIMessage(inMessage.UpdatedInteractable));
        }

        private void OnInteractionStatusUpdated(InteractionStatusUpdatedMessage inMessage)
        {
            _uiDispatcher.InvokeMessageEvent(new InteractionStatusUpdatedUIMessage(inMessage.Interactable));
        }

        private void OnEnterCinematicCameraActionStateMessage(EnterCinematicCameraActionStateMessage inMessage)
        {
            _uiDispatcher.InvokeMessageEvent(new UpdateUIEnabledMessage(false));
        }

        private void OnExitCinematicCameraActionStateMessage(ExitCinematicCameraActionStateMessage inMessage)
        {
            _uiDispatcher.InvokeMessageEvent(new UpdateUIEnabledMessage(true));
        }

        private void OnPauseStatusChanged(PauseStatusChangedMessage inMessage)
        {
            if (inMessage.NewPauseStatus == EPauseStatus.Paused)
            {
                _actionStateMachine.RequestActionState(EActionStateMachineTrack.UI, EActionStateId.OpenMenuUI, new ActionStateInfo(gameObject));
            }
            else
            {
                _actionStateMachine.RequestActionState(EActionStateMachineTrack.UI, EActionStateId.Null, new ActionStateInfo(gameObject));
            }
        }

        private void OnCompanionSlotsUpdated(CompanionSlotsUpdatedMessage inMessage)
        {
            _uiDispatcher.InvokeMessageEvent(new CompanionSlotsUpdatedUIMessage(inMessage.Updates));
        }

        private void OnSaveTriggerActivated(SaveGameTriggerActivatedMessage inMessage)
        {
            _uiDispatcher.InvokeMessageEvent(new DisplayToastUIMessage(SavedMessage.ToString(), SavedNoise));
        }

        private void OnGadgetUpdated(GadgetUpdatedMessage inMessage)
        {
            Sprite dispatchedSprite = null;
            if (inMessage.NewGadget != null)
            {
                dispatchedSprite = inMessage.NewGadget.GetGadgetIcon();
            }

            _uiDispatcher.InvokeMessageEvent(new GadgetUpdatedUIMessage(dispatchedSprite, inMessage.SlotCount));
        }
    }
}
