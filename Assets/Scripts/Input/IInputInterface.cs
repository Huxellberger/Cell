// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.UnityLayer.Input;
using UnityEngine;

namespace Assets.Scripts.Input
{
    public delegate void OnButtonInputDelegate(EInputKey inputKey, bool pressed);
    public delegate void OnAnalogInputDelegate(EInputKey inputKey, float analogValue);
    public delegate void OnMouseInputDelegate(EInputKey inputKey, Vector3 newMousePos);

    public interface IInputInterface
    {
        event OnButtonInputDelegate OnButtonInputEvent;
        event OnAnalogInputDelegate OnAnalogInputEvent;
        event OnMouseInputDelegate OnMouseInputEvent;

        void SetInputMappingProvider(IInputMappingProviderInterface inInputMappingProviderInterface);
        void SetUnityInputInterface(IUnityInputInterface inUnityInputInterface);
    }
}
