// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Services.Persistence
{
    public class PersistenceService 
        : IPersistenceServiceInterface
    {
        private readonly Dictionary<string, IPersistentEntityInterface> _entities = new Dictionary<string, IPersistentEntityInterface>();

        // IPersistenceServiceInterface
        public void RegisterPersistentEntity(string key, IPersistentEntityInterface entity)
        {
            if (entity != null)
            {
                _entities.Add(key, entity);
            }
        }

        public void UnregisterPersistentEntity(string key)
        {
            _entities.Remove(key);
        }

        public IPersistentEntityInterface GetEntity(string key)
        {
            if (_entities.ContainsKey(key))
            {
                return _entities[key];
            }

            Debug.LogError("Failed to find entity for " + key);

            return null;
        }
        // ~IPersistenceServiceInterface
    }
}
