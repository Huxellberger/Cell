// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Components.Equipment.Holdables.Horn
{
    [RequireComponent(typeof(AudioSource))]
    public class HornHoldable 
        : HoldableItemComponent
    {
        public AudioClip PrimaryHornSound;
        public AudioClip SecondaryHornSound;

        private AudioSource _source;
    
        protected void Start()
        {
            _source = gameObject.GetComponent<AudioSource>();
        }

        protected override void UseHoldableImpl(EHoldableAction inAction)
        {
            if (inAction == EHoldableAction.Primary)
            {
                PlaySound(PrimaryHornSound);
            }
            else
            {
                PlaySound(SecondaryHornSound);
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
    }
}
