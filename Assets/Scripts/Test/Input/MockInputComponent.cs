// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Input;
using Assets.Scripts.UnityLayer.Input;
using UnityEngine;

namespace Assets.Scripts.Test.Input
{
    public class MockInputComponent 
        : MonoBehaviour
        , IInputInterface
    {
        public event OnButtonInputDelegate OnButtonInputEvent;
        public event OnAnalogInputDelegate OnAnalogInputEvent;
        public event OnMouseInputDelegate OnMouseInputEvent;

        public IInputMappingProviderInterface InputMappingProvider { get; private set; }
        public IUnityInputInterface UnityInputInterface { get; private set; }

        public void PrepareForTest(params object[] parameters)
        {
            InputMappingProvider = null;
            UnityInputInterface = null;
        }
 
        public void SetInputMappingProvider(IInputMappingProviderInterface inInputMappingProviderInterface)
        {
            InputMappingProvider = inInputMappingProviderInterface;
        }

        public void SetUnityInputInterface(IUnityInputInterface inUnityInputInterface)
        {
            UnityInputInterface = inUnityInputInterface;
        }
    }
}

#endif
