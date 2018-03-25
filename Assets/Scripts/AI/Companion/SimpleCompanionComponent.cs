// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.AI.Companion
{
    [RequireComponent(typeof(AudioSource))]
    public class SimpleCompanionComponent 
        : CompanionComponent
    {
        public AudioClip PowerNoise;
        public bool CanUsePower;

        private AudioSource _source;

        protected override void Awake()
        {
            base.Awake();

            _source = gameObject.GetComponent<AudioSource>();
        }

        protected override bool CanUseCompanionPowerImpl()
        {
            return CanUsePower;
        }

        protected override void CompanionPowerImpl()
        {
            _source.PlayOneShot(PowerNoise);
        }

        protected override void OnLeaderSetImpl()
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<Collider2D>().enabled = false;
        }

        protected override void OnLeaderClearedImpl()
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.GetComponent<Collider2D>().enabled = true;
        }
    }
}
