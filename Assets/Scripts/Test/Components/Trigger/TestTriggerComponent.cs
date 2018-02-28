// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Trigger;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Trigger
{
    public class TestTriggerComponent 
        : TriggerComponent
    {
        public bool CanTriggerResult = true;
        public GameObject CanTriggerGameObjectInput { get; private set; }

        public bool CanCancelTriggerResult = true;
        public GameObject CanCancelTriggerGameObjectInput { get; private set; }

        public void TestCollide(GameObject inGameObject)
        {
            OnGameObjectCollides(inGameObject);
        }

        public void TestStopColliding(GameObject inGameObject)
        {
            OnGameObjectStopsColliding(inGameObject);
        }

        protected override bool CanTrigger(GameObject inGameObject)
        {
            CanTriggerGameObjectInput = inGameObject;
            return CanTriggerResult;
        }

        protected override bool CanCancelTrigger(GameObject inGameObject)
        {
            CanCancelTriggerGameObjectInput = inGameObject;
            return CanCancelTriggerResult;
        }
    }
}

#endif // UNITY_EDITOR
