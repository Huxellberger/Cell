// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Interaction;
using Assets.Scripts.Localisation;
using Assets.Scripts.Messaging;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.HUD
{
    [RequireComponent(typeof(Image))]
    public class InteractionPromptHUDComponent 
        : UIComponent
    {
        private Image _backingImage;
        public LocalisableUIText InteractionVerbText;
        public Text InteractableNameText;

        private UnityMessageEventHandle<ActiveInteractableUpdatedUIMessage> _activeInteractableUpdatedHandle;
        private UnityMessageEventHandle<InteractionStatusUpdatedUIMessage> _interactionStatusUpdatedHandle;

        protected override void OnStart()
        {
            _backingImage = gameObject.GetComponent<Image>();
            _backingImage.enabled = false;

            InteractionVerbText.enabled = false;
            InteractableNameText.enabled = false;

            RegisterForMessages();
        }

        protected override void OnEnd()
        {
            UnregisterForMessages();

            InteractableNameText = null;
            InteractionVerbText = null;
        }

        private void RegisterForMessages()
        {
            _activeInteractableUpdatedHandle =
                Dispatcher.RegisterForMessageEvent<ActiveInteractableUpdatedUIMessage>(OnActiveInteractableUpdated);

            _interactionStatusUpdatedHandle =
                Dispatcher.RegisterForMessageEvent<InteractionStatusUpdatedUIMessage>(OnInteractionStatusUpdated);
        }

        private void UnregisterForMessages()
        {
            Dispatcher.UnregisterForMessageEvent(_interactionStatusUpdatedHandle);
            Dispatcher.UnregisterForMessageEvent(_activeInteractableUpdatedHandle);
        }

        private void OnActiveInteractableUpdated(ActiveInteractableUpdatedUIMessage inMessage)
        {
            if (inMessage.UpdatedInteractable == null)
            {
                InteractableNameText.text = "";
            }
            else
            {
                InteractableNameText.text = inMessage.UpdatedInteractable.GetInteractableName();
            }
        }

        private void OnInteractionStatusUpdated(InteractionStatusUpdatedUIMessage inMessage)
        {
            if (inMessage.Interactable)
            {
                InteractionVerbText.enabled = true;
                InteractableNameText.enabled = true;
                _backingImage.enabled = true;
            }
            else
            {
                InteractionVerbText.enabled = false;
                InteractableNameText.enabled = false;
                _backingImage.enabled = false;
            }
        }
    }
}
