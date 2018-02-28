// Copyright (C) Threetee Gang All Rights Reserved 

#if UNITY_EDITOR

using Assets.Scripts.UI.VirtualMouse;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Test.UI.VirtualMouse
{
    public class MockVirtualMouseComponent
        : MonoBehaviour
        , IVirtualMouseInterface
    {
        public Vector3 GetVirtualMousePositionResult { get; set; }
        public Vector3 ? SetVirtualMousePositionResult { get; private set; }
        public bool ? SetMouseVisibleResult { get; private set; }
        public bool IsMouseVisibleResult { get; set; }
        public VirtualMouseData GetVirtualMouseDataResult { get; set; }


        public float? ApplyHorizontalMovementResult { get; private set; }
        public float? ApplyVerticalMovementResult { get; private set; }

        public PointerEventData.InputButton? SetButtonStateButtonResult { get; private set; }
        public bool? SetButtonStatePressedResult { get; private set; }

        public Vector3 GetVirtualMousePosition()
        {
            return GetVirtualMousePositionResult;
        }

        public void SetVirtualMousePosition(Vector3 inPosition)
        {
            SetVirtualMousePositionResult = inPosition;
        }

        public void SetMouseVisibile(bool isVisible)
        {
            SetMouseVisibleResult = isVisible;
        }

        public bool IsMouseVisible()
        {
            return IsMouseVisibleResult;
        }

        public VirtualMouseData GetVirtualMouseData()
        {
            return GetVirtualMouseDataResult;
        }

        public void ApplyHorizontalMovement(float inValue)
        {
            ApplyHorizontalMovementResult = inValue;
        }

        public void ApplyVerticalMovement(float inValue)
        {
            ApplyVerticalMovementResult = inValue;
        }

        public void SetButtonState(PointerEventData.InputButton inButton, bool isPressed)
        {
            SetButtonStateButtonResult = inButton;
            SetButtonStatePressedResult = isPressed;
        }
    }
}

#endif // UNITY_EDITOR
