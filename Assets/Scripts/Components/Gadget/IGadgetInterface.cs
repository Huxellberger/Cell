// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Components.Gadget
{
    public interface IGadgetInterface
    {
        void UseGadget(GameObject user);
        EGadgetSlot GetGadgetSlot();
        Sprite GetGadgetIcon();
    }
}
