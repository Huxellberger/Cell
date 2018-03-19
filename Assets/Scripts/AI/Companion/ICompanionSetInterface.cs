// Copyright (C) Threetee Gang All Rights Reserved

namespace Assets.Scripts.AI.Companion
{
    public interface ICompanionSetInterface
    {
        void SetCompanion(ICompanionInterface inCompanion, ECompanionSlot inSlot);
        void ClearCompanion(ECompanionSlot inSlot);
        void UseCompanionPower(ECompanionSlot inSlot);
        void RequestCompanionDialogue(ECompanionSlot inSlot);
    }
}
