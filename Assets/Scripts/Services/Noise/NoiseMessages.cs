// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Messaging;

namespace Assets.Scripts.Services.Noise
{
    public class NoiseHeardMessage
        : UnityMessagePayload
    {
        public readonly NoiseData HeardNoise;

        public NoiseHeardMessage(NoiseData inNoiseData)
            : base()
        {
            HeardNoise = inNoiseData;
        }
    }
}
