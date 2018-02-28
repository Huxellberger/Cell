// Copyright (C) Threetee Gang All Rights Reserved

namespace Assets.Scripts.Components.Species
{
    public interface ISpeciesInterface
    {
        ESpeciesType GetCurrentSpeciesType();

        void SpeciesCry(ECryType inCryType);
        bool IsSpeciesCryInProgress(ECryType inCryType);
    }
}
