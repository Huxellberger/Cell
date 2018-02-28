// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Trigger;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Trigger
{
    public class TestTactileFeedbackTriggerResponseComponent 
        : TactileFeedbackTriggerResponseComponent
    {
        public AudioClip PlayedAudioClip { get; private set; }

        public void TestStart()
        {
            Start();
        }
	
        public void TestDestroy()
        {
            OnDestroy();
        }

        protected override void PlayAudioClip(AudioClip inClip)
        {
            PlayedAudioClip = inClip;
        }
    }
}

#endif // UNITY_EDITOR
