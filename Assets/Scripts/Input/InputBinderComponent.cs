// Copyright (C) Threetee Gang All Rights Reserved

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Input
{
    public class InputBinderComponent 
        : MonoBehaviour
        , IInputBinderInterface
    {
        // Behaves like a stack. E.g last registered input is considered first
        private IList<InputHandler> _registeredInputHandlers;
        private IInputInterface _inputInterface;

        protected void Awake ()
        {
		    _registeredInputHandlers = new List<InputHandler>();
            _inputInterface = null;
        }

        // IInputBinderInterface
        public void SetInputInterface(IInputInterface inInputInterface)
        {
            UnregisterForInputEvents();
            _inputInterface = inInputInterface;
            RegisterForInputEvents();
        }

        public void RegisterInputHandler(InputHandler inInputHandler)
        {
            if (_registeredInputHandlers.Contains(inInputHandler) || inInputHandler == null)
            {
                throw new InvalidInputHandlerException(inInputHandler);
            }

            _registeredInputHandlers.Add(inInputHandler);
        }

        public void UnregisterInputHandler(InputHandler inInputHandler)
        {
            if (!_registeredInputHandlers.Remove(inInputHandler))
            {
                throw new InvalidInputHandlerException(inInputHandler);
            }
        }
        // ~IInputBinderInterface

        private void RegisterForInputEvents()
        {
            if (_inputInterface == null)
            {
                throw new ArgumentNullException("Input interface was null on registration!");
            }

            _inputInterface.OnAnalogInputEvent += OnAnalogInput;
            _inputInterface.OnButtonInputEvent += OnButtonInput;
            _inputInterface.OnMouseInputEvent += OnMouseInput;
        }

        private void UnregisterForInputEvents()
        {
            if (_inputInterface != null)
            {
                _inputInterface.OnAnalogInputEvent -= OnAnalogInput;
                _inputInterface.OnButtonInputEvent -= OnButtonInput;
                _inputInterface.OnMouseInputEvent -= OnMouseInput;
            }
        }

        private void OnAnalogInput(EInputKey inInputKey, float analogValue)
        {
            for(var currentHandlerIndex = _registeredInputHandlers.Count - 1; currentHandlerIndex >= 0; currentHandlerIndex-- )
            {
                if (_registeredInputHandlers[currentHandlerIndex].HandleAnalogInput(inInputKey, analogValue) == EInputHandlerResult.Handled)
                {
                    return;
                }
            }
        }

        private void OnButtonInput(EInputKey inInputKey, bool pressed)
        {
            for (var currentHandlerIndex = _registeredInputHandlers.Count - 1; currentHandlerIndex >= 0; currentHandlerIndex--)
            {
                if (_registeredInputHandlers[currentHandlerIndex].HandleButtonInput(inInputKey, pressed) == EInputHandlerResult.Handled)
                {
                    return;
                }
            }
        }

        private void OnMouseInput(EInputKey inInputKey, Vector3 mousePosition)
        {
            for (var currentHandlerIndex = _registeredInputHandlers.Count - 1; currentHandlerIndex >= 0; currentHandlerIndex--)
            {
                if (_registeredInputHandlers[currentHandlerIndex].HandleMouseInput(inInputKey, mousePosition) == EInputHandlerResult.Handled)
                {
                    return;
                }
            }
        }
    }
}
