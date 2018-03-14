// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.AI.Goals
{
    public abstract class Goal
    {
        protected GameObject Owner { get; private set; }

        protected Goal(GameObject inOwner)
        {
            Owner = inOwner;
        }

        public float GetDesirability()
        {
            return Mathf.Clamp(CalculateDesirability(), 0.0f, 1.0f);
        }

        public virtual float CalculateDesirability()
        {
            return 0.0f;
        }

        // Called on Creation/Destruction of goal
        public virtual void RegisterGoal() { }
        public virtual void UnregisterGoal() { }

        // Called during active running of goal
        public abstract void Initialise();
        public abstract EGoalStatus Update(float inDeltaTime);
        public abstract void Terminate();
    }
}
