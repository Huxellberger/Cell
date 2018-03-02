// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Services.Noise
{
    public interface INoiseServiceInterface
    {
        void RegisterListener(INoiseListenerInterface inListener, Vector3 inListenerLocation);
        void UnregisterListener(INoiseListenerInterface inListener);
        void UpdateListener(INoiseListenerInterface inListener, Vector3 inListenerLocation);
        void RecordNoise(NoiseData inNoiseData);
    }
}
