﻿// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.UnityEvent;
using UnityEngine.Events;

namespace Assets.Scripts.Messaging
{
    public class UnityMessageEventHandle<TMessageType>
        where TMessageType : UnityMessagePayload
    {
        public UnityMessageEventHandle()
        {
            _action = null;
        }

        public void RegisterHandle(UnityMessageEvent<TMessageType> inEvent, UnityAction<TMessageType> inAction)
        {
            if (inAction == null || inEvent == null)
            {
                throw new UnityMessageHandleException("Invalid action or event on registration!");
            }

            _action = inAction;
            inEvent.AddListener(inAction);
        }

        public void UnregisterHandle(UnityMessageEvent<TMessageType> inEvent)
        {
            if (inEvent == null)
            {
                throw new UnityMessageHandleException("Invalid event on unregistration!");
            }

            inEvent.RemoveListener(_action);
            _action = null;
        }

        public bool IsRegistered()
        {
            return _action != null;
        }

        private UnityAction<TMessageType> _action;
    }
}
