// Copyright (C) Threetee Gang All Rights Reserved 

using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Components.Interaction
{
    public class InteractionZone 
        : MonoBehaviour
    {
        public GameObject AttachedInteractable;

        private IInteractableInterface _interactable;
        private List<GameObject> _currentInteractionObjects;

        protected void Start()
        {
            _currentInteractionObjects = new List<GameObject>();

            if (AttachedInteractable != null)
            {
                _interactable = AttachedInteractable.GetComponent<IInteractableInterface>();
            }

            if (_interactable == null)
            {
                Debug.LogError("Failed to retrieve attached interactable!");
            }
        }

        // E.g when a HoldableItem is picked up we no longer want it to register as an interactable.
        protected void OnDisable()
        {
            foreach (var currentInteractionObject in _currentInteractionObjects)
            {
                OnGameObjectStopsColliding(currentInteractionObject);
            }

            _currentInteractionObjects.Clear();
        }

        private void OnTriggerEnter2D(Collider2D inCollider)
        {
		    OnGameObjectCollides(inCollider.gameObject);
        }

        private void OnTriggerExit2D(Collider2D inCollider)
        {
            OnGameObjectStopsColliding(inCollider.gameObject);
        }

        protected void OnGameObjectCollides(GameObject inCollidingObject)
        {
            var interactionInterface = inCollidingObject.GetComponent<IInteractionInterface>();
            if (interactionInterface != null)
            {
                interactionInterface.AddActiveInteractable(_interactable);
                _currentInteractionObjects.Add(inCollidingObject);
            }
        }

        protected void OnGameObjectStopsColliding(GameObject inCollidingObject)
        {
            if (inCollidingObject != null)
            {
                var interactionInterface = inCollidingObject.GetComponent<IInteractionInterface>();
                if (interactionInterface != null)
                {
                    interactionInterface.RemoveActiveInteractable(_interactable);
                }
            }
        }
    }
}
