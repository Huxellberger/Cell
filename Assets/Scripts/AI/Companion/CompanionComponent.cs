// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.AI.Chatter;
using Assets.Scripts.Instance;
using Assets.Scripts.Messaging;
using Assets.Scripts.UI.HUD.Conversation;
using UnityEngine;

namespace Assets.Scripts.AI.Companion
{
    public abstract class CompanionComponent 
        : MonoBehaviour 
        , ICompanionInterface
    {
        public DialogueData DialogueEntries;
        public Sprite CompanionUIIcon;

        public string DefaultDialogueEntry;
        public float PowerCooldownTime = 2.0f;
        public int MaxPowerCharges = 3;
        
        protected GameObject Leader { get; private set; }

        private Dictionary<string, DialogueEntry> _dialogueMappings;
        private UnityMessageEventDispatcher _uiDispatcher;
        private CompanionData _currentData;
        private float _cooldownTimeRemaining = 0.0f;
        private float CooldownTimeRemaining
        {
            get { return _cooldownTimeRemaining; }
            set
            {
                _cooldownTimeRemaining = value;
                if (_currentData != null)
                {
                    _currentData.PowerCooldown = GetCompanionPowerCooldown();
                }
            } 
        }

        protected virtual void Start()
        {
            _uiDispatcher = GameInstance.CurrentInstance.GetUIMessageDispatcher();

            _dialogueMappings = DialogueData.GenerateDialogueMappings(DialogueEntries);
            _currentData = new CompanionData{Image = CompanionUIIcon, PowerCooldown = GetCompanionPowerCooldown(), PowerUseCount = MaxPowerCharges};
        }

        protected void Update()
        {
            if (CooldownTimeRemaining > 0.0f)
            {
                CooldownTimeRemaining -= GetDeltaTime();
            }
        }

        protected virtual float GetDeltaTime()
        {
            return Time.deltaTime;
        }

        private float GetCompanionPowerCooldown()
        {
            if (IsPowerCooledDown())
            {
                return 1.0f;
            }

            return (PowerCooldownTime - CooldownTimeRemaining) / PowerCooldownTime;
        }

        // ICompanionInterface
        public CompanionData GetCompanionData()
        {
            return _currentData;
        }
        
        public bool CanUseCompanionPower()
        {
            if (Leader != null && IsPowerCooledDown() && PowerHasChargesRemaining())
            {
                return CanUseCompanionPowerImpl();
            }

            return false;
        }

        private bool IsPowerCooledDown()
        {
            return CooldownTimeRemaining <= 0.0f;
        }

        private bool PowerHasChargesRemaining()
        {
            return _currentData.PowerUseCount == CompanionConstants.UnlimitedCharges || _currentData.PowerUseCount > 0;
        }

        public void UseCompanionPower()
        {
            if (CanUseCompanionPower())
            {
                CompanionPowerImpl();
                CooldownTimeRemaining = PowerCooldownTime;
                _currentData.PowerUseCount--;
            }
        }

        public void RequestDialogue()
        {
            if (_dialogueMappings.ContainsKey(DefaultDialogueEntry))
            {
                var dialogueLines = _dialogueMappings[DefaultDialogueEntry];
                _uiDispatcher.InvokeMessageEvent(new RequestDialogueUIMessage(dialogueLines.Lines, 0, null));
            }
        }

        public void SetLeader(GameObject inLeader)
        {
            ClearLeader();

            if (inLeader != null)
            {
                Leader = inLeader;

                OnLeaderSetImpl();
            }
        }

        public void ClearLeader()
        {
            if (Leader != null)
            {
                OnLeaderClearedImpl();

                Leader = null;
            }
        }
        // ~ICompanionInterface

        protected abstract bool CanUseCompanionPowerImpl();
        protected abstract void CompanionPowerImpl();
        protected abstract void OnLeaderSetImpl();
        protected abstract void OnLeaderClearedImpl();
    }
}
