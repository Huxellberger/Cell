// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Services.Noise;
using UnityEngine;

namespace Assets.Scripts.Test.Services.Noise
{
    public class MockNoiseListenerComponent 
        : MonoBehaviour
        , INoiseListenerInterface
    {
        public NoiseData ? NoiseHeard { get; private set; }
        public int NoiseHeardCount = 0;

        public void OnNoiseHeard(NoiseData inNoiseData)
        {
            NoiseHeard = inNoiseData;
            NoiseHeardCount++;
        }
    }
}

#endif // UNITY_EDITOR
