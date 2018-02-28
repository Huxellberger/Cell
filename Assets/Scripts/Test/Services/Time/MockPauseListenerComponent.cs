// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Services.Time;
using UnityEngine;

namespace Assets.Scripts.Test.Services.Time
{
    public class MockPauseListenerComponent 
        : MonoBehaviour
            , IPauseListenerInterface
    {
        public EPauseStatus ? UpdatePauseStatusResult { get; private set; }
	
        public void UpdatePauseStatus(EPauseStatus inStatus)
        {
            UpdatePauseStatusResult = inStatus;
        }
    }
}

#endif // UNITY_EDITOR
