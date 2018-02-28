// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Interaction;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Interaction
{
    public class TestInteractionZone 
        : InteractionZone
    {
        public void TestStart ()
        {
		    Start();
        }

        public void TestDisable()
        {
            OnDisable();
        }

        public void TestCollide(GameObject inObject)
        {
            OnGameObjectCollides(inObject);
        }

        public void TestCollideStop(GameObject inObject)
        {
            OnGameObjectStopsColliding(inObject);
        }
    }
}

#endif // UNITY_EDITOR
