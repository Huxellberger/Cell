// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Character;
using Assets.Scripts.Components.Equipment.Holdables;
using Assets.Scripts.Components.Movement;
using Assets.Scripts.Input;

namespace Assets.Scripts.Components.ActionStateMachine.States.Locomotion
{
    public class LocomotionInputHandler 
        : InputHandler
    {
        private IMovementInterface MovementInterface { get; set; }
        private IPlayerCameraInterface CameraInterface { get; set; }
        private IHeldItemInterface HeldItemInterface { get; set; }

        public LocomotionInputHandler(IMovementInterface inMovementInterface, IPlayerCameraInterface inCameraInterface, IHeldItemInterface inHeldItemInterface)
            : base()
        {
            MovementInterface = inMovementInterface;
            CameraInterface = inCameraInterface;
            HeldItemInterface = inHeldItemInterface;

            AnalogResponses.Add(EInputKey.HorizontalAnalog, OnHorizontalInput);
            AnalogResponses.Add(EInputKey.VerticalAnalog, OnVerticalInput);
            AnalogResponses.Add(EInputKey.CameraHorizontal, OnCameraHorizontalInput);
            AnalogResponses.Add(EInputKey.CameraVertical, OnCameraVerticalInput);
            AnalogResponses.Add(EInputKey.CameraZoom, OnCameraZoomInput);

            ButtonResponses.Add(EInputKey.CameraZoomReset, OnCameraZoomReset);
            ButtonResponses.Add(EInputKey.CameraToggle, OnCameraToggle);
            ButtonResponses.Add(EInputKey.SprintButton, OnSprintButton);
            ButtonResponses.Add(EInputKey.PrimaryHeldAction, OnPrimaryHeldAction);
            ButtonResponses.Add(EInputKey.SecondaryHeldAction, OnSecondaryHeldAction);
            ButtonResponses.Add(EInputKey.DropHeldItem, OnDropHeldItem);
        }

        private EInputHandlerResult OnHorizontalInput(float inInput)
        {
            if (MovementInterface != null)
            {
                MovementInterface.ApplySidewaysMotion(inInput);
                return EInputHandlerResult.Handled;
            }

            return EInputHandlerResult.Unhandled;
        }

        private EInputHandlerResult OnVerticalInput(float inInput)
        {
            if (MovementInterface != null)
            {
                MovementInterface.ApplyForwardMotion(inInput);
                return EInputHandlerResult.Handled;
            }

            return EInputHandlerResult.Unhandled;
        }

        private EInputHandlerResult OnCameraHorizontalInput(float inInput)
        {
            if (CameraInterface != null)
            {
                CameraInterface.RotateHorizontal(inInput);
                return EInputHandlerResult.Handled;
            }

            return EInputHandlerResult.Unhandled;
        }

        private EInputHandlerResult OnCameraVerticalInput(float inInput)
        {
            if (CameraInterface != null)
            {
                CameraInterface.RotateVertical(inInput * -1);
                return EInputHandlerResult.Handled;
            }

            return EInputHandlerResult.Unhandled;
        }

        private EInputHandlerResult OnCameraZoomInput(float inInput)
        {
            if (CameraInterface != null)
            {
                CameraInterface.Zoom(inInput);
                return EInputHandlerResult.Handled;
            }

            return EInputHandlerResult.Unhandled;
        }

        private EInputHandlerResult OnCameraZoomReset(bool isEnabled)
        {
            if (CameraInterface != null)
            {
                if (isEnabled)
                {
                    CameraInterface.ResetZoom();
                }
                
                return EInputHandlerResult.Handled;
            }

            return EInputHandlerResult.Unhandled;
        }

        private EInputHandlerResult OnCameraToggle(bool isEnabled)
        {
            if (CameraInterface != null)
            {
                if (isEnabled)
                {
                    CameraInterface.SetCameraMode(EPlayerCameraMode.FirstPerson);
                }

                return EInputHandlerResult.Handled;
            }

            return EInputHandlerResult.Unhandled;
        }

        private EInputHandlerResult OnSprintButton(bool isEnabled)
        {
            if (MovementInterface != null)
            {
                MovementInterface.SetSprintEnabled(isEnabled);

                return EInputHandlerResult.Handled;
            }

            return EInputHandlerResult.Unhandled;
        }

        private EInputHandlerResult OnPrimaryHeldAction(bool isEnabled)
        {
            if (HeldItemInterface != null)
            {
                if (isEnabled)
                {
                    HeldItemInterface.UseCurrentHoldable(EHoldableAction.Primary);
                }

                return EInputHandlerResult.Handled;
            }

            return EInputHandlerResult.Unhandled;
        }

        private EInputHandlerResult OnSecondaryHeldAction(bool isEnabled)
        {
            if (HeldItemInterface != null)
            {
                if (isEnabled)
                {
                    HeldItemInterface.UseCurrentHoldable(EHoldableAction.Secondary);
                }

                return EInputHandlerResult.Handled;
            }

            return EInputHandlerResult.Unhandled;
        }

        private EInputHandlerResult OnDropHeldItem(bool isEnabled)
        {
            if (HeldItemInterface != null)
            {
                if (isEnabled)
                {
                    HeldItemInterface.DropHoldable();
                }

                return EInputHandlerResult.Handled;
            }

            return EInputHandlerResult.Unhandled;
        }
    }
}
