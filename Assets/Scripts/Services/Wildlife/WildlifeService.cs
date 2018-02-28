// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.Components.Species;
using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.Services.Wildlife
{
    public class WildlifeService 
        : IWildlifeServiceInterface
    {
        public class WildlifeEntry
        {
            public readonly GameObject WildlifeGameObject;
            public readonly ISpeciesInterface WildlifeSpeciesInterface;

            public bool Valid { get; private set; }

            public WildlifeEntry(GameObject inWildlifeEntry)
            {
                Valid = true;
                WildlifeGameObject = inWildlifeEntry;
                WildlifeSpeciesInterface = WildlifeGameObject.GetComponent<ISpeciesInterface>();

                if (WildlifeSpeciesInterface == null)
                {
                    Debug.LogError("Tried to add a wildlife entry that lacks a proper species interface!");
                    Valid = false;
                }
            }
        }

        private readonly List<WildlifeEntry> _registeredWildlife;

        private List<LocalWildlifeResult> _currentQueryResult;

        public WildlifeService()
        {
            _registeredWildlife = new List<WildlifeEntry>();
            _currentQueryResult = new List<LocalWildlifeResult>();
        }

        public void RegisterWildlife(GameObject inWildlife)
        {
            var entry = new WildlifeEntry(inWildlife);
            if (entry.Valid)
            {
                _registeredWildlife.Add(entry);
            }
        }

        public void UnregisterWildlife(GameObject inWildlife)
        {
            _registeredWildlife.RemoveAll(entry => entry.WildlifeGameObject == inWildlife);
        }

        public List<LocalWildlifeResult> GetWildlifeInRadius(Vector3 inLocation, float inRadius)
        {
            var thresholdSquared = Mathf.Pow(inRadius, 2);

            _currentQueryResult = new List<LocalWildlifeResult>(_registeredWildlife.Count);

            foreach (var registeredAnimal in _registeredWildlife)
            {
                var distanceSquared = VectorFunctions.DistanceSquared(inLocation, registeredAnimal.WildlifeGameObject.transform.position);
                if (distanceSquared < thresholdSquared)
                {
                    _currentQueryResult.Add(new LocalWildlifeResult(registeredAnimal, distanceSquared));
                }
            }

            return _currentQueryResult;
        }
    }
}
