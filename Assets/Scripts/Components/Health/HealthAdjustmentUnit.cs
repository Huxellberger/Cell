// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Components.Health
{
    public class HealthAdjustmentUnit
    {
        public readonly int AdjustAmount;
        public readonly GameObject Author;

        public HealthAdjustmentUnit(int inAdjustAmount, GameObject inAuthor)
        {
            AdjustAmount = inAdjustAmount;
            Author = inAuthor;
        }
    }
}
