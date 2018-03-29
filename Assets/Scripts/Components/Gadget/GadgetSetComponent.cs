// Copyright (C) Threetee Gang All Rights Reserved

using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Messaging;
using UnityEngine;

namespace Assets.Scripts.Components.Gadget
{
    [Serializable]
    public struct GadgetSlotCapcity
    {
        public EGadgetSlot Slot;
        public int Capacity;
    }

    public class GadgetSlot
    {
        public readonly List<IGadgetInterface> Gadgets;

        public readonly EGadgetSlot Slot;
        public int Capacity;

        public GadgetSlot(EGadgetSlot inSlot, int inCapacity)
        {
            Slot = inSlot;
            Capacity = inCapacity;

            Gadgets = new List<IGadgetInterface>(Capacity);
        }
    }

    public class GadgetSetComponent 
        : MonoBehaviour 
        , IGadgetSetInterface
    {
        public List<GadgetSlotCapcity> InitialCapacities = new List<GadgetSlotCapcity>();

        public readonly Dictionary<EGadgetSlot, int> _slotCapacities = new Dictionary<EGadgetSlot, int>();
        private readonly List<GadgetSlot> _gadgetSlots = new List<GadgetSlot>();
        private int _activeSlotIndex = 0;

        protected void Awake()
        {
            foreach (var initialCapacity in InitialCapacities)
            {
                _slotCapacities.Add(initialCapacity.Slot, initialCapacity.Capacity);
            }
        }

        // IGadgetSetInterface
        public bool CanAddGadget(IGadgetInterface gadget)
        {
            if (gadget == null)
            {
                return false;
            }

            return HasSlotSpace(gadget.GetGadgetSlot());
        }

        public void AddGadget(IGadgetInterface gadget)
        {
            if (CanAddGadget(gadget))
            {
                if (_gadgetSlots.Exists((value) => value.Slot == gadget.GetGadgetSlot()))
                {
                    var slot = _gadgetSlots.Find((value) => value.Slot == gadget.GetGadgetSlot());
                    slot.Gadgets.Add(gadget);
                }
                else
                {
                    var newSlot = new GadgetSlot(gadget.GetGadgetSlot(), _slotCapacities[gadget.GetGadgetSlot()]);
                    newSlot.Gadgets.Add(gadget);
                    _gadgetSlots.Add(newSlot);
                }

                OnGadgetSlotUpdated();
            }
        }

        private void OnGadgetSlotUpdated()
        {
            if (_gadgetSlots.Count > 0)
            {
                UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(gameObject, new GadgetUpdatedMessage(_gadgetSlots[_activeSlotIndex].Gadgets.Last()));
            }
            else
            {
                UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(gameObject, new GadgetUpdatedMessage(null));
            }
        }

        public void UseActiveGadget()
        {
            if (_gadgetSlots.Count == 0)
            {
                return;
            }

            var activeSlot = _gadgetSlots[_activeSlotIndex];
            var usedIndex = activeSlot.Gadgets.Count - 1;

            activeSlot.Gadgets[usedIndex].UseGadget(gameObject);
            activeSlot.Gadgets.RemoveAt(usedIndex);

            if (activeSlot.Gadgets.Count == 0)
            {
                _gadgetSlots.Remove(activeSlot);
            }

            CycleActiveGadget(0);
            OnGadgetSlotUpdated();
        }

        public void CycleActiveGadget(int cycleAmount)
        {
            var initialSlot = _activeSlotIndex;
            _activeSlotIndex += cycleAmount;

            // Cycle round
            if (_activeSlotIndex > _gadgetSlots.Count - 1)
            {
                _activeSlotIndex = 0;
            }
            else if (_activeSlotIndex < 0)
            {
                _activeSlotIndex = _gadgetSlots.Count - 1;
            }

            if (initialSlot != _activeSlotIndex)
            {
                OnGadgetSlotUpdated();
            }
        }
        // ~IGadgetSetInterface

        private bool HasSlotSpace(EGadgetSlot inSlotId)
        {
            // Need to have a capacity to carry
            if (_slotCapacities.ContainsKey(inSlotId) && _slotCapacities[inSlotId] > 0)
            {
                // No slot would mean it's empty (remove slots on empty)
                if (_gadgetSlots.Exists((value) => value.Slot == inSlotId))
                {
                    var slot = _gadgetSlots.Find((value) => value.Slot == inSlotId);
                    return slot.Gadgets.Count < slot.Capacity;
                }

                return true;
            }

            return false;
        }
    }
}
