// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Equipment.Holdables;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Equipment.Holdables
{
    public class MockHoldableComponent 
        : MonoBehaviour 
        , IHoldableInterface
    {
        public EHoldableAction ? UseHoldableInputAction { get; private set; }

        public GameObject OnHeldGameObject { get; private set; }
        public bool OnDroppedCalled = false;

        public GameObject GetHoldableObjectResult { get; set; }

        public void UseHoldable(EHoldableAction inAction)
        {
            UseHoldableInputAction = inAction;
        }

        public void OnHeld(GameObject inGameObject)
        {
            OnHeldGameObject = inGameObject;
        }

        public void OnDropped()
        {
            OnDroppedCalled = true;
        }

        public GameObject GetHoldableObject()
        {
            return GetHoldableObjectResult;
        }
    }
}

#endif // UNITY_EDITOR
