// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Messaging;

namespace Assets.Scripts.Components.Gadget
{
    public class GadgetUpdatedMessage 
        : UnityMessagePayload
    {
        public readonly IGadgetInterface NewGadget;

        public GadgetUpdatedMessage(IGadgetInterface newGadget)
            : base()
        {
            NewGadget = newGadget;
        }
    }

    public class GadgetUpdatedUIMessage
        : UnityMessagePayload
    {
        public readonly IGadgetInterface NewGadget;

        public GadgetUpdatedUIMessage(IGadgetInterface newGadget)
            : base()
        {
            NewGadget = newGadget;
        }
    }
}
