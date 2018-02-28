// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Controller;
using Assets.Scripts.Mode;
using UnityEngine;

namespace Assets.Scripts.Test.Mode
{
    public class TestGameModeComponent 
        : GameModeComponent
    {
        public void TestStart()
        {
            Start();
        }

        public void TestDestroy()
        {
            OnDestroy();
        }

        public GameObject GetHUDInstance()
        {
            return HUDInstance;
        }

        public void SetActiveController(ControllerComponent inController)
        {
            ActiveController = inController;
        }
    }
}

#endif
