// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Interaction;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Interaction
{
    public class MockInteractionComponent 
        : MonoBehaviour 
        , IInteractionInterface
    {
        public IInteractableInterface AddActiveInteractableResult { get; private set; }
        public IInteractableInterface RemoveActiveInteractableResult { get; private set; }
        public IInteractableInterface GetActiveInteractableResult { get; set; }
        public bool TryInteractResult { get; set; }

        public bool TryInteractCalled = false;

        public void AddActiveInteractable(IInteractableInterface inInteractableInterface)
        {
            AddActiveInteractableResult = inInteractableInterface;
        }

        public void RemoveActiveInteractable(IInteractableInterface inInteractableInterface)
        {
            RemoveActiveInteractableResult = inInteractableInterface;
        }

        public IInteractableInterface GetActiveInteractable()
        {
            return GetActiveInteractableResult;
        }

        public bool TryInteract()
        {
            TryInteractCalled = true;

            return TryInteractResult;
        }
    }
}

#endif // UNITY_EDITOR
