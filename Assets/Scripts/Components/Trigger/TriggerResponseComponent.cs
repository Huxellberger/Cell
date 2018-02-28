// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Messaging;
using UnityEngine;

namespace Assets.Scripts.Components.Trigger
{
    public abstract class TriggerResponseComponent
        : MonoBehaviour
    {
        public GameObject TriggerObject;
        public bool MultiTrigger = false;

        private UnityMessageEventHandle<TriggerMessage> _triggerMessageHandle;
        private UnityMessageEventHandle<CancelTriggerMessage> _cancelTriggerMessageHandle;

        private bool _previouslyTriggered = false;

        protected virtual void Start()
        {
            if (TriggerObject != null)
            {
                RegisterForTriggerMessages();
            }
            else
            {
                Debug.LogError("Cannot register as trigger not set!");
            }
        }

        protected void OnDestroy()
        {
            UnregisterForTriggerMessages();
        }

        private void RegisterForTriggerMessages()
        {
            _triggerMessageHandle = UnityMessageEventFunctions.RegisterActionWithDispatcher<TriggerMessage>
                (TriggerObject, OnTrigger);

            _cancelTriggerMessageHandle = UnityMessageEventFunctions.RegisterActionWithDispatcher<CancelTriggerMessage>
                (TriggerObject, OnCancelTrigger);
        }

        private void UnregisterForTriggerMessages()
        {
            if (TriggerObject != null)
            {
                UnityMessageEventFunctions.UnregisterActionWithDispatcher(TriggerObject, _cancelTriggerMessageHandle);
                UnityMessageEventFunctions.UnregisterActionWithDispatcher(TriggerObject, _triggerMessageHandle);
            }
        }

        private void OnTrigger(TriggerMessage inMessage)
        {
            if (MultiTrigger || !_previouslyTriggered)
            {
                OnTriggerImpl(inMessage);
            }

            _previouslyTriggered = true;
        }

        private void OnCancelTrigger(CancelTriggerMessage inMessage)
        {
            if (MultiTrigger)
            {
                OnCancelTriggerImpl(inMessage);
            }
        }

        protected abstract void OnTriggerImpl(TriggerMessage inMessage);
        protected abstract void OnCancelTriggerImpl(CancelTriggerMessage inMessage);
    }
}
