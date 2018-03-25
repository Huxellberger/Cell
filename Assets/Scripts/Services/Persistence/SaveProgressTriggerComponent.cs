// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Character;
using Assets.Scripts.Messaging;
using Assets.Scripts.UnityLayer.Storage;
using UnityEngine;

namespace Assets.Scripts.Services.Persistence
{
    public class SaveProgressTriggerComponent 
        : MonoBehaviour
    {
        private readonly LazyServiceProvider<IPersistenceServiceInterface> _persistence 
            = new LazyServiceProvider<IPersistenceServiceInterface>();

        private void OnTriggerEnter2D(Collider2D inCollider)
        {
            if (inCollider != null && inCollider.gameObject != null)
            {
                OnGameObjectCollides(inCollider.gameObject);
            }
        }

        protected void OnGameObjectCollides(GameObject inCollidingObject)
        {
            if (inCollidingObject.GetComponent<CharacterComponent>() != null)
            {
                PersistenceFunctions.WriteCurrentSave(GameDataStorageConstants.SaveDataPath, _persistence.Get());
                UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(inCollidingObject, new SaveGameTriggerActivatedMessage());
            }
        }
    }
}
