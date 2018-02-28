// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Species;
using UnityEngine;

namespace Assets.Scripts.Services.Wildlife
{
    public class LocalWildlifeResult
    {
        public readonly ISpeciesInterface Wildlife;
        public readonly GameObject WildlifeGameObject;
        public readonly float DistanceSquared;

        public LocalWildlifeResult(WildlifeService.WildlifeEntry inEntry, float inDistanceSquared)
        {
            Wildlife = inEntry.WildlifeSpeciesInterface;
            WildlifeGameObject = inEntry.WildlifeGameObject;
            DistanceSquared = inDistanceSquared;
        }
    }
}
