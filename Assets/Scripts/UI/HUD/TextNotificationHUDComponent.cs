// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Messaging;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.HUD
{
    [RequireComponent(typeof(Image))]
    public class TextNotificationHUDComponent : UIComponent
    {
        private IList<string> _textNotifications;
        private Image _backingImage;
        private Text _text;

        private UnityMessageEventHandle<TextNotificationSentUIMessage> _textNotificationSentHandle;
        private UnityMessageEventHandle<TextNotificationClearedUIMessage> _textNotificationClearedHandle;

        protected override void OnStart()
        {
            _textNotifications = new List<string>();

            _backingImage = gameObject.GetComponent<Image>();
            _backingImage.enabled = false;

            _text = gameObject.GetComponentInChildren<Text>();
            _text.text = "";

            RegisterForMessages();
        }

        protected override void OnEnd()
        {
            UnregisterForMessages();

            _textNotifications.Clear();
            _text = null;
        }

        private void RegisterForMessages()
        {
            _textNotificationSentHandle =
                Dispatcher.RegisterForMessageEvent<TextNotificationSentUIMessage>(OnTextNotificationSent);

            _textNotificationClearedHandle =
                Dispatcher.RegisterForMessageEvent<TextNotificationClearedUIMessage>(OnTextNotificationCleared);
        }

        private void UnregisterForMessages()
        {
            Dispatcher.UnregisterForMessageEvent(_textNotificationClearedHandle);
            Dispatcher.UnregisterForMessageEvent(_textNotificationSentHandle);
        }

        private void UpdateTextElement()
        {
            if (_textNotifications.Count == 0)
            {
                _text.text = "";
                _backingImage.enabled = false;
            }
            else
            {
                _text.text = _textNotifications.Last();
                _backingImage.enabled = true;
            }
        }

        private void OnTextNotificationSent(TextNotificationSentUIMessage inMessage)
        {
            _textNotifications.Add(inMessage.Message);
            UpdateTextElement();
        }

        private void OnTextNotificationCleared(TextNotificationClearedUIMessage inMessage)
        {
            _textNotifications.RemoveAt(_textNotifications.Count -1);
            UpdateTextElement();
        }
    }
}
