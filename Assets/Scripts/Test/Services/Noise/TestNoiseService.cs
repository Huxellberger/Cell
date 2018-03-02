// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Services.Noise;

namespace Assets.Scripts.Test.Services.Noise
{
    public class TestNoiseService 
        : NoiseService
    {
        public void TestUpdate()
        {
            Update();
        }
    }
}

#endif // UNITY_EDITOR
