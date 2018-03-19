// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.Input;

namespace Assets.Scripts.Components.ActionStateMachine.States.CinematicCamera
{
    public class BlockingInputHandler 
        : InputHandler
    {
        public BlockingInputHandler()
            : base()
        {
            ButtonResponses.Add(EInputKey.SprintButton, OnDudButtonPressed);
            ButtonResponses.Add(EInputKey.CameraZoomReset, OnDudButtonPressed);
            ButtonResponses.Add(EInputKey.Interact, OnDudButtonPressed);
            ButtonResponses.Add(EInputKey.PositiveAnimalCry, OnDudButtonPressed);
            ButtonResponses.Add(EInputKey.NegativeAnimalCry, OnDudButtonPressed);
            ButtonResponses.Add(EInputKey.PrimaryHeldAction, OnDudButtonPressed);
            ButtonResponses.Add(EInputKey.SecondaryHeldAction, OnDudButtonPressed);
            ButtonResponses.Add(EInputKey.DropHeldItem, OnDudButtonPressed);
            ButtonResponses.Add(EInputKey.PrimaryPower, OnDudButtonPressed);
            ButtonResponses.Add(EInputKey.SecondaryPower, OnDudButtonPressed);

            AnalogResponses.Add(EInputKey.CameraHorizontal, OnDudAnalogInput);
            AnalogResponses.Add(EInputKey.CameraZoom, OnDudAnalogInput);
            AnalogResponses.Add(EInputKey.HorizontalAnalog, OnDudAnalogInput);
            AnalogResponses.Add(EInputKey.VerticalAnalog, OnDudAnalogInput);
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
