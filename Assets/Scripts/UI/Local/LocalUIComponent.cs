// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.Messaging;
using UnityEngine;

namespace Assets.Scripts.UI.Local
{
    [RequireComponent(typeof(IUnityMessageEventInterface))]
    public class LocalUIComponent 
        : MonoBehaviour
    {
        private IUnityMessageEventInterface _messageEventInterface;
        private LocalUIElementComponent[] _uiElements;

        protected void Awake()
        {
            _messageEventInterface = gameObject.GetComponent<IUnityMessageEventInterface>();

            _uiElements = GetComponentsInChildren<LocalUIElementComponent>();

            foreach (var uiElement in _uiElements)
            {
                uiElement.OnElementInitialised(_messageEventInterface.GetUnityMessageEventDispatcher());
            }
        }

        protected void OnDestroy()
        {
            foreach (var uiElement in _uiElements)
            {
                uiElement.OnElementUninitialised();
            }

            _messageEventInterface = null;
        }
    }
}
