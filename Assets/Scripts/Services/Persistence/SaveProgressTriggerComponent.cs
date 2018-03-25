// Copyright (C) Threetee Gang All Rights Reserved

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.Components.Character;
using Assets.Scripts.Messaging;
using Assets.Scripts.UnityLayer.Storage;
using UnityEngine;

namespace Assets.Scripts.Services.Persistence
{
    public class SaveProgressTriggerComponent 
        : MonoBehaviour
        , IPersistentBehaviourInterface
    {
        private readonly LazyServiceProvider<IPersistenceServiceInterface> _persistence 
            = new LazyServiceProvider<IPersistenceServiceInterface>();

        private bool _previouslySaved = false;

        private void OnTriggerEnter2D(Collider2D inCollider)
        {
            if (inCollider != null && inCollider.gameObject != null)
            {
                OnGameObjectCollides(inCollider.gameObject);
            }
        }

        protected void OnGameObjectCollides(GameObject inCollidingObject)
        {
            if (inCollidingObject.GetComponent<CharacterComponent>() != null && !_previouslySaved)
            {
                _previouslySaved = true;
                PersistenceFunctions.WriteCurrentSave(GameDataStorageConstants.SaveDataPath, _persistence.Get());
                UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(inCollidingObject, new SaveGameTriggerActivatedMessage());
            }
        }

        // IPersistentBehaviourInterface
        public void WriteData(Stream stream)
        {
            var bf = new BinaryFormatter();

            bf.Serialize(stream, _previouslySaved);
        }

        public void ReadData(Stream stream)
        {
            var bf = new BinaryFormatter();

            _previouslySaved = (bool)bf.Deserialize(stream);
        }
        // ~IPersistentBehaviourInterface
    }
}
