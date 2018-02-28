// Copyright (C) Threetee Gang All Rights Reserved

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Assets.Scripts.Components.Character
{
    public static class CameraFaderConstants
    {
        public const float DefaultFinalPostExposure = -10f;
    }

    public class CameraFader
    {
        public class ColorGradeEntry
        {
            public ColorGrading ColorGrade { get; set; }
            public readonly float DefaultPostExposure;

            public float CurrentStartPostExposure { get; set; }
            public float CurrentFinalPostExposure { get; set; }

            public ColorGradeEntry(ColorGrading inColorGrading, float inDefaultPostExposure)
            {
                CurrentStartPostExposure = inColorGrading.postExposure.value;
                CurrentFinalPostExposure = 0.0f;

                ColorGrade = inColorGrading;
                DefaultPostExposure = inDefaultPostExposure;
            }
        }

        public bool FadeInProgress { get; private set; }

        public float FinalAlpha { get; private set; }

        public float FinishTime { get; private set; }
        public float CurrentFadeTime { get; private set; }

        private readonly List<ColorGradeEntry> _colorGradings;

        public CameraFader(GameObject inOwner)
        {
            FadeInProgress = false;

            FinalAlpha = 0.0f;
            FinishTime = 0.0f;
            CurrentFadeTime = 0.0f;

            _colorGradings = new List<ColorGradeEntry>();

            foreach (var postProcessVolume in inOwner.GetComponents<PostProcessVolume>())
            {
                ColorGrading currentColorGrading = null;
                if (postProcessVolume.profile.TryGetSettings(out currentColorGrading))
                {
                    _colorGradings.Add(new ColorGradeEntry(currentColorGrading, currentColorGrading.postExposure));
                }
            }
        }

        public void StartFade(float inFinalAlpha, float inDeltaTime)
        {
            if (Math.Abs(inDeltaTime) < 0.001f)
            {
                InstantFade(inFinalAlpha);
            }
            else
            {
                InitialiseFade(inFinalAlpha, inDeltaTime);
            }
        }

        private void InstantFade(float inFinalAlpha)
        {
            InitialiseFade(inFinalAlpha, 1.0f);

            Update(FinishTime + 0.1f);
        }

        private void InitialiseFade(float inFinalAlpha, float inDeltaTime)
        {
            FadeInProgress = true;
            CurrentFadeTime = 0.0f;

            FinalAlpha = Mathf.Clamp(inFinalAlpha, 0.0f, 1.0f);
            FinishTime = inDeltaTime;

            foreach (var colorGrading in _colorGradings)
            {
                colorGrading.CurrentStartPostExposure = colorGrading.ColorGrade.postExposure.value;
                colorGrading.CurrentFinalPostExposure = Mathf.Lerp(colorGrading.DefaultPostExposure,
                    CameraFaderConstants.DefaultFinalPostExposure, FinalAlpha);
            }
        }

        public void Update(float deltaTime)
        {
            if (FadeInProgress)
            {
                CurrentFadeTime += deltaTime;

                if (CurrentFadeTime >= FinishTime)
                {
                    FadeInProgress = false;
                    CurrentFadeTime = 0.0f;

                    foreach (var colorGrading in _colorGradings)
                    {
                        colorGrading.ColorGrade.postExposure.value = colorGrading.CurrentFinalPostExposure;
                    }
                }
                else
                {
                    foreach (var colorGrading in _colorGradings)
                    {
                        colorGrading.ColorGrade.postExposure.value = Mathf.Lerp(colorGrading.CurrentStartPostExposure,
                            colorGrading.CurrentFinalPostExposure, CurrentFadeTime / FinishTime);
                    }
                }
            }
        }
    }
}
