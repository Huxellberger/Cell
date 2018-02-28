// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.AI.Pathfinding;
using UnityEngine;

namespace Assets.Scripts.Test.AI.Pathfinding
{
    public class MockPathfindingComponent 
        : MonoBehaviour
        , IPathfindingInterface
    {
        public GameObject SetFollowTargetResult { get; private set; }
        public Vector3 TargetLocation { get; private set; }
        private OnPathfindingCompleteDelegate _delegate;

        public void SetTargetLocation(Vector3 targetLocation, OnPathfindingCompleteDelegate inDelegate)
        {
            TargetLocation = targetLocation;
            _delegate = inDelegate;
        }

        public void SetFollowTarget(GameObject inTarget)
        {
            SetFollowTargetResult = inTarget;
        }

        public void CompleteDelegate()
        {
            _delegate();
        }
    }
}

#endif // UNITY_EDITOR
