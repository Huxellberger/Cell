// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.AI.Companion;
using UnityEngine;

namespace Assets.Scripts.Test.AI.Companion
{
    public class MockCompanionSetComponent 
        : MonoBehaviour 
        , ICompanionSetInterface
    {
        public ICompanionInterface SetCompanionResult { get; private set; }
        public ECompanionSlot ? SetCompanionSlotResult { get; private set; }

        public ECompanionSlot ? ClearCompanionSlotResult { get; private set; }
        public ECompanionSlot ? UseCompanionPowerSlotResult { get; private set; }
        public ECompanionSlot ? RequestCompanionDialogueSlotResult { get; private set; }

        public void SetCompanion(ICompanionInterface inCompanion, ECompanionSlot inSlot)
        {
            SetCompanionResult = inCompanion;
            SetCompanionSlotResult = inSlot;
        }

        public void ClearCompanion(ECompanionSlot inSlot)
        {
            ClearCompanionSlotResult = inSlot;
        }

        public void UseCompanionPower(ECompanionSlot inSlot)
        {
            UseCompanionPowerSlotResult = inSlot;
        }

        public void RequestCompanionDialogue(ECompanionSlot inSlot)
        {
            RequestCompanionDialogueSlotResult = inSlot;
        }
    }
}

#endif // UNITY_EDITOR
