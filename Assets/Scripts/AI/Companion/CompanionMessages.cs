// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.Messaging;

namespace Assets.Scripts.AI.Companion
{
    public class CompanionSlotsUpdatedMessage 
        : UnityMessagePayload
    {
        public readonly Dictionary<ECompanionSlot, PriorCompanionSlotState> Updates;

        public CompanionSlotsUpdatedMessage(Dictionary<ECompanionSlot, PriorCompanionSlotState> updates)
        {
            Updates = updates;
        }
    }

    public class CompanionSlotsUpdatedUIMessage
        : UnityMessagePayload
    {
        public readonly Dictionary<ECompanionSlot, PriorCompanionSlotState> Updates;

        public CompanionSlotsUpdatedUIMessage(Dictionary<ECompanionSlot, PriorCompanionSlotState> updates)
        {
            Updates = updates;
        }
    }
}
