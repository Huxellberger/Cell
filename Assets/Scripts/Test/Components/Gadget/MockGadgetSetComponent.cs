// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Gadget;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Gadget
{
    public class MockGadgetSetComponent 
        : MonoBehaviour 
        , IGadgetSetInterface
    {
        public IGadgetInterface CanAddGadgetParameter { get; private set; }
        public bool CanAddGadgetResult = true;
        public IGadgetInterface AddGadgetResult { get; private set; }
        public bool UseActiveGadgetCalled = false;
        public int? CycleActiveGadgetResult { get; private set; }

        public bool CanAddGadget(IGadgetInterface gadget)
        {
            CanAddGadgetParameter = gadget;

            return CanAddGadgetResult;
        }

        public void AddGadget(IGadgetInterface gadget)
        {
            AddGadgetResult = gadget;
        }

        public void UseActiveGadget()
        {
            UseActiveGadgetCalled = true;
        }

        public void CycleActiveGadget(int cycleAmount)
        {
            CycleActiveGadgetResult = cycleAmount;
        }
    }
}

#endif // UNITY_EDITOR
