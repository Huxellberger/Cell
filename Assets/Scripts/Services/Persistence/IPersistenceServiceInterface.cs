// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;

namespace Assets.Scripts.Services.Persistence
{
    public interface IPersistenceServiceInterface
    {
        void RegisterPersistentEntity(string key, IPersistentEntityInterface entity);
        void UnregisterPersistentEntity(string key);
        Dictionary<string, IPersistentEntityInterface> GetEntities();
    }
}
