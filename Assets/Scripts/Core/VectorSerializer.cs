// Copyright (C) Threetee Gang All Rights Reserved

using System;
using UnityEngine;

namespace Assets.Scripts.Core
{
    [Serializable]
    public struct Vector3Serializer
    {
        public float x;
        public float y;
        public float z;

        public Vector3Serializer Fill(Vector3 v3)
        {
            x = v3.x;
            y = v3.y;
            z = v3.z;

            return this;
        }

        public Vector3 AsVector
        { get { return new Vector3(x, y, z); } }
    }
}