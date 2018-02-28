// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Character;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Character
{
    public class MockCameraPostProcessingComponent 
        : MonoBehaviour
        , ICameraPostProcessingInterface
    {
        public float ? RequestCameraFadeAlpha { get; private set; }
        public float ? RequestCameraFadeFadeTime { get; private set; }

        public void RequestCameraFade(float finalAlpha, float fadeTime)
        {
            RequestCameraFadeAlpha = finalAlpha;
            RequestCameraFadeFadeTime = fadeTime;
        }
    }
}

#endif // UNITY_EDITOR
