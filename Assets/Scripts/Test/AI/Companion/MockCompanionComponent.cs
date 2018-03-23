// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.AI.Companion;
using UnityEngine;

namespace Assets.Scripts.Test.AI.Companion
{
    public class MockCompanionComponent 
        : MonoBehaviour 
        , ICompanionInterface
    {
        public CompanionData GetCompanionDataResult { get; set; }
        public bool CanUseCompanionPowerResult = true;

        public bool UseCompanionPowerCalled = false;
        public bool RequestDialogueCalled = false;
        public GameObject SetLeaderGameObject { get; private set; }
        public bool ClearLeaderCalled = false;

        public CompanionData GetCompanionData()
        {
            return GetCompanionDataResult;
        }

        public bool CanUseCompanionPower()
        {
            return CanUseCompanionPowerResult;
        }

        public void UseCompanionPower()
        {
            UseCompanionPowerCalled = true;
        }

        public void RequestDialogue()
        {
            RequestDialogueCalled = true;
        }

        public void SetLeader(GameObject inLeader)
        {
            SetLeaderGameObject = inLeader;
        }

        public void ClearLeader()
        {
            ClearLeaderCalled = true;
        }
    }
}

#endif // UNITY_EDITOR
