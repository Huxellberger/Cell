// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.UI.MainMenu;

namespace Assets.Scripts.Test.UI.MainMenu
{
    public class TestLoadSceneButtonComponent 
        : LoadSceneButtonComponent
    {
        public void TestClick()
        {
            OnButtonPressed();
        }
    }
}

#endif // UNITY_EDITOR
