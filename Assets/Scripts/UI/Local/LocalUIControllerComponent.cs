// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Emote;
using Assets.Scripts.Messaging;
using Assets.Scripts.UnityLayer.GameObjects;
using UnityEngine;

namespace Assets.Scripts.UI.Local
{
    public class LocalUIControllerComponent 
        : MonoBehaviour
    {
        public GameObject LocalUIPrefab;
        public Vector3 LocalUIOffset;

        protected GameObject InstantiatedLocalUI;

        private UnityMessageEventHandle<EmoteStatusChangedMessage> _emoteStatusChangedHandle;

        protected void Awake()
        {
            InstantiatedLocalUI = Instantiate(LocalUIPrefab);

            InstantiatedLocalUI.gameObject.transform.parent = gameObject.transform;
            InstantiatedLocalUI.gameObject.transform.localPosition = LocalUIOffset;

            RegisterMessages();
        }
	
        protected void OnDestroy() 
        {
            UnregisterMessages();

            DestructionFunctions.DestroyGameObject(InstantiatedLocalUI);
        }

        private void RegisterMessages()
        {
            _emoteStatusChangedHandle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<EmoteStatusChangedMessage>(gameObject,
                    OnEmoteStatusChanged);
        }

        private void UnregisterMessages()
        {
            UnityMessageEventFunctions.UnregisterActionWithDispatcher(gameObject, _emoteStatusChangedHandle);   
        }

        private void OnEmoteStatusChanged(EmoteStatusChangedMessage message)
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(InstantiatedLocalUI, new EmoteStatusChangedUIMessage(message.State));
        }
    }
}
