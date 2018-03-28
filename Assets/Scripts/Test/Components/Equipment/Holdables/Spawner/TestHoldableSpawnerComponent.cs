// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Equipment.Holdables.Spawner;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Equipment.Holdables.Spawner
{
    public class TestHoldableSpawnerComponent 
        : HoldableSpawnerComponent
    {
        public bool HasClearShotResult = true;

        public void TestStart() 
        {
		    Start();
        }

        protected override bool HasClearShot(GameObject target)
        {
            return HasClearShotResult;
        }
    }
}

#endif // UNITY_EDITOR
