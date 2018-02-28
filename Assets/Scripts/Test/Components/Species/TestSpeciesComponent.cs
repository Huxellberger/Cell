// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Species;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Species
{
    public class TestSpeciesComponent 
        : SpeciesComponent
    {
        public AudioClip PlayedAudioClip { get; private set; }

        private float _getDeltaTimeResult = 0.0f;

        public void TestStart()
        {
            Start();	
        }

        public void TestDestroy()
        {
            OnDestroy();
        }

        public void TestUpdate(float inDeltaTime)
        {
            _getDeltaTimeResult = inDeltaTime;
            Update();
        }

        protected override float GetDeltaTime()
        {
            return _getDeltaTimeResult;
        }

        protected override void PlayAudioClip(AudioClip inClip)
        {
            PlayedAudioClip = inClip;
        }
    }
}

#endif // UNITY_EDITOR
