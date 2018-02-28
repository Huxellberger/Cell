// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Interaction;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Interaction
{
    public class TestInteractableComponent 
        : InteractableComponent
    {
        public bool CanInteractImplCalled = false;
        public bool CanInteractImplResult { get; set; }
        public GameObject CanInteractImplGameObject { get; private set; }

        public bool OnInteractImplCalled = false;
        public GameObject OnInteractImplGameObject { get; private set; }

        protected override bool CanInteractImpl(GameObject inGameObject)
        {
            CanInteractImplCalled = true;
            CanInteractImplGameObject = inGameObject;

            return CanInteractImplResult;
        }

        protected override void OnInteractImpl(GameObject inGameObject)
        {
            OnInteractImplCalled = true;
            OnInteractImplGameObject = inGameObject;
        }
    }
}

#endif // UNITY_EDITOR
