// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using System.Collections.Generic;
using Assets.Scripts.Services.Wildlife;
using UnityEngine;

namespace Assets.Scripts.Test.Services.Wildlife
{
    public class MockWildlifeService 
        : IWildlifeServiceInterface
    {
        public GameObject RegisterWildlifeResult { get; private set; }
        public GameObject UnregisterWildlifeResult { get; private set; }

        public List<LocalWildlifeResult> GetWildlifeInRadiusResult = new List<LocalWildlifeResult>();
        public Vector3 GetWildlifeInRadiusLocationInput { get; private set; }
        public float GetWildlifeInRadiusDesiredRadiusInput { get; private set; }

        public void RegisterWildlife(GameObject inWildlife)
        {
            RegisterWildlifeResult = inWildlife;
        }

        public void UnregisterWildlife(GameObject inWildlife)
        {
            UnregisterWildlifeResult = inWildlife;
        }

        public List<LocalWildlifeResult> GetWildlifeInRadius(Vector3 inLocation, float inRadius)
        {
            GetWildlifeInRadiusLocationInput = inLocation;
            GetWildlifeInRadiusDesiredRadiusInput = inRadius;

            return GetWildlifeInRadiusResult;
        }
    }
}

#endif // UNITY_EDITOR
