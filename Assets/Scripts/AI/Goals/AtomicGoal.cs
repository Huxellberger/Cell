// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.AI.Goals
{
    public abstract class AtomicGoal : Goal
    {
        protected AtomicGoal(GameObject inOwner) : base(inOwner)
        {
        }
    }
}
