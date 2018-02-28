// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Services.Spawn;

namespace Assets.Scripts.Test.Services.Spawn
{
    public class TestSpawnLocationComponent : SpawnLocationComponent
    {
        public void TestStart()
        {
            Start();
        }

        public void TestDestroy()
        {
            OnDestroy();
        }
    }
}

#endif // UNITY_EDITOR
