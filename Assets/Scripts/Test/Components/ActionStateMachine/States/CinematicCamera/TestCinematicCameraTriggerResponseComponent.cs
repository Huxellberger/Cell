// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.ActionStateMachine.States.CinematicCamera;

namespace Assets.Scripts.Test.Components.ActionStateMachine.States.CinematicCamera
{
    public class TestCinematicCameraTriggerResponseComponent 
        : CinematicCameraTriggerResponseComponent
    {
        public void TestStart()
        {
            Start();
        }

        public void TestDestroy()
        {
            OnDestroy();
        }
    }
}

#endif // UNITY_EDITOR
