// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Services.Persistence;

namespace Assets.Scripts.Test.Services.Persistence
{
    public class MockPersistenceService 
        : IPersistenceServiceInterface
    {
        public string RegisterPersistentEntityKey { get; private set; }
        public IPersistentEntityInterface RegisterPersistentEntityChosen { get; private set; }

        public string UnregisterPersistentEntityKey { get; private set; }

        public IPersistentEntityInterface GetEntityResult { get; set; }
        public string GetEntityKey { get; private set; }

        public void RegisterPersistentEntity(string key, IPersistentEntityInterface entity)
        {
            RegisterPersistentEntityKey = key;
            RegisterPersistentEntityChosen = entity;
        }

        public void UnregisterPersistentEntity(string key)
        {
            UnregisterPersistentEntityKey = key;
        }

        public IPersistentEntityInterface GetEntity(string key)
        {
            GetEntityKey = key;
            return GetEntityResult;
        }
    }
}

#endif // UNITY_EDITOR
