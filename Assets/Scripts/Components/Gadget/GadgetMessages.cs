// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Messaging;
using UnityEngine;

namespace Assets.Scripts.Components.Gadget
{
    public class GadgetUpdatedMessage 
        : UnityMessagePayload
    {
        public readonly IGadgetInterface NewGadget;
        public readonly int SlotCount;

        public GadgetUpdatedMessage(IGadgetInterface newGadget, int slotCount)
            : base()
        {
            NewGadget = newGadget;
            SlotCount = slotCount;
        }
    }

    public class GadgetUpdatedUIMessage
        : UnityMessagePayload
    {
        public readonly Sprite GadgetGraphic;
        public readonly int SlotCount;

        public GadgetUpdatedUIMessage(Sprite gadgetGraphic, int slotCount)
            : base()
        {
            GadgetGraphic = gadgetGraphic;
            SlotCount = slotCount;
        }
    }
}
