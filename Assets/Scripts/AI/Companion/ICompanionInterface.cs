﻿// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.AI.Companion
{
    public interface ICompanionInterface
    {
        float GetCompanionPowerCooldown();
        bool CanUseCompanionPower();
        void UseCompanionPower();
        void RequestDialogue();
        void SetCompanion(GameObject inLeader);
        void ClearCompanion();
    }
}
