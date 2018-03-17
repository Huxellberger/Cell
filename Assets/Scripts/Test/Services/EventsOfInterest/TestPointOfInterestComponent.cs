// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Services.EventsOfInterest;
using UnityEngine;

namespace Assets.Scripts.Test.Services.EventsOfInterest
{
    public class TestPointOfInterestComponent 
        : PointOfInterestComponent
    {
        public void TestCollide(GameObject inCollider)
        {
            OnGameObjectCollides(inCollider);
        }
    }
}

#endif // UNITY_EDITOR
