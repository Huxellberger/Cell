// Copyright (C) Threetee Gang All Rights Reserved 

using Assets.Scripts.Input;

namespace Assets.Scripts.Components.ActionStateMachine.States.OpenMenuUI
{
    public class InGameMenuInputHandler 
        : InputHandler
    {
        public InGameMenuInputHandler()
            : base()
        {
            ButtonResponses.Add(EInputKey.SprintButton, OnDudButtonPressed);
            ButtonResponses.Add(EInputKey.JumpButton, OnDudButtonPressed);
            ButtonResponses.Add(EInputKey.CameraZoomReset, OnDudButtonPressed);
            ButtonResponses.Add(EInputKey.Interact, OnDudButtonPressed);
            ButtonResponses.Add(EInputKey.PositiveAnimalCry, OnDudButtonPressed);
            ButtonResponses.Add(EInputKey.NegativeAnimalCry, OnDudButtonPressed);
            ButtonResponses.Add(EInputKey.PrimaryHeldAction, OnDudButtonPressed);
            ButtonResponses.Add(EInputKey.SecondaryHeldAction, OnDudButtonPressed);
            ButtonResponses.Add(EInputKey.DropHeldItem, OnDudButtonPressed);
            ButtonResponses.Add(EInputKey.CameraToggle, OnDudButtonPressed);

            AnalogResponses.Add(EInputKey.CameraHorizontal, OnDudAnalogInput);
            AnalogResponses.Add(EInputKey.CameraVertical, OnDudAnalogInput);
            AnalogResponses.Add(EInputKey.CameraZoom, OnDudAnalogInput);
        }

        private static EInputHandlerResult OnDudAnalogInput(float inInput)
        {
            return EInputHandlerResult.Handled;
        }

        private static EInputHandlerResult OnDudButtonPressed(bool isPressed)
        {
            return EInputHandlerResult.Handled;
        }
    }
}
