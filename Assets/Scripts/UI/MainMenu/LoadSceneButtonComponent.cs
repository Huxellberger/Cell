// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Instance;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MainMenu
{
    [RequireComponent(typeof(Button))]
    public class LoadSceneButtonComponent 
        : MonoBehaviour
    {
        public string LevelToLoad;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnButtonPressed);
        }

        protected void OnButtonPressed()
        {
            GameInstance.CurrentInstance.LoadLevel(LevelToLoad, null);
        }
    }
}
