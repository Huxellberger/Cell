// Copyright (C) Threetee Gang All Rights Reserved

using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.Instance;
using Assets.Scripts.Services.Persistence;
using Assets.Scripts.UnityLayer.Storage;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MainMenu
{
    [RequireComponent(typeof(Button))]
    public class LoadSaveButtonComponent
        : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnButtonPressed);
        }

        protected void OnButtonPressed()
        {
            var saveData = PersistenceFunctions.InitializeSaveRead(GameDataStorageConstants.SaveDataPath);

            var binaryFormatter = new BinaryFormatter();

            var levelToLoad = (string)binaryFormatter.Deserialize(saveData);
            GameInstance.CurrentInstance.LoadLevel(levelToLoad, saveData);
        }
    }
}
