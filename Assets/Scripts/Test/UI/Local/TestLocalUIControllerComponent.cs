// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.UI.Local;
using UnityEngine;

namespace Assets.Scripts.Test.UI.Local
{
    public class TestLocalUIControllerComponent 
        : LocalUIControllerComponent
    {
        public void TestAwake() 
        {
            Awake();
        }
	
        public void TestDestroy()
        {
            OnDestroy();
        }

        public GameObject GetInstantiatedUI()
        {
            return InstantiatedLocalUI;
        }
    }
}

#endif // UNITY_EDITOR
