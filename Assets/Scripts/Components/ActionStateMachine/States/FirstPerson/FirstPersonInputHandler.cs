// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Character;
using Assets.Scripts.Input;

namespace Assets.Scripts.Components.ActionStateMachine.States.FirstPerson
{
    public class FirstPersonInputHandler 
        : InputHandler
    {
        private readonly IPlayerCameraInterface _playerCamera;

        public FirstPersonInputHandler(IPlayerCameraInterface inPlayerCamera)
            : base()
        {
            _playerCamera = inPlayerCamera;

            AnalogResponses.Add(EInputKey.HorizontalAnalog, OnCameraHorizontalInput);
            AnalogResponses.Add(EInputKey.VerticalAnalog, OnCameraVerticalInput);
            AnalogResponses.Add(EInputKey.CameraHorizontal, OnCameraHorizontalInput);
            AnalogResponses.Add(EInputKey.CameraVertical, OnCameraVerticalInput);

            ButtonResponses.Add(EInputKey.CameraToggle, OnCameraToggle);
        }

        private EInputHandlerResult OnCameraHorizontalInput(float inInput)
        {
            if (_playerCamera != null)
            {
                _playerCamera.RotateHorizontal(inInput);
                return EInputHandlerResult.Handled;
            }

            return EInputHandlerResult.Unhandled;
        }

        private EInputHandlerResult OnCameraVerticalInput(float inInput)
        {
            if (_playerCamera != null)
            {
                _playerCamera.RotateVertical(inInput * -1);
                return EInputHandlerResult.Handled;
            }

            return EInputHandlerResult.Unhandled;
        }

        private EInputHandlerResult OnCameraToggle(bool isEnabled)
        {
            if (_playerCamera != null)
            {
                if (isEnabled)
                {
                    _playerCamera.SetCameraMode(EPlayerCameraMode.ThirdPerson);
                }

                return EInputHandlerResult.Handled;
            }

            return EInputHandlerResult.Unhandled;
        }
    }
}
