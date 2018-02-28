// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Character;

namespace Assets.Scripts.Test.Components.Character
{
    public class TestCameraPostProcessingComponent 
        : CameraPostProcessingComponent
    {
        private float _currentDelta;

        public void TestStart ()
        {
            _currentDelta = 0.0f;
		    Start();
        }
	
        public void TestUpdate (float inDelta)
        {
            _currentDelta = inDelta;
            Update();
        }

        protected override float GetDeltaTime()
        {
            return _currentDelta;
        }

        public CameraFader GetCameraFader()
        {
            return Fader;
        }
    }
}

#endif // UNITY_EDITOR
