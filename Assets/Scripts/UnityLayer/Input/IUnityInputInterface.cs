// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.UnityLayer.Input
{
    public interface IUnityInputInterface
    {
        float GetAxis(string axisName);
        bool GetButton(string buttonName);
        Vector3 GetMousePosition();
    }
}
