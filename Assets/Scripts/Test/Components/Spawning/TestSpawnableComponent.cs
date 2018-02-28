// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Spawning;

namespace Assets.Scripts.Test.Components.Spawning
{
    public class TestSpawnableComponent 
        : SpawnableComponent
    {
        public bool OnSpawnedImplCalled = false;

        public void TestDespawn()
        {
            Despawn();
        }

        protected override void OnSpawnedImpl()
        {
            OnSpawnedImplCalled = true;
        }
    }
}

#endif // UNITY_EDITOR
