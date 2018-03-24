// Copyright (C) Threetee Gang All Rights Reserved

using System.IO;
using Assets.Scripts.Core;
using Assets.Scripts.Instance;
using Assets.Scripts.UnityLayer.Storage;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MainMenu
{
    [RequireComponent(typeof(Button))]
    public class LoadSceneButtonComponent 
        : MonoBehaviour
    {
        public string LevelToLoad;
        public bool UseSaveData = false;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnButtonPressed);
        }

        protected void OnButtonPressed()
        {
            Stream saveData = null;

            if (UseSaveData)
            {
                var fileStream = File.Open(Application.persistentDataPath + GameDataStorageConstants.SaveDataPath, FileMode.Open);
                saveData = new MemoryStream(PersistantDataOperationFunctions.DecryptFileStream(fileStream, GameDataStorageConstants.AESKey, GameDataStorageConstants.AESIV));
                fileStream.Close();
            }

            GameInstance.CurrentInstance.LoadLevel(LevelToLoad, saveData);
        }
    }
}
