// Copyright (C) Threetee Gang All Rights Reserved 

using Assets.Scripts.Components.ActionStateMachine.States.Dead;
using Assets.Scripts.Components.ActionStateMachine.States.Spawning;
using Assets.Scripts.Messaging;
using UnityEngine;

namespace Assets.Scripts.Components.Audio
{
    public class PlayerMusicComponent 
        : MonoBehaviour
    {
        public AudioSource MusicAudioSource { get; set; }
        public AudioClip DefaultMusic;

        private UnityMessageEventHandle<EnterSpawningActionStateMessage> _enterSpawningHandle;
        private UnityMessageEventHandle<EnterDeadActionStateMessage> _enterDeadHandle;

        protected void Awake()
        {
            RegisterMessages();
        }

        private void RegisterMessages()
        {
            _enterSpawningHandle = UnityMessageEventFunctions.RegisterActionWithDispatcher<EnterSpawningActionStateMessage>(gameObject, OnEnterSpawningActionState);
            _enterDeadHandle = UnityMessageEventFunctions.RegisterActionWithDispatcher<EnterDeadActionStateMessage>(gameObject, OnEnterDeadActionState);
        }

        protected void OnDestroy()
        {
            UnregisterMessages();

            MusicAudioSource = null;
        }

        private void UnregisterMessages()
        {
            UnityMessageEventFunctions.UnregisterActionWithDispatcher(gameObject, _enterDeadHandle);
            UnityMessageEventFunctions.UnregisterActionWithDispatcher(gameObject, _enterSpawningHandle);
        }

        private void OnEnterSpawningActionState(EnterSpawningActionStateMessage inMessage)
        {
            PlayAudioClip(DefaultMusic);
        }

        private void OnEnterDeadActionState(EnterDeadActionStateMessage inMessage)
        {
            StopPlaying();
        }

        protected virtual void PlayAudioClip(AudioClip inClip)
        {
            if (MusicAudioSource != null)
            {
                MusicAudioSource.clip = inClip;
                MusicAudioSource.Play();
            }
        }

        protected virtual void StopPlaying()
        {
            if (MusicAudioSource != null && MusicAudioSource.isPlaying)
            {
                MusicAudioSource.Stop();
            }
        }
    }
}
