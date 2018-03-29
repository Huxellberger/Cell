// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Gadget;

namespace Assets.Scripts.Test.Components.Gadget
{
    public class TestGadgetSetComponent 
        : GadgetSetComponent
    {
        public void TestAwake()
        {
            Awake();
        }
    }
}

#endif // UNITY_EDITOR
