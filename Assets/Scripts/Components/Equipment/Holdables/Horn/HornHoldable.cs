// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Services.Noise;
using UnityEngine;

namespace Assets.Scripts.Components.Equipment.Holdables.Horn
{
    [RequireComponent(typeof(AudioSource), typeof(INoiseEmitterInterface))]
    public class HornHoldable 
        : HoldableItemComponent
    {
        public AudioClip PrimaryHornSound;
        public NoiseData PrimaryHornNoiseData;
        public AudioClip SecondaryHornSound;
        public NoiseData SecondaryHornNoiseData;

        private AudioSource _source;
        private INoiseEmitterInterface _emitter;
    
        protected void Start()
        {
            _source = gameObject.GetComponent<AudioSource>();
            _emitter = gameObject.GetComponent<INoiseEmitterInterface>();
        }

        protected override void UseHoldableImpl(EHoldableAction inAction)
        {
            if (inAction == EHoldableAction.Primary)
            {
                PlaySound(PrimaryHornSound);
                RecordNoise(PrimaryHornNoiseData);
            }
            else
            {
                PlaySound(SecondaryHornSound);
                RecordNoise(SecondaryHornNoiseData);
            }
        }

        protected override void OnHeldImpl()
        {
        }

        protected override void OnDroppedImpl()
        {
        }

        protected virtual void PlaySound(AudioClip inSound)
        {
            if (inSound != null)
            {
                _source.PlayOneShot(inSound);
            }
        }

        private void RecordNoise(NoiseData inData)
        {
            inData.NoiseLocation = gameObject.transform.position;
            _emitter.RecordNoise(inData);
        }
    }
}
