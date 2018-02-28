// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Components.Interaction
{
    public interface IInteractableInterface
    {
        bool CanInteract(GameObject inGameObject);
        void OnInteract(GameObject inGameObject);
        string GetInteractableName();
    }
}
