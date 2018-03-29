// Copyright (C) Threetee Gang All Rights Reserved

namespace Assets.Scripts.Components.Gadget
{
    public interface IGadgetSetInterface
    {
        bool CanAddGadget(IGadgetInterface gadget);
        void AddGadget(IGadgetInterface gadget);
        void UseActiveGadget();
        void CycleActiveGadget(int cycleAmount);
    }
}
