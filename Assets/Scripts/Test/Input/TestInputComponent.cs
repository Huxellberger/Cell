// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Input;

namespace Assets.Scripts.Test.Input
{
    public class TestInputComponent
        : InputComponent
    {
        public void TestAwake()
        {
            Awake();
        }

        public void TestUpdate()
        {
            Update();
        }
    }
}

#endif
