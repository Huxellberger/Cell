// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Health.Damage;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Health.Damage
{
    public class TestProximityHealthAdjustmentComponent 
        : ProximityHealthAdjustmentComponent
    {
        public void TestCollide(GameObject inGameObject)
        {
            OnGameObjectCollides(inGameObject);
        }
    }
}

#endif
