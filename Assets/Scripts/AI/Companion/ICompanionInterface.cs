// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.AI.Companion
{
    public interface ICompanionInterface
    {
        CompanionData GetCompanionData();
        bool CanUseCompanionPower();
        void UseCompanionPower();
        void RequestDialogue();
        void SetLeader(GameObject inLeader);
        void ClearLeader();
    }
}
