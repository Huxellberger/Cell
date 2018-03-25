// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;

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
            _entities[key] = null;
        }

        public Dictionary<string, IPersistentEntityInterface> GetEntities()
        {
            return _entities;
        }
        // ~IPersistenceServiceInterface
    }
}
