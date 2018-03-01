// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.Components.Emote;
using Assets.Scripts.Messaging;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Local
{
    [System.Serializable]
    public class EmoteGraphicEntry
    {
        public EEmoteState State;
        public Image DisplayImage;
    }

    public class EmoteLocalUIElementComponent 
        : LocalUIElementComponent
    {
        public List<EmoteGraphicEntry> EmoteGraphics;

        private Dictionary<EEmoteState, Image> _emoteMappings;
        private UnityMessageEventHandle<EmoteStatusChangedUIMessage> _emoteStateChangedHandle;
        private EEmoteState _currentState;

        protected override void OnElementInitialisedImpl()
        {
            _currentState = EEmoteState.None;

            SetMappings();
            RegisterMessages();
        }

        protected override void OnElementUninitialisedImpl()
        {
            UnregisterMessages();
            ClearMappings();
        }

        private void SetMappings()
        {
            _emoteMappings = new Dictionary<EEmoteState, Image>();

            foreach (var emoteGraphic in EmoteGraphics)
            {
                _emoteMappings.Add(emoteGraphic.State, emoteGraphic.DisplayImage);
                emoteGraphic.DisplayImage.gameObject.SetActive(false);
            }
        }

        private void ClearMappings()
        {
            _emoteMappings.Clear();
        }

        private void RegisterMessages()
        {
            _emoteStateChangedHandle = Dispatcher.RegisterForMessageEvent<EmoteStatusChangedUIMessage>(OnEmoteStateChanged);
        }

        private void UnregisterMessages()
        {
            Dispatcher.UnregisterForMessageEvent(_emoteStateChangedHandle);
        }

        private void OnEmoteStateChanged(EmoteStatusChangedUIMessage inMessage)
        {
            _emoteMappings[_currentState].gameObject.SetActive(false);

            _currentState = inMessage.State;

            if (_currentState != EEmoteState.None)
            {
                _emoteMappings[_currentState].gameObject.SetActive(true);
            }
        }
    }
}