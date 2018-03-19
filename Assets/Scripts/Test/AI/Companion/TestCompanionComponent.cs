// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.AI.Companion;

namespace Assets.Scripts.Test.AI.Companion
{
    public class TestCompanionComponent 
        : CompanionComponent
    {
        private float _deltaTime;

        public bool CanUseCompanionPowerImplResult = true;

        public bool OnLeaderSetImplCalled = false;
        public bool OnLeaderClearedImplCalled = false;
        public bool CompanionPowerImplCalled = false;


        public void TestStart() 
        {
            Start();
        }
	
        public void TestUpdate(float deltaTime)
        {
            _deltaTime = deltaTime;
            Update();
        }

        protected override float GetDeltaTime()
        {
            return _deltaTime;
        }

        protected override bool CanUseCompanionPowerImpl()
        {
            return CanUseCompanionPowerImplResult;
        }

        protected override void CompanionPowerImpl()
        {
            CompanionPowerImplCalled = true;
        }

        protected override void OnLeaderSetImpl()
        {
            OnLeaderSetImplCalled = true;
        }

        protected override void OnLeaderClearedImpl()
        {
            OnLeaderClearedImplCalled = true;
        }
    }
}

#endif // UNITY_EDITOR
