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

        public string DefaultDialogueEntry;
        public float PowerCooldownTime = 2.0f;
        
        protected GameObject Leader { get; private set; }

        private Dictionary<string, DialogueEntry> _dialogueMappings;
        private UnityMessageEventDispatcher _uiDispatcher;
        private float _cooldownTimeRemaining = 0.0f;

        protected void Start()
        {
            _uiDispatcher = GameInstance.CurrentInstance.GetUIMessageDispatcher();

            _dialogueMappings = DialogueData.GenerateDialogueMappings(DialogueEntries);
        }

        protected void Update()
        {
            if (_cooldownTimeRemaining > 0.0f)
            {
                _cooldownTimeRemaining -= GetDeltaTime();
            }
        }

        protected virtual float GetDeltaTime()
        {
            return Time.deltaTime;
        }

        // ICompanionInterface
        public float GetCompanionPowerCooldown()
        {
            if (IsPowerCooledDown())
            {
                return 1.0f;
            }

            return (PowerCooldownTime - _cooldownTimeRemaining) / PowerCooldownTime;
        }

        public bool CanUseCompanionPower()
        {
            if (Leader != null && IsPowerCooledDown())
            {
                return CanUseCompanionPowerImpl();
            }

            return false;
        }

        private bool IsPowerCooledDown()
        {
            return _cooldownTimeRemaining <= 0.0f;
        }

        public void UseCompanionPower()
        {
            if (CanUseCompanionPower())
            {
                CompanionPowerImpl();
                _cooldownTimeRemaining = PowerCooldownTime;
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
