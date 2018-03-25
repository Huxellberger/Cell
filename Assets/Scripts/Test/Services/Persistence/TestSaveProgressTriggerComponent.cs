// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Services.Persistence;
using UnityEngine;

namespace Assets.Scripts.Test.Services.Persistence
{
    public class TestSaveProgressTriggerComponent 
        : SaveProgressTriggerComponent
    { 
        public void TestCollide(GameObject inGameObject)
        {
            OnGameObjectCollides(inGameObject);
        }
    }
}

#endif // UNITY_EDITOR
