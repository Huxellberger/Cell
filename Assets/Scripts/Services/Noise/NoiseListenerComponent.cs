// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.Core;
using Assets.Scripts.Messaging;
using UnityEngine;

namespace Assets.Scripts.Services.Noise
{
    public static class NoiseListenerConstants
    {
        public const float UpdateDistanceSquared = 64.0f;
    }

    public class NoiseListenerComponent 
        : MonoBehaviour 
        , INoiseListenerInterface
    {
        public List<ENoiseType> NoisesOfInterest = new List<ENoiseType>();

        private Vector3 _previousLocation;

        private INoiseServiceInterface _noiseServiceInterface;

        private INoiseServiceInterface NoiseServiceInterface
        {
            get
            {
                if (_noiseServiceInterface == null)
                {
                    _noiseServiceInterface = GameServiceProvider.CurrentInstance.GetService<INoiseServiceInterface>();
                }

                return _noiseServiceInterface;
            }
            set { _noiseServiceInterface = value; }
        }

        protected void Start()
        {
            _previousLocation = gameObject.transform.position;
		    RegisterAsListener();
        }
	
        protected void Update()
        {
            var currentLocation = gameObject.transform.position;

            if (VectorFunctions.DistanceSquared(currentLocation, _previousLocation) > NoiseListenerConstants.UpdateDistanceSquared)
            {
                UpdateListenerLocation(currentLocation);
            }

            _previousLocation = currentLocation;
        }

        protected void OnDestroy()
        {
            UnregisterAsListener();
        }

        public void OnNoiseHeard(NoiseData inNoiseData)
        {
            if (NoisesOfInterest.Contains(inNoiseData.NoiseType))
            {
                UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(gameObject, new NoiseHeardMessage(inNoiseData));
            }
        }

        private void RegisterAsListener()
        {
            NoiseServiceInterface.RegisterListener(this, gameObject.transform.position);
        }

        private void UnregisterAsListener()
        {
            NoiseServiceInterface.UnregisterListener(this);
        }

        private void UpdateListenerLocation(Vector3 inNewLocation)
        {
            NoiseServiceInterface.UpdateListener(this, inNewLocation);
        }
    }
}
