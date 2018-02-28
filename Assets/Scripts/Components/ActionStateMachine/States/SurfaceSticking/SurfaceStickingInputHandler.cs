// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Movement;
using Assets.Scripts.Input;

namespace Assets.Scripts.Components.ActionStateMachine.States.SurfaceSticking
{
    public class SurfaceStickingInputHandler 
        : InputHandler
    {
        private readonly IMovementInterface _movementInterface;

        public SurfaceStickingInputHandler(IMovementInterface inMovementInterface)
            : base()
        {
            _movementInterface = inMovementInterface;

            AnalogResponses.Add(EInputKey.HorizontalAnalog, OnHorizontalInput);
            AnalogResponses.Add(EInputKey.VerticalAnalog, OnVerticalInput);
        }

        private EInputHandlerResult OnHorizontalInput(float inInput)
        {
            if (_movementInterface != null)
            {
                _movementInterface.ApplySidewaysMotion(inInput);
                return EInputHandlerResult.Handled;
            }

            return EInputHandlerResult.Unhandled;
        }

        private EInputHandlerResult OnVerticalInput(float inInput)
        {
            if (_movementInterface != null)
            {
                _movementInterface.ApplyForwardMotion(inInput);
                return EInputHandlerResult.Handled;
            }

            return EInputHandlerResult.Unhandled;
        }
    }
}
