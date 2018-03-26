// Copyright (C) Threetee Gang All Rights Reserved

using System.IO;
using UnityEngine;

namespace Assets.Scripts.Components.Trigger
{
    [RequireComponent(typeof(SpriteRenderer), typeof(AudioSource))]
    public class TactileFeedbackTriggerResponseComponent 
        : TriggerResponseComponent
    {
        public AudioClip TriggerAudioClip;
        public AudioClip CancelAudioClip;

        public Color TriggerColor;
        private Color _initialColor;

        private AudioSource _audioSource;
        private SpriteRenderer _spriteRenderer;

        protected void Awake()
        {
            _audioSource = gameObject.GetComponent<AudioSource>();
            _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

            if (Application.isPlaying)
            {
                _initialColor = _spriteRenderer.color;
            }
        }

        protected override void OnTriggerImpl(TriggerMessage inMessage)
        {
            PlayAudioClip(TriggerAudioClip);

            if (Application.isPlaying)
            {
                _spriteRenderer.color = TriggerColor;
            }
        }

        protected override void OnCancelTriggerImpl(CancelTriggerMessage inMessage)
        {
            PlayAudioClip(CancelAudioClip);

            if (Application.isPlaying)
            {
                _spriteRenderer.color = _initialColor;
            }
        }

        protected override void OnReadData(Stream stream)
        {
            if (Application.isPlaying)
            {
                _spriteRenderer.color = TriggerColor;
            }
        }

        protected virtual void PlayAudioClip(AudioClip inClip)
        {
            if (inClip != null)
            {
                _audioSource.PlayOneShot(inClip);
            }
        }
    }
}
