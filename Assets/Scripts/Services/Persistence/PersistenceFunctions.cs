// Copyright (C) Threetee Gang All Rights Reserved

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Assets.Scripts.Services.Persistence
{
    public static class PersistenceFunctions 
    {
        public static void SaveData(Stream data, IPersistenceServiceInterface persistenceService)
        {
            var binaryFormatter = new BinaryFormatter();

            var persistentEntities = persistenceService.GetEntities();

            binaryFormatter.Serialize(data, persistentEntities.Count);

            foreach (var persistentEntity in persistentEntities)
            {
                binaryFormatter.Serialize(data, persistentEntity.Key);
                var destroyed = persistentEntity.Value == null;
                binaryFormatter.Serialize(data, destroyed);

                if (!destroyed)
                {
                    persistentEntity.Value.WriteData(data);
                }
            }
        }

        public static void LoadData(Stream data, IPersistenceServiceInterface persistenceService)
        {
            var binaryFormatter = new BinaryFormatter();

            var persistentEntities = persistenceService.GetEntities();

            var entryCount = (int) binaryFormatter.Deserialize(data);

            for (var currentEntryCount = 0; currentEntryCount < entryCount; currentEntryCount++)
            {
                var entityName = (string) binaryFormatter.Deserialize(data);
                var wasDestroyed = (bool) binaryFormatter.Deserialize(data);

                if (persistentEntities.ContainsKey(entityName))
                {
                    if (persistentEntities[entityName] != null)
                    {
                        persistentEntities[entityName].ReadData(data, wasDestroyed);
                    }
                }
                else
                {
                    UnityEngine.Debug.LogError("Failed to find entry for key " + entityName);
                }
            }
        }
    }
}
