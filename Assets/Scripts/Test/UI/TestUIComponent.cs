// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Messaging;
using Assets.Scripts.UI;

namespace Assets.Scripts.Test.UI
{
    public class TestUIComponent 
        : UIComponent
    {
        public bool OnStartCalled = false;
        public bool OnEndCalled = false;

        public void TestStart ()
        {
		    Start();
        }

        public void TestDestroy()
        {
            OnDestroy();
        }

        public UnityMessageEventDispatcher GetDispatcher()
        {
            return Dispatcher;
        }

        protected override void OnStart()
        {
            OnStartCalled = true;
        }

        protected override void OnEnd()
        {
            OnEndCalled = true;
        }
    }
}

#endif // UNITY_EDITOR
