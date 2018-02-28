// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Services.Wildlife
{
    public interface IWildlifeServiceInterface
    {
        void RegisterWildlife(GameObject inWildlife);
        void UnregisterWildlife(GameObject inWildlife);

        List<LocalWildlifeResult> GetWildlifeInRadius(Vector3 inLocation, float inRadius);
    }
}
