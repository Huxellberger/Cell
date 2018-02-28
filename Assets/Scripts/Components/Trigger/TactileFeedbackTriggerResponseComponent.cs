// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Components.Trigger
{
    [RequireComponent(typeof(MeshRenderer), typeof(AudioSource))]
    public class TactileFeedbackTriggerResponseComponent 
        : TriggerResponseComponent
    {
        public AudioClip TriggerAudioClip;
        public AudioClip CancelAudioClip;

        public Color TriggerColor;
        private Color _initialColor;

        private AudioSource _audioSource;
        private MeshRenderer _meshRenderer;

        protected override void Start()
        {
            base.Start();

            _audioSource = gameObject.GetComponent<AudioSource>();
            _meshRenderer = gameObject.GetComponent<MeshRenderer>();

            if (Application.isPlaying)
            {
                _initialColor = _meshRenderer.material.color;
            }
        }

        protected override void OnTriggerImpl(TriggerMessage inMessage)
        {
            PlayAudioClip(TriggerAudioClip);

            if (Application.isPlaying)
            {
                _meshRenderer.material.color = TriggerColor;
            }
        }

        protected override void OnCancelTriggerImpl(CancelTriggerMessage inMessage)
        {
            PlayAudioClip(CancelAudioClip);

            if (Application.isPlaying)
            {
                _meshRenderer.material.color = _initialColor;
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
