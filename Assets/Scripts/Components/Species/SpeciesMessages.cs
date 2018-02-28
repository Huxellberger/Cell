// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Messaging;

namespace Assets.Scripts.Components.Species
{
    public class SpeciesCryMessage 
        : UnityMessagePayload
    {
        public readonly ECryType Cry;

        public SpeciesCryMessage(ECryType inCry)
            : base()
        {
            Cry = inCry;
        }
    }
}
