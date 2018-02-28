// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.Messaging;
using UnityEngine;

namespace Assets.Scripts.Components.Interaction 
{
    public class InteractionComponent 
        : MonoBehaviour 
        , IInteractionInterface
    {
        private readonly List<IInteractableInterface> _possibleActiveInteractables = new List<IInteractableInterface>(4);
        private bool _interactionPossible = false;

        private IInteractableInterface ActiveInteractable { get; set; }

        protected void Start()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(gameObject, new InteractionStatusUpdatedMessage(_interactionPossible));
        }

        protected void Update()
        {
            UpdateInteractionStatus();
        }

        private void UpdateInteractionStatus()
        {
            var mostDesirableInteractable = GetMostDesirableInteractable();
            var canStillInteract = mostDesirableInteractable != null;

            if (mostDesirableInteractable != ActiveInteractable)
            {
                ActiveInteractable = mostDesirableInteractable;
                UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(gameObject, new ActiveInteractableUpdatedMessage(ActiveInteractable));
            }
            if (canStillInteract != _interactionPossible)
            {
                _interactionPossible = canStillInteract;
                UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(gameObject, new InteractionStatusUpdatedMessage(_interactionPossible));
            }
        }

        private IInteractableInterface GetMostDesirableInteractable()
        {
            foreach (var possibleActiveInteractable in _possibleActiveInteractables)
            {
                if (possibleActiveInteractable.CanInteract(gameObject))
                {
                    return possibleActiveInteractable;
                }
            }

            return null;
        }

        // IInteractionInterface
        public void AddActiveInteractable(IInteractableInterface inInteractableInterface)
        {
            _possibleActiveInteractables.Add(inInteractableInterface);
        }

        public void RemoveActiveInteractable(IInteractableInterface inInteractableInterface)
        {
            _possibleActiveInteractables.Remove(inInteractableInterface);
        }

        public IInteractableInterface GetActiveInteractable()
        {
            UpdateInteractionStatus();
            return ActiveInteractable;
        }

        public bool TryInteract()
        {
            UpdateInteractionStatus();

            if (_interactionPossible)
            {
                ActiveInteractable.OnInteract(gameObject);
                return true;
            }

            return false;
        }
        // ~IInteractionInterface
    }
}