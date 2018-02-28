// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Components.Spawning
{
    [RequireComponent(typeof(Rigidbody), typeof(AudioSource))]
    public class ProjectileSpawnableComponent 
        : SpawnableComponent
    {
        public float Speed = 5.0f;

        public AudioClip SpawnAudioClip;
        public AudioClip CollisionAudioClip;

        private AudioSource _audioSource;
        private Rigidbody _rigidbody;

        protected void Awake()
        {
            _audioSource = gameObject.GetComponent<AudioSource>();
            _rigidbody = gameObject.GetComponent<Rigidbody>();
        }

        protected override void OnSpawnedImpl()
        {
            PlaySound(SpawnAudioClip);
            _rigidbody.velocity = gameObject.transform.forward * Speed;
        }

        private void OnTriggerEnter(Collider inCollider)
        {
            if (inCollider != null && inCollider.gameObject != null)
            {
                OnGameObjectCollides(inCollider.gameObject);
            }
        }

        protected void OnGameObjectCollides(GameObject inGameObject)
        {
            PlaySound(CollisionAudioClip);
            Despawn();
        }

        protected virtual void PlaySound(AudioClip inClip)
        {
            if (inClip != null)
            {
                _audioSource.PlayOneShot(inClip);
            }
        }
    }
}
