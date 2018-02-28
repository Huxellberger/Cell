// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Species;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Species
{
    public class MockSpeciesComponent 
        : MonoBehaviour 
        , ISpeciesInterface
    {
        public ESpeciesType GetCurrentSpeciesTypeResult { get; set; }

        public bool SpeciesCryCalled = false;
        public ECryType ? SpeciesCryTypeInput { get; private set; }

        public ECryType ? IsSpeciesCryInProgressInput { get; private set; }
        public bool IsSpeciesCryInProgressResult { get; set; }

        public ESpeciesType GetCurrentSpeciesType()
        {
            return GetCurrentSpeciesTypeResult;
        }

        public void SpeciesCry(ECryType inCryType)
        {
            SpeciesCryCalled = true;
            SpeciesCryTypeInput = inCryType;
        }

        public bool IsSpeciesCryInProgress(ECryType inCryType)
        {
            IsSpeciesCryInProgressInput = inCryType;
            return IsSpeciesCryInProgressResult;
        }
    }
}

#endif // UNITY_EDITOR
