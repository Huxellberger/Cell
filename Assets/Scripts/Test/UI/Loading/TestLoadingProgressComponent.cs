// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Messaging;
using Assets.Scripts.UI.Loading;

namespace Assets.Scripts.Test.UI.Loading
{
    public class TestLoadingProgressComponent 
        : LoadingProgressComponent
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
