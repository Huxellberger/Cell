// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Messaging;
using Assets.Scripts.UI.HUD.Conversation;
using UnityEngine;

namespace Assets.Scripts.Test.UI.HUD.Conversation
{
    public class TestDialogueHUDComponent 
        : DialogueHUDComponent
    {
        public UnityMessageEventDispatcher DispatcherOverride;
        private float _deltaTime;

        public bool StopAudioCalled = false;
        public AudioClip PlayedAudioClip = null;

        public void TestStart() 
        {
            Start();
        }

        public void TestDestroy()
        {
            OnDestroy();
        }
	
        public void TestUpdate(float deltaTime)
        {
            _deltaTime = deltaTime;
            Update();
        }

        protected override UnityMessageEventDispatcher GetUIDispatcher()
        {
            return DispatcherOverride;
        }

        protected override void PlayChatterSound(AudioClip clip)
        {
            PlayedAudioClip = clip;
        }

        protected override void StopChatterSound()
        {
            StopAudioCalled = true;
        }

        protected override float GetDeltaTime()
        {
            return _deltaTime;
        }
    }
}

#endif // UNITY_EDITOR
