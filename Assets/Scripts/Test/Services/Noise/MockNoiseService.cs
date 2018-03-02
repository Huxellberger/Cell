// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Services.Noise;
using UnityEngine;

namespace Assets.Scripts.Test.Services.Noise
{
    public class MockNoiseService 
        : INoiseServiceInterface
    {
	    public INoiseListenerInterface RegisteredListener { get; private set; }
        public Vector3 ? RegisteredLocation { get; private set; }

        public INoiseListenerInterface UnregisteredListener { get; private set; }

        public INoiseListenerInterface UpdatedListener { get; private set; }
        public Vector3 ? UpdatedLocation { get; private set; }

        public NoiseData ? RecordedNoise { get; private set; }

        public void RegisterListener(INoiseListenerInterface inListener, Vector3 inListenerLocation)
        {
            RegisteredListener = inListener;
            RegisteredLocation = inListenerLocation;
        }

        public void UnregisterListener(INoiseListenerInterface inListener)
        {
            UnregisteredListener = inListener;
        }

        public void UpdateListener(INoiseListenerInterface inListener, Vector3 inListenerLocation)
        {
            UpdatedListener = inListener;
            UpdatedLocation = inListenerLocation;
        }

        public void RecordNoise(NoiseData inNoiseData)
        {
            RecordedNoise = inNoiseData;
        }
    }
}

#endif // UNITY_EDITOR
