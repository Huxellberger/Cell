// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Interaction;
using Assets.Scripts.Input;

namespace Assets.Scripts.Components.ActionStateMachine.States.Locomotion
{
    public class InteractionInputHandler : InputHandler
    {
        private IInteractionInterface InteractionInterface { get; set; }

        public InteractionInputHandler(IInteractionInterface inInteractionInterface)
            : base()
        {
            InteractionInterface = inInteractionInterface;

            ButtonResponses.Add(EInputKey.Interact, OnInteractButton);
        }

        private EInputHandlerResult OnInteractButton(bool inPressed)
        {
            if (InteractionInterface != null)
            {
                if (inPressed)
                {
                    InteractionInterface.TryInteract();

                }

                return EInputHandlerResult.Handled;
            }

            return EInputHandlerResult.Unhandled;
        }
    }
}
