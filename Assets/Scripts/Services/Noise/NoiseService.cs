// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.Services.Noise
{
    public static class NoiseServiceConstants
    {
        public const int NoisesPerUpdate = 20;
    }

    public class NoiseListenerEntry
    {
        public readonly INoiseListenerInterface Listener;
        public Vector3 Location { get; set; }

        public NoiseListenerEntry(INoiseListenerInterface inListener, Vector3 inLocation)
        {
            Listener = inListener;
            Location = inLocation;
        }
    }

    public class NoiseService 
        : MonoBehaviour
        , INoiseServiceInterface
    {
        private readonly Dictionary<INoiseListenerInterface, NoiseListenerEntry> _listenerMappings = new Dictionary<INoiseListenerInterface, NoiseListenerEntry>();
        private readonly List<NoiseListenerEntry> _listenerEntries = new List<NoiseListenerEntry>();

        private readonly List<NoiseData> _noisesToUpdate = new List<NoiseData>(30);

        public void RegisterListener(INoiseListenerInterface inListener, Vector3 inListenerLocation)
        {
            var entry = new NoiseListenerEntry(inListener, inListenerLocation);

            _listenerEntries.Add(entry);
            _listenerMappings.Add(inListener, entry);
        }

        protected void Update()
        {
            for (
                    var currentNoiseIndex = 0;
                    currentNoiseIndex < _noisesToUpdate.Count && currentNoiseIndex < NoiseServiceConstants.NoisesPerUpdate;
                    currentNoiseIndex++
                )
            {
                var currentNoise = _noisesToUpdate[currentNoiseIndex];
                foreach (var listenerEntry in _listenerEntries)
                {
                    if (VectorFunctions.DistanceSquared(currentNoise.NoiseLocation, listenerEntry.Location) < currentNoise.NoiseRadius)
                    {
                        listenerEntry.Listener.OnNoiseHeard(currentNoise);
                    }
                }
            }

            if (_noisesToUpdate.Count < NoiseServiceConstants.NoisesPerUpdate)
            {
                _noisesToUpdate.Clear();
            }
            else
            {
                _noisesToUpdate.RemoveRange(0, NoiseServiceConstants.NoisesPerUpdate - 1);
            }
        }

        public void UnregisterListener(INoiseListenerInterface inListener)
        {
            _listenerMappings.Remove(inListener);
            _listenerEntries.RemoveAll((entry) => entry.Listener == inListener);
        }

        public void UpdateListener(INoiseListenerInterface inListener, Vector3 inListenerLocation)
        {
            if (_listenerMappings.ContainsKey(inListener))
            {
                _listenerMappings[inListener].Location = inListenerLocation;
            }
        }

        public void RecordNoise(NoiseData inNoiseData)
        {
            _noisesToUpdate.Add(inNoiseData);
        }
    }
}
