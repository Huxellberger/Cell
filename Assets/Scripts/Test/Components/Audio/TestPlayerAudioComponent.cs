// Copyright (C) Threetee Gang All Rights Reserved 

#if UNITY_EDITOR

using Assets.Scripts.Components.Audio;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Audio
{
    public class TestPlayerAudioComponent 
        : PlayerAudioComponent
    {
        public AudioClip PlayedClip { get; private set; }

        public void TestStart()
        {
            PlayedClip = null;
            Start();
        }

        public void TestDestroy()
        {
            OnDestroy();
        }

        protected override void PlayAudioClip(AudioClip inClip)
        {
            PlayedClip = inClip;
        }
    }
}

#endif // UNITY_EDITOR
