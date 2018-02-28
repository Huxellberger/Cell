// Copyright (C) Threetee Gang All Rights Reserved

using System;
using Assets.Scripts.UnityLayer.Input;
using UnityEngine;

namespace Assets.Scripts.Input
{
    public class InputComponent 
        : MonoBehaviour
        , IInputInterface
    {
        private IInputMappingProviderInterface _inputMappingProviderInterface = null;
        private IUnityInputInterface _unityInputInterface = null;
        private const float AxisPrecision = 0.01f;

        protected void Awake()
        {
        }
	    
        // When input flags are updated
        protected void Update ()
        {
            if (_inputMappingProviderInterface == null || _unityInputInterface == null)
            {
                return;
            }

            foreach (var input in _inputMappingProviderInterface.GetRawInputs())
            {
                var inputPressed = false;
                var inputValue = 0.0f;
                var inputCoordinate = new Vector3();

                // Get state of button
                switch (input.InputType)
                {
                    case EInputType.Analog:
                        inputValue = _unityInputInterface.GetAxis(input.InputName);
                        inputPressed = Math.Abs(inputValue) < AxisPrecision;
                        break;
                    case EInputType.Button:
                        inputPressed = _unityInputInterface.GetButton(input.InputName); // Make everything a button
                        break;
                    case EInputType.Mouse:
                        inputCoordinate = _unityInputInterface.GetMousePosition();
                        break;
                    default:
                        break;
                }

                var actualInput = _inputMappingProviderInterface.GetTranslatedInput(input);

                if (actualInput != null)
                {
                    // respond to state
                    switch (actualInput.InputType)
                    {
                        case EInputType.Analog:
                            OnAnalogInput(actualInput, inputValue);
                            break;
                        case EInputType.Button:
                            OnButtonInput(actualInput, inputPressed);
                            break;
                        case EInputType.Mouse:
                            OnMouseInput(actualInput, inputCoordinate);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void OnAnalogInput(TranslatedInput translatedInput, float newInputValue)
        {
            translatedInput.AxisValue = newInputValue;
            if (OnAnalogInputEvent != null)
            {
                OnAnalogInputEvent(translatedInput.InputKey, translatedInput.AxisValue);
            }
        }

        private void OnButtonInput(TranslatedInput translatedInput, bool newPressed)
        {
            if (translatedInput.Pressed != newPressed)
            {
                translatedInput.Pressed = newPressed;
                if (OnButtonInputEvent != null)
                {
                    OnButtonInputEvent(translatedInput.InputKey, translatedInput.Pressed);
                }
            }
        }
        private void OnMouseInput(TranslatedInput translatedInput, Vector3 newSpace)
        {
            if (!Mathf.Approximately(translatedInput.Coordinate.sqrMagnitude, newSpace.sqrMagnitude))
            {
                translatedInput.Coordinate = newSpace;
                if (OnMouseInputEvent != null)
                {
                    OnMouseInputEvent(translatedInput.InputKey, translatedInput.Coordinate);
                }
            }
        }

        // IInputInterface
        public event OnButtonInputDelegate OnButtonInputEvent;
        public event OnAnalogInputDelegate OnAnalogInputEvent;
        public event OnMouseInputDelegate OnMouseInputEvent;

        public void SetInputMappingProvider(IInputMappingProviderInterface inInputMappingProviderInterface)
        {
            _inputMappingProviderInterface = inInputMappingProviderInterface;
        }

        public void SetUnityInputInterface(IUnityInputInterface inUnityInputInterface)
        {
            _unityInputInterface = inUnityInputInterface;
        }
        // ~IInputInterface
    }
}
