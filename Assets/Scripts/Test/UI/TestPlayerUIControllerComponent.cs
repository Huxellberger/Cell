// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.UI;

namespace Assets.Scripts.Test.UI
{
    public class TestPlayerUIControllerComponent 
        : PlayerUIControllerComponent
    {

        public void TestStart ()
        {
		    Start();
        }
	
        public void TestDestroy ()
        {
		    OnDestroy();
        }
    }
}

#endif // UNITY_EDITOR
