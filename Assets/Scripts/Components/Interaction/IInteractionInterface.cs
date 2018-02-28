// Copyright (C) Threetee Gang All Rights Reserved

namespace Assets.Scripts.Components.Interaction
{
    public interface IInteractionInterface
    {
        void AddActiveInteractable(IInteractableInterface inInteractableInterface);
        void RemoveActiveInteractable(IInteractableInterface interactableInterface);
        IInteractableInterface GetActiveInteractable();

        bool TryInteract();
    }
}