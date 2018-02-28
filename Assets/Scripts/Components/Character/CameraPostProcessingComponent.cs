// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Components.Character
{
    public class CameraPostProcessingComponent 
        : MonoBehaviour
        , ICameraPostProcessingInterface
    {
        protected CameraFader Fader { get; private set; }

        protected void Start()
        {
            Fader = new CameraFader(gameObject);
        }

        protected void Update()
        {
            Fader.Update(GetDeltaTime());
        }

        protected virtual float GetDeltaTime()
        {
            return Time.deltaTime;
        }

        // ICameraPostProcessingInterface
        public void RequestCameraFade(float finalAlpha, float fadeTime)
        {
            Fader.StartFade(finalAlpha, fadeTime);
        }
        // ~ICameraPostProcessingInterface
    }
}
