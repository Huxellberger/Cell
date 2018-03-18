// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.Instance;
using Assets.Scripts.Messaging;
using Assets.Scripts.Services;
using Assets.Scripts.Services.EventsOfInterest;
using Assets.Scripts.UI.HUD.Conversation;
using UnityEngine;

namespace Assets.Scripts.AI.Chatter
{
    public class ChatterComponent 
        : MonoBehaviour
    {
        public DialogueData ChatterData;

        private Dictionary<string, DialogueEntry> _chatterEntries;
        private readonly LazyServiceProvider<IEventsOfInterestServiceInterface> _eventsOfInterestService 
            = new LazyServiceProvider<IEventsOfInterestServiceInterface>();

        private List<EventOfInterestRegistration> _registrations;

        private UnityMessageEventDispatcher _uiDispatcher;

        protected void Start() 
        {
            InitialiseChatterData();
            RegisterForEventsOfInterest();
            _uiDispatcher = GameInstance.CurrentInstance.GetUIMessageDispatcher();
        }

        private void InitialiseChatterData()
        {
            if (ChatterData != null)
            {
                _chatterEntries = DialogueData.GenerateDialogueMappings(ChatterData);
            }
            else
            {
                Debug.LogWarning("Dialogue Chatter data should probably be set for " + gameObject.name);
            }
        }

        private void RegisterForEventsOfInterest()
        {
            var eventsOfInterest = _eventsOfInterestService.Get();
            _registrations = new List<EventOfInterestRegistration>(_chatterEntries.Count);
            foreach (var chatterEntry in _chatterEntries)
            {
                var registration = new EventOfInterestRegistration(chatterEntry.Key, OnEventOfInterest, chatterEntry.Value.ChatterType);
                eventsOfInterest.ListenForEventOfInterest(registration);
                _registrations.Add(registration);
            }
        }
	
        protected void OnDestroy() 
        {
		    UnregisterForEventsOfInterest();
        }

        private void UnregisterForEventsOfInterest()
        {
            var eventsOfInterest = _eventsOfInterestService.Get();
            foreach (var registration in _registrations)
            {
                eventsOfInterest.StopListeningForEventOfInterest(registration);
            }
            _registrations.Clear();
        }

        private void OnEventOfInterest(string inKey)
        {
            if (_chatterEntries.ContainsKey(inKey))
            {
                var entry = _chatterEntries[inKey];
                _uiDispatcher.InvokeMessageEvent(new RequestDialogueUIMessage(entry.Lines, entry.Priority, OnDialogueComplete));
            }
        }

        private void OnDialogueComplete()
        {
            // Potential callback for journal entries, reputation boosts etc.
        }
    }
}
