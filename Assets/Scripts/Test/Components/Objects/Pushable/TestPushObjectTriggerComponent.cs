// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Objects.Pushable;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Objects.Pushable
{
    public class TestPushObjectTriggerComponent 
        : PushObjectTriggerComponent
    {
        public void TestCollide(GameObject inGameObject)
        {
            OnGameObjectCollides(inGameObject);
        }

        public void TestStopColliding(GameObject inGameObject)
        {
            OnGameObjectStopsColliding(inGameObject);
        }
    }
}

#endif // UNITY_EDITOR
