// Copyright (C) Threetee Gang All Rights Reserved 

#if UNITY_EDITOR

using Assets.Scripts.Components.Audio;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Audio
{
    public class TestPlayerMusicComponent 
        : PlayerMusicComponent
    {
        public AudioClip PlayedAudioClip { get; private set; }
        public bool StopPlayingCalled { get; private set; }

        public void TestAwake()
        {
            StopPlayingCalled = false;
		    Awake();
        }

        public void TestDestroy()
        {
            OnDestroy();
        }

        protected override void PlayAudioClip(AudioClip inClip)
        {
            PlayedAudioClip = inClip;
        }

        protected override void StopPlaying()
        {
            StopPlayingCalled = true;
        }
    }
}

#endif // UNITY_EDITOR
