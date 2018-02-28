// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Components.Character
{
    public interface ICameraPostProcessingInterface
    {
        void RequestCameraFade(float finalAlpha, float fadeTime);
    }
}
