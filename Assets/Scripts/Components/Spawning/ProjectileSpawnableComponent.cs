// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Components.Spawning
{
    [RequireComponent(typeof(Rigidbody2D), typeof(AudioSource))]
    public class ProjectileSpawnableComponent 
        : SpawnableComponent
    {
        public float Speed = 5.0f;

        public AudioClip SpawnAudioClip;
        public AudioClip CollisionAudioClip;

        private AudioSource _audioSource;
        private Rigidbody2D _rigidbody;

        protected void Awake()
        {
            _audioSource = gameObject.GetComponent<AudioSource>();
            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        }

        protected override void OnSpawnedImpl()
        {
            PlaySound(SpawnAudioClip);
            _rigidbody.velocity = gameObject.transform.up * Speed;
        }

        private void OnTriggerEnter2D(Collider2D inCollider)
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
