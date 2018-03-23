// Copyright (C) Threetee Gang All Rights Reserved

using System;
using System.Collections.Generic;
using Assets.Scripts.Messaging;
using UnityEngine;

namespace Assets.Scripts.AI.Companion
{
    public class PriorCompanionSlotState
    {
        public void ConvertDataToState(CompanionData inData)
        {
            PriorUIIcon = inData.Image;
            PriorCooldown = inData.PowerCooldown;
            PriorUseCount = inData.PowerUseCount;
        }

        public ICompanionInterface PriorCompanion;
        public Sprite PriorUIIcon;
        public float PriorCooldown;
        public int PriorUseCount;
        public bool PriorActive;
    }

    public class CompanionSetComponent 
        : MonoBehaviour
        , ICompanionSetInterface
    {
        private readonly Dictionary<ECompanionSlot, ICompanionInterface> _companions = new Dictionary<ECompanionSlot, ICompanionInterface>();
        private readonly Dictionary<ECompanionSlot, PriorCompanionSlotState> _priorStates = new Dictionary<ECompanionSlot, PriorCompanionSlotState>();

        protected void Start()
        {
            foreach (ECompanionSlot slot in Enum.GetValues(typeof(ECompanionSlot)))
            {
                _companions.Add(slot, null);
                _priorStates.Add(slot, new PriorCompanionSlotState());
            }
        }

        protected void Update()
        {
            var stateAltered = false;
            foreach (var priorState in _priorStates)
            {
                if (UpdateEntry(priorState.Key, priorState.Value))
                {
                    stateAltered = true;
                }
            }

            if (stateAltered)
            {
                UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(gameObject, new CompanionSlotsUpdatedMessage(_priorStates));
            }
        }

        private bool UpdateEntry(ECompanionSlot inSlot, PriorCompanionSlotState inPriorState)
        {
            var entryChanged = false;

            var currentCompanion = _companions[inSlot];
            if (currentCompanion != inPriorState.PriorCompanion)
            {
                entryChanged = true;

                inPriorState.PriorCompanion = currentCompanion;
                if (currentCompanion != null)
                {
                    inPriorState.PriorActive = currentCompanion.CanUseCompanionPower();
                    inPriorState.ConvertDataToState(currentCompanion.GetCompanionData());
                }
            }
            else
            {
                if (currentCompanion != null)
                {
                    var newData = currentCompanion.GetCompanionData();
                    
                    if (Math.Abs(newData.PowerCooldown - inPriorState.PriorCooldown) > 0.04f)
                    {
                        entryChanged = true;
                        inPriorState.PriorCooldown = newData.PowerCooldown;
                    }

                    if (newData.PowerUseCount != inPriorState.PriorUseCount)
                    {
                        entryChanged = true;
                        inPriorState.PriorUseCount = newData.PowerUseCount;
                    }

                    if (newData.Image != inPriorState.PriorUIIcon)
                    {
                        entryChanged = true;
                        inPriorState.PriorUIIcon = newData.Image;
                    }

                    var newActive = currentCompanion.CanUseCompanionPower();

                    if (newActive != inPriorState.PriorActive)
                    {
                        entryChanged = true;
                        inPriorState.PriorActive = newActive;
                    }
                }
            }

            return entryChanged;
        }

        // ICompanionSetInterface
        public void SetCompanion(ICompanionInterface inCompanion, ECompanionSlot inSlot)
        {
            if (inCompanion != null && !_companions.ContainsValue(inCompanion))
            {
                ClearCompanion(inSlot);

                _companions[inSlot] = inCompanion;
                inCompanion.SetLeader(gameObject);
            }
        }

        public void ClearCompanion(ECompanionSlot inSlot)
        {
            if (CompanionSlotOccupied(inSlot))
            {
                _companions[inSlot].ClearLeader();
                _companions[inSlot] = null;
            }
        }

        public void UseCompanionPower(ECompanionSlot inSlot)
        {
            if (CompanionSlotOccupied(inSlot) && _companions[inSlot].CanUseCompanionPower())
            {
                _companions[inSlot].UseCompanionPower();                
            }
        }

        public void RequestCompanionDialogue(ECompanionSlot inSlot)
        {
            if (CompanionSlotOccupied(inSlot))
            {
                _companions[inSlot].RequestDialogue();
            }
        }
        // ~ICompanionSetInterface

        private bool CompanionSlotOccupied(ECompanionSlot inSlot)
        {
            return _companions.ContainsKey(inSlot) && _companions[inSlot] != null;
        }
    }
}
