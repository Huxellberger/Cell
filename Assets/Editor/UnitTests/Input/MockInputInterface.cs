// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Input;
using Assets.Scripts.UnityLayer.Input;
using UnityEngine;

namespace Assets.Editor.UnitTests.Input
{
    public class MockInputInterface
        : IInputInterface
    {
        public event OnButtonInputDelegate OnButtonInputEvent;
        public event OnAnalogInputDelegate OnAnalogInputEvent;
        public event OnMouseInputDelegate OnMouseInputEvent;

        public void SetInputMappingProvider(IInputMappingProviderInterface inInputMappingProviderInterface)
        {
            throw new System.NotImplementedException();
        }

        public void SetUnityInputInterface(IUnityInputInterface inUnityInputInterface)
        {
            throw new System.NotImplementedException();
        }

        public void TestActivateAnalogEvent(EInputKey inInputKey, float analogValue)
        {
            OnAnalogInputEvent(inInputKey, analogValue);
        }

        public void TestActivateButtonEvent(EInputKey inInputKey, bool pressed)
        {
            OnButtonInputEvent(inInputKey, pressed);
        }

        public void TestActivateMouseEvent(EInputKey inInputKey, Vector3 mousePosition)
        {
            OnMouseInputEvent(inInputKey, mousePosition);
        }
    }
}

#endif
