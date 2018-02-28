// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Messaging;

namespace Assets.Scripts.Components.Interaction
{
    public class ActiveInteractableUpdatedMessage
        : UnityMessagePayload
    {
        public readonly IInteractableInterface UpdatedInteractable;

        public ActiveInteractableUpdatedMessage(IInteractableInterface inUpdatedInteractable)
        {
            UpdatedInteractable = inUpdatedInteractable;
        }
    }

    public class ActiveInteractableUpdatedUIMessage
        : UnityMessagePayload
    {
        public readonly IInteractableInterface UpdatedInteractable;

        public ActiveInteractableUpdatedUIMessage(IInteractableInterface inUpdatedInteractable)
        {
            UpdatedInteractable = inUpdatedInteractable;
        }
    }

    public class InteractionStatusUpdatedMessage
        : UnityMessagePayload
    {
        public readonly bool Interactable;

        public InteractionStatusUpdatedMessage(bool inInteractable)
        {
            Interactable = inInteractable;
        }
    }

    public class InteractionStatusUpdatedUIMessage
        : UnityMessagePayload
    {
        public readonly bool Interactable;

        public InteractionStatusUpdatedUIMessage(bool inInteractable)
        {
            Interactable = inInteractable;
        }
    }
}
