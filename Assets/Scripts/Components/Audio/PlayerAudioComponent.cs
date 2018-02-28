// Copyright (C) Threetee Gang All Rights Reserved 

using Assets.Scripts.Components.Health;
using Assets.Scripts.Messaging;

using UnityEngine;

namespace Assets.Scripts.Components.Audio
{
    [RequireComponent(typeof(AudioSource), typeof(UnityMessageEventDispatcherComponent))]
    public class PlayerAudioComponent
        : MonoBehaviour
    {
        public AudioSource SoundEffectsAudioSource;
        public AudioClip DamageSound;

        private UnityMessageEventHandle<HealthChangedMessage> _healthChangedEventHandle;

        protected void Start()
        {
            RegisterForMessages();
        }

        protected void OnDestroy()
        {
            UnregisterForMessages();

            SoundEffectsAudioSource = null;
        }

        private void RegisterForMessages()
        {
            _healthChangedEventHandle = UnityMessageEventFunctions.RegisterActionWithDispatcher<HealthChangedMessage>(gameObject, OnHealthChangedMessage);
        }

        private void UnregisterForMessages()
        {
            UnityMessageEventFunctions.UnregisterActionWithDispatcher(gameObject, _healthChangedEventHandle);
        }

        private void OnHealthChangedMessage(HealthChangedMessage inMessage)
        {
            if (inMessage.HealthChange < 0)
            {
                PlayAudioClip(DamageSound);
            }
        }

        protected virtual void PlayAudioClip(AudioClip inClip)
        {
            if (SoundEffectsAudioSource != null)
            {
                SoundEffectsAudioSource.PlayOneShot(inClip);
            }
        }
    }
}