// Copyright (C) Threetee Gang All Rights Reserved

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.Core;
using Assets.Scripts.Instance;
using Assets.Scripts.UnityLayer.Storage;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Services.Persistence
{
    public static class PersistenceFunctions 
    {
        public static void LoadCurrentSave(string savePath)
        {
            var saveData = InitializeSaveRead(savePath);

            var binaryFormatter = new BinaryFormatter();

            var levelToLoad = (string)binaryFormatter.Deserialize(saveData);
            GameInstance.CurrentInstance.LoadLevel(levelToLoad, saveData);
        }

        // Decrypts save data ready for other objects to read
        public static Stream InitializeSaveRead(string savePath)
        {
            using (var fileStream =
                File.Open(Application.persistentDataPath + savePath, FileMode.Open))
            {
                var decryptedStream = new MemoryStream(PersistantDataOperationFunctions.DecryptFileStream(fileStream, GameDataStorageConstants.AESKey,
                    GameDataStorageConstants.AESIV));

                fileStream.Close();

                return decryptedStream;
            }
        }

        public static void WriteCurrentSave(string savePath, IPersistenceServiceInterface persistenceService)
        {
            using (var writeStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();

                binaryFormatter.Serialize(writeStream, SceneManager.GetActiveScene().path);
                SaveData(writeStream, persistenceService);

                var encryptedStream = new MemoryStream(PersistantDataOperationFunctions.EncryptFileStream(new MemoryStream(writeStream.ToArray()), GameDataStorageConstants.AESKey,
                    GameDataStorageConstants.AESIV));

                using (var fileStream =
                    File.Open(Application.persistentDataPath + savePath, FileMode.OpenOrCreate))
                {
                    var bytesToCopy = encryptedStream.ToArray();
                    fileStream.Write(bytesToCopy, 0, bytesToCopy.Length);

                    fileStream.Close();
                }
            }
        }

        public static void SaveData(Stream data, IPersistenceServiceInterface persistenceService)
        {
            if (persistenceService == null)
            {
                return;
            }

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
