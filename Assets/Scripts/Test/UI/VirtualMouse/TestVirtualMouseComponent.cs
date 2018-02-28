// Copyright (C) Threetee Gang All Rights Reserved 

#if UNITY_EDITOR

using Assets.Scripts.UI.VirtualMouse;

namespace Assets.Scripts.Test.UI.VirtualMouse
{
    public class TestVirtualMouseComponent 
        : VirtualMouseComponent
    {
        private float _getDeltaTimeResult;

        public void TestAwake()
        {
            Awake();
        }

        public void TestDestroy()
        {
            OnDestroy();
        }

        public void TestUpdate(float inDeltaTime)
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

#endif // UNITY_EDITOR
