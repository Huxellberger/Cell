// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.AI.Pathfinding;

namespace Assets.Scripts.Test.AI.Pathfinding
{
    public class TestPathfindingComponent 
        : PathfindingComponent
    {
        private float _getDeltaTimeResult = 0.0f;

        public void TestStart ()
        {
            Start();
        }
	
        public void TestUpdate (float inDeltaTime)
        {
            _getDeltaTimeResult = inDeltaTime;
            Update();
        }

        protected override float GetDeltaTime()
        {
            return _getDeltaTimeResult;
        }
    }
}

#endif // UNITY_EDTITOR
