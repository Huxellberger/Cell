// Copyright (C) Threetee Gang All Rights Reserved

using System;
using System.Collections.Generic;
using Assets.Scripts.AI.Companion;
using Assets.Scripts.Messaging;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.HUD
{
    public class CompanionStatusHUDComponent 
        : UIComponent
    {
        [Serializable]
        public class CompanionStatus
        {
            public Slider CompanionSlider;
            public Image CompanionSliderFill;
            public Text CompanionUseCountText;
        }

        [Serializable]
        public class CompanionStatusPairing
        {
            public ECompanionSlot Slot;
            public CompanionStatus Status;
        }

        public List<CompanionStatusPairing> CompanionStates = new List<CompanionStatusPairing>();
        public float IconDisableAlpha = 0.2f;

        private readonly Dictionary<ECompanionSlot, CompanionStatus> _companionSliders = new Dictionary<ECompanionSlot, CompanionStatus>();
        private UnityMessageEventHandle<CompanionSlotsUpdatedUIMessage> _slotsUpdatedHandle;

        protected override void OnStart()
        {
            foreach (var companionStatus in CompanionStates)
            {
                _companionSliders.Add(companionStatus.Slot, companionStatus.Status);
            }

            _slotsUpdatedHandle = Dispatcher.RegisterForMessageEvent<CompanionSlotsUpdatedUIMessage>(OnSlotsUpdated);
        }

        protected override void OnEnd()
        {
            Dispatcher.UnregisterForMessageEvent(_slotsUpdatedHandle);
        }

        private void OnSlotsUpdated(CompanionSlotsUpdatedUIMessage inMessage)
        {
            foreach (var slotPairing in inMessage.Updates)
            {
                if (_companionSliders.ContainsKey(slotPairing.Key))
                {
                    UpdateCompanionStatus(_companionSliders[slotPairing.Key], slotPairing.Value);
                }
            }
        }

        private void UpdateCompanionStatus(CompanionStatus inStatus, PriorCompanionSlotState inState)
        {
            // Image
            inStatus.CompanionSliderFill.sprite = inState.PriorUIIcon;

            // Slider value
            inStatus.CompanionSlider.value = inState.PriorCooldown;

            // Use Count
            if (inState.PriorUseCount == CompanionConstants.UnlimitedCharges)
            {
                inStatus.CompanionUseCountText.text = "";
            }
            else
            {
                inStatus.CompanionUseCountText.text = inState.PriorUseCount.ToString();

                inStatus.CompanionUseCountText.color = inState.PriorUseCount == 0 ? Color.red : Color.white;
            }

            // Active
            inStatus.CompanionSliderFill.CrossFadeAlpha(inState.PriorActive ? 1.0f : IconDisableAlpha, 0.0f, true);
            inStatus.CompanionSliderFill.type = Image.Type.Filled;
            inStatus.CompanionSliderFill.fillMethod = Image.FillMethod.Vertical;
        }
    }
}
