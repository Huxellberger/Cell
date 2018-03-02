// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Services.Noise;
using UnityEngine;

namespace Assets.Scripts.Test.Services.Noise
{
    public class MockNoiseEmitterComponent 
        : MonoBehaviour 
            , INoiseEmitterInterface
    {
        public NoiseData ? RecordedNoise { get; private set; }

        public void RecordNoise(NoiseData inNoiseData)
        {
            RecordedNoise = inNoiseData;
        }
    }
}

#endif // UNITY_EDITOR
