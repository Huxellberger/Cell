// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using System.IO;
using Assets.Scripts.Services.Persistence;
using UnityEngine;

namespace Assets.Scripts.Test.Services.Persistence
{
    public class MockPersistentEntityComponent 
        : MonoBehaviour 
            , IPersistentEntityInterface
    {
        public Stream WriteDataStream { get; private set; }
        public Stream ReadDataStream { get; private set; }

        public void WriteData(Stream stream)
        {
            WriteDataStream = stream;
        }

        public void ReadData(Stream stream)
        {
            ReadDataStream = stream;
        }
    }
}

#endif // UNITY_EDITOR
