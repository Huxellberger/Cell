// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.ActionStateMachine.States.SurfaceSticking;
using UnityEngine;

namespace Assets.Scripts.Test.Components.ActionStateMachine.States.SurfaceSticking
{
    public class TestStickySurfaceComponent
        : StickySurfaceComponent
    { 
        public void TestStopColliding(GameObject inGameObject)
        {
            OnGameObjectStopsColliding(inGameObject);
        }
    }
}

#endif // UNITY_EDITOR
