// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.Messaging;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Wildlife;
using UnityEngine;

namespace Assets.Scripts.Components.Species
{
    public class SpeciesComponent 
        : MonoBehaviour
        , ISpeciesInterface
    {
        [System.Serializable]
        public class CryAudioClipEntry
        {
            public ECryType CryType;
            public AudioClip CrySound;

            public CryAudioClipEntry()
                : this(ECryType.Positive, null)
            {
            }

            public CryAudioClipEntry(ECryType inCryType, AudioClip inCrySound)
            {
                CryType = inCryType;
                CrySound = inCrySound;
            }
        }

        public AudioSource SpeciesAudioSource;
        public List<CryAudioClipEntry> CryClips = new List<CryAudioClipEntry>();
        private Dictionary<ECryType, AudioClip> _speciesCryMap;

        public ESpeciesType InitialSpeciesType;
        private ESpeciesType ? _currentSpeciesType;

        private ECryType? _cryInProgress;
        private AudioClip _currentCry;
        private float _cryTimeElapsed = 0.0f;

        protected void Start()
        {
            InitialiseCryDictionary();

            _currentSpeciesType = InitialSpeciesType;

            GameServiceProvider.CurrentInstance.GetService<IWildlifeServiceInterface>().RegisterWildlife(gameObject);
        }

        private void InitialiseCryDictionary()
        {
            _speciesCryMap = new Dictionary<ECryType, AudioClip>();

            foreach (var cryClip in CryClips)
            {
                _speciesCryMap.Add(cryClip.CryType, cryClip.CrySound);
            }
        }

        protected void OnDestroy()
        {
            GameServiceProvider.CurrentInstance.GetService<IWildlifeServiceInterface>().UnregisterWildlife(gameObject);
        }

        protected void Update()
        {
            if (IsCryInProgress())
            {
                var deltaTime = GetDeltaTime();

                _cryTimeElapsed += deltaTime;

                if (_cryTimeElapsed > _currentCry.length)
                {
                    _cryTimeElapsed = 0.0f;
                    _cryInProgress = null;
                    _currentCry = null;
                }
            }
        }

        protected virtual float GetDeltaTime()
        {
            return Time.deltaTime;
        }

        // ISpeciesInterface
        public ESpeciesType GetCurrentSpeciesType()
        {
            if (_currentSpeciesType == null)
            {
                return InitialSpeciesType;
            }

            return _currentSpeciesType.Value;
        }

        public void SpeciesCry(ECryType inCryType)
        {
            if (!IsCryInProgress())
            {
                _cryInProgress = inCryType;
                UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(gameObject, new SpeciesCryMessage(_cryInProgress.Value));

                if (_speciesCryMap.ContainsKey(inCryType))
                {
                    _currentCry = _speciesCryMap[inCryType];

                    PlayAudioClip(_currentCry);
                }
            }
        }

        public bool IsSpeciesCryInProgress(ECryType inCryType)
        {
            return _cryInProgress == inCryType;
        }
        // ~ISpeciesInterface

        protected virtual void PlayAudioClip(AudioClip inClip)
        {
            SpeciesAudioSource.PlayOneShot(inClip);
        }

        private bool IsCryInProgress()
        {
            return _cryInProgress != null;
        }
    }
}
