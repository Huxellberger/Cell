// Copyright (C) Threetee Gang All Rights Reserved

namespace Assets.Scripts.Services.Persistence
{
    public interface IPersistenceServiceInterface
    {
        void RegisterPersistentEntity(string key, IPersistentEntityInterface entity);
        void UnregisterPersistentEntity(string key);
        IPersistentEntityInterface GetEntity(string key);
    }
}
