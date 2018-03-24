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

        protected override void Start()
        {
            base.Start();

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
        }

        protected override void OnLeaderClearedImpl()
        {
        }
    }
}
