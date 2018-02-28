// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.UnityLayer.Input
{
    public class DefaultUnityInput
        : IUnityInputInterface
    {
        // IUnityInputInterface
        public float GetAxis(string axisName)
        {
            /* ToDo Consider whether we want to keep input smoothing
            // Input smoothing fails on time scale of zero
            if (PauseFunctions.IsGameUnpaused())
            {
                return UnityEngine.Input.GetAxis(axisName);
            }
            */

            return UnityEngine.Input.GetAxisRaw(axisName);
        }

        public bool GetButton(string buttonName)
        {
            return UnityEngine.Input.GetButton(buttonName);
        }

        public Vector3 GetMousePosition()
        {
            return UnityEngine.Input.mousePosition;
        }
        // ~IUnityInputInterface
    }
}
