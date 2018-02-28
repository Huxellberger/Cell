// Copyright (C) Threetee Gang All Rights Reserved 

using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.VirtualMouse
{
    public interface IVirtualMouseInterface
    {
        Vector3 GetVirtualMousePosition();
        void SetVirtualMousePosition(Vector3 inPosition);
        void SetMouseVisibile(bool isVisible);
        bool IsMouseVisible();
        VirtualMouseData GetVirtualMouseData();

        void ApplyHorizontalMovement(float inValue);
        void ApplyVerticalMovement(float inValue);

        void SetButtonState(PointerEventData.InputButton inButton, bool isPressed);
    }
}
