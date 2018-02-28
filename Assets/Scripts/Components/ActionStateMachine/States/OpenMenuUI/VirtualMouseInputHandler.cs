// Copyright (C) Threetee Gang All Rights Reserved 

using Assets.Scripts.Input;
using Assets.Scripts.UI.VirtualMouse;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Components.ActionStateMachine.States.OpenMenuUI
{
    public class VirtualMouseInputHandler 
        : InputHandler
    {
        private readonly IVirtualMouseInterface _pointerInterface;

        public VirtualMouseInputHandler(IVirtualMouseInterface inPointerInterface)
        {
            _pointerInterface = inPointerInterface;

            AnalogResponses.Add(EInputKey.HorizontalAnalog, OnHorizontalAnalog);
            AnalogResponses.Add(EInputKey.VerticalAnalog, OnVerticalAnalog);

            ButtonResponses.Add(EInputKey.VirtualLeftClick, OnVirtualLeftMouseClick);
            ButtonResponses.Add(EInputKey.VirtualRightClick, OnVirtualRightMouseClick);
            ButtonResponses.Add(EInputKey.VirtualMiddleClick, OnVirtualMiddleMouseClick);
        }

        private EInputHandlerResult OnHorizontalAnalog(float inValue)
        {
            if (_pointerInterface != null)
            {
                _pointerInterface.ApplyHorizontalMovement(inValue);
                return EInputHandlerResult.Handled;
            }

            return EInputHandlerResult.Unhandled;
        }

        private EInputHandlerResult OnVerticalAnalog(float inValue)
        {
            if (_pointerInterface != null)
            {
                _pointerInterface.ApplyVerticalMovement(inValue);
                return EInputHandlerResult.Handled;
            }

            return EInputHandlerResult.Unhandled;
        }

        private EInputHandlerResult OnVirtualLeftMouseClick(bool isPressed)
        {
            return OnMouseClick(PointerEventData.InputButton.Left, isPressed);
        }

        private EInputHandlerResult OnVirtualRightMouseClick(bool isPressed)
        {
            return OnMouseClick(PointerEventData.InputButton.Right, isPressed);
        }

        private EInputHandlerResult OnVirtualMiddleMouseClick(bool isPressed)
        {
            return OnMouseClick(PointerEventData.InputButton.Middle, isPressed);
        }

        private EInputHandlerResult OnMouseClick(PointerEventData.InputButton inButton, bool isPressed)
        {
            if (_pointerInterface != null)
            {
                _pointerInterface.SetButtonState(inButton, isPressed);
                return EInputHandlerResult.Handled;
            }

            return EInputHandlerResult.Unhandled;
        }
    }
}
