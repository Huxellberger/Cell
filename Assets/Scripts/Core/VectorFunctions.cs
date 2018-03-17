// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Core
{
    public static class VectorFunctions
    {
        public static float DistanceSquared(Vector3 firstVector, Vector3 secondVector)
        {
            return Mathf.Pow((firstVector.x - secondVector.x), 2) + Mathf.Pow((firstVector.y - secondVector.y), 2) +
                   Mathf.Pow((firstVector.z - secondVector.z), 2);
        }

        public static float DistanceSquared(Vector2 firstVector, Vector2 secondVector)
        {
            return Mathf.Pow((firstVector.x - secondVector.x), 2) + Mathf.Pow((firstVector.y - secondVector.y), 2);
        }

        public static Vector3 LerpVector(Vector3 firstVector, Vector3 secondVector, float lerpPoint)
        {
            lerpPoint = Mathf.Clamp(lerpPoint, 0.0f, 1.0f);

            return new Vector3
            (
                Mathf.Lerp(firstVector.x, secondVector.x, lerpPoint),
                Mathf.Lerp(firstVector.y, secondVector.y, lerpPoint),
                Mathf.Lerp(firstVector.z, secondVector.z, lerpPoint)
            );
        }
    }
}
