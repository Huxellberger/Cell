// Copyright (C) Threetee Gang All Rights Reserved

using System;
using UnityEngine;

namespace Assets.Scripts.Services.Noise
{
    [System.Serializable]
    public struct NoiseData
    {
        public Vector3 NoiseLocation;
        public float NoiseRadius;
        public ENoiseType NoiseType;

        public bool Equals(NoiseData otherData)
        {
            return NoiseLocation == otherData.NoiseLocation &&
                   Math.Abs(NoiseRadius - otherData.NoiseRadius) < 0.001f &&
                   NoiseType == otherData.NoiseType;
        }

        public static bool operator ==(NoiseData c1, NoiseData c2)
        {
            return c1.Equals(c2);
        }

        public static bool operator !=(NoiseData c1, NoiseData c2)
        {
            return !c1.Equals(c2);
        }
    }
}
