// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Components.Spawning
{
    public interface ISpawnerInterface
    {
        void Spawn();
        void RequestRespawn(GameObject inGameObject);
    }
}
