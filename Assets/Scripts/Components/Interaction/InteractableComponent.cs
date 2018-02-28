// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Localisation;
using UnityEngine;

namespace Assets.Scripts.Components.Interaction
{
    public abstract class InteractableComponent 
        : MonoBehaviour
        , IInteractableInterface
    {
        public LocalisationKey LocalisedInteractableKey;

        private LocalisedTextRef _localisedTextRef;

        // IInteractableInterface
        public bool CanInteract(GameObject inGameObject)
        {
            return CanInteractImpl(inGameObject);
        }

        public void OnInteract(GameObject inGameObject)
        {
            if (CanInteract(inGameObject))
            {
                OnInteractImpl(inGameObject);
            }
            else
            {
                Debug.LogError("Tried to interact when interaction was invalid!");
            }
        }

        public string GetInteractableName()
        {
            if (_localisedTextRef == null)
            {
                _localisedTextRef = new LocalisedTextRef(LocalisedInteractableKey);
            }

            return _localisedTextRef.ToString();
        }
        // ~IInteractableInterface

        protected abstract bool CanInteractImpl(GameObject inGameObject);
        protected abstract void OnInteractImpl(GameObject inGameObject);
    }
}