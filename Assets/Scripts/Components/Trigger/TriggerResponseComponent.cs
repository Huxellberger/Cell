// Copyright (C) Threetee Gang All Rights Reserved

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.Messaging;
using Assets.Scripts.Services.Persistence;
using UnityEngine;

namespace Assets.Scripts.Components.Trigger
{
    public abstract class TriggerResponseComponent
        : MonoBehaviour
        , IPersistentBehaviourInterface
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
        protected virtual void OnWriteData(Stream stream) { }
        protected virtual void OnReadData(Stream stream) { }

        // IPersistentBehaviourInterface
        public void WriteData(Stream stream)
        {
            var bf = new BinaryFormatter();

            bf.Serialize(stream, _previouslyTriggered);

            if (!MultiTrigger && _previouslyTriggered)
            {
                OnWriteData(stream);
            }
        }

        public void ReadData(Stream stream)
        {
            var bf = new BinaryFormatter();

            _previouslyTriggered = (bool)bf.Deserialize(stream);

            if (!MultiTrigger && _previouslyTriggered)
            {
                OnReadData(stream);
            }
        }
        // ~IPersistentBehaviourInterface
    }
}
