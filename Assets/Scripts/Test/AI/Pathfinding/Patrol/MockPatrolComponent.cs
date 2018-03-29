// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.AI.Pathfinding.Patrol;
using UnityEngine;

namespace Assets.Scripts.Test.AI.Pathfinding.Patrol
{
    public class MockPatrolComponent 
        : MonoBehaviour 
        , IPatrolInterface
    {
        public bool StartPatrollingCalled = false;
        public bool StopPatrollingCalled = false;

        public void StartPatrolling()
        {
            StartPatrollingCalled = true;
        }

        public void StopPatrolling()
        {
            StopPatrollingCalled = true;
        }
    }
}

#endif // UNITY_EDITOR
