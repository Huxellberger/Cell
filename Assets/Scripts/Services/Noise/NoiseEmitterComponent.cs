// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Services.Noise
{
    public class NoiseEmitterComponent 
        : MonoBehaviour 
        , INoiseEmitterInterface
    {
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

        public void RecordNoise(NoiseData inNoiseData)
        {
            NoiseServiceInterface.RecordNoise(inNoiseData);
        }
    }
}
