// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Interaction;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Interaction
{
    public class MockInteractableComponent 
        : MonoBehaviour
        , IInteractableInterface
    {
        public bool CanInteractCalled = false;
        public bool CanInteractResult { get; set; }

        public bool OnInteractCalled = false;
        public GameObject OnInteractGameObject { get; private set; }
        public string GetInteractableNameResult { get; set; }
        

        public bool CanInteract(GameObject inGameObject)
        {
            CanInteractCalled = true;
            return CanInteractResult;
        }

        public void OnInteract(GameObject inGameObject)
        {
            OnInteractCalled = true;

            OnInteractGameObject = inGameObject;
        }

        public string GetInteractableName()
        {
            return GetInteractableNameResult;
        }
    }
}

#endif // UNITY_EDITOR
