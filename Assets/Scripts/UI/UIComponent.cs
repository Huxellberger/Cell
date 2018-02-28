// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Instance;
using Assets.Scripts.Messaging;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public abstract class UIComponent 
        : MonoBehaviour
    {
        public bool DisabledOnNotification = true;

        protected UnityMessageEventDispatcher Dispatcher { get; set; }

        private UnityMessageEventHandle<UpdateUIEnabledMessage> _updateUIEnabledHandle;
        private bool? _priorActiveState;

        protected void Start()
        {
            Dispatcher = GetUIDispatcher();

            RegisterMessages();

            OnStart();
        }

        private void RegisterMessages()
        {
            _updateUIEnabledHandle =
                Dispatcher.RegisterForMessageEvent<UpdateUIEnabledMessage>(OnUpdateUIEnabledMessage);
        }

        protected void OnDestroy()
        {
            OnEnd();

            UnregisterMessages();

            Dispatcher = null;
        }

        private void UnregisterMessages()
        {
            Dispatcher.UnregisterForMessageEvent(_updateUIEnabledHandle);
        }

        private void OnUpdateUIEnabledMessage(UpdateUIEnabledMessage inMessage)
        {
            if (DisabledOnNotification)
            {
                if (inMessage.IsEnabled)
                {
                    if (_priorActiveState != null)
                    {
                        gameObject.SetActive(_priorActiveState.Value);
                    }
                }
                else
                {
                    _priorActiveState = gameObject.activeSelf;
                    gameObject.SetActive(inMessage.IsEnabled);
                }
            }
        }

        protected abstract void OnStart();
        protected abstract void OnEnd();

        protected virtual UnityMessageEventDispatcher GetUIDispatcher()
        {
            return GameInstance.CurrentInstance.GetUIMessageDispatcher();
        }
    }
}
