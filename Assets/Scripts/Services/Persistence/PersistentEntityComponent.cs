// Copyright (C) Threetee Gang All Rights Reserved

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.Core;
using Assets.Scripts.UnityLayer.GameObjects;
using UnityEngine;

namespace Assets.Scripts.Services.Persistence
{
    public class PersistentEntityComponent 
        : MonoBehaviour 
        , IPersistentEntityInterface
    {
        private readonly LazyServiceProvider<IPersistenceServiceInterface> _persistence = new LazyServiceProvider<IPersistenceServiceInterface>();

        protected void Awake() 
        {
            _persistence.Get().RegisterPersistentEntity(gameObject.name, this);
        }
	
        protected void OnDestroy() 
        {
            _persistence.Get().UnregisterPersistentEntity(gameObject.name);
        }

        public void WriteData(Stream stream)
        {
            var bf = new BinaryFormatter();

            bf.Serialize(stream, gameObject.activeSelf);
            bf.Serialize(stream, new Vector3Serializer().Fill(gameObject.transform.position));
            bf.Serialize(stream, new Vector3Serializer().Fill(gameObject.transform.eulerAngles));
        }

        public void ReadData(Stream stream, bool previouslyDestroyed)
        {
            var bf = new BinaryFormatter();

            if (previouslyDestroyed)
            {
                DestructionFunctions.DestroyGameObject(gameObject);
            }
            else
            {
                gameObject.SetActive((bool)bf.Deserialize(stream));
                gameObject.transform.position = ((Vector3Serializer)bf.Deserialize(stream)).AsVector;
                gameObject.transform.eulerAngles = ((Vector3Serializer)bf.Deserialize(stream)).AsVector;
            }
        }
    }
}
