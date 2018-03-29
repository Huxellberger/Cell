// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Messaging;
using Assets.Scripts.UI.HUD;

namespace Assets.Scripts.Test.UI.HUD
{
    public class TestGadgetHUDComponent
        : GadgetHUDComponent
    {
        public UnityMessageEventDispatcher TestDispatcher;

        public void TestStart()
        {
            TestDispatcher = new UnityMessageEventDispatcher();
            Start();
        }

        public void TestDestroy()
        {
            OnDestroy();
        }

        protected override UnityMessageEventDispatcher GetUIDispatcher()
        {
            return TestDispatcher;
        }
    }
}

#endif // UNITY_EDITOR
