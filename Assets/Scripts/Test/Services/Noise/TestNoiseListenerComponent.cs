// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Services.Noise;

namespace Assets.Scripts.Test.Services.Noise
{
    public class TestNoiseListenerComponent 
        : NoiseListenerComponent
    {
        public void TestStart()
        {
            Start();
        }

        public void TestUpdate()
        {
            Update();
        }

        public void TestDestroy()
        {
            OnDestroy();
        }
    }
}

#endif // UNITY_EDITOR
