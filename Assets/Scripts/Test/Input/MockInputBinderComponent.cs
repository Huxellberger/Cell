// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using Assets.Scripts.Input;
using UnityEngine;

namespace Assets.Scripts.Test.Input
{
    public class MockInputBinderComponent 
        : MonoBehaviour
        , IInputBinderInterface
    {
        public IInputInterface InputInterface { get; private set; }

        public readonly IList<InputHandler> RegisteredHandlers = new List<InputHandler>();
        public readonly IList<InputHandler> UnregisteredHandlers = new List<InputHandler>();

        public void SetInputInterface(IInputInterface inInputInterface)
        {
            InputInterface = inInputInterface;
        }

        public void RegisterInputHandler(InputHandler inInputHandler)
        {
            RegisteredHandlers.Add(inInputHandler);
        }

        public void UnregisterInputHandler(InputHandler inInputHandler)
        {
            UnregisteredHandlers.Add(inInputHandler);
        }

        public bool IsHandlerOfTypeRegistered<TInputHandlerType>()
            where TInputHandlerType : InputHandler
        {
            foreach (var registeredHandler in RegisteredHandlers)
            {
                try
                {
                    if ((TInputHandlerType)registeredHandler != null)
                    {
                        return true;
                    }
                }
                catch (InvalidCastException e)
                {
                    Debug.Log(e);
                }
            }

            return false;
        }

        public bool IsHandlerOfTypeUnregistered<TInputHandlerType>()
            where TInputHandlerType : InputHandler
        {
            foreach (var unregisteredHandler in UnregisteredHandlers)
            {
                try
                {
                    if ((TInputHandlerType)unregisteredHandler != null)
                    {
                        return true;
                    }
                }
                catch (InvalidCastException e)
                {
                }
            }

            return false;
        }
    }
}

#endif
