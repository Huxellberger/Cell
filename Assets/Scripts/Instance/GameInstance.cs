// Copyright (C) Threetee Gang All Rights Reserved

using System.Linq;
using Assets.Scripts.Input;
using Assets.Scripts.Instance.Loading;
using Assets.Scripts.Messaging;
using Assets.Scripts.UnityLayer.GameObjects;
using Assets.Scripts.UnityLayer.Input;
using Assets.Scripts.UnityLayer.Storage;
using UnityEngine;

namespace Assets.Scripts.Instance
{
    [RequireComponent(typeof(IInputInterface))]
    public class GameInstance
        : MonoBehaviour
    {
        private static GameInstance _currentInstance;
        public static GameInstance CurrentInstance
        {
            get { return _currentInstance; }
            private set
            {
                if (_currentInstance == null || value == null)
                {
                    _currentInstance = value;
                }
                else
                {
                    Debug.LogError("Found existing GameInstance!");
                }
            }
        }

        public string NextSceneToLoad { get; private set; }

        private UnityMessageEventDispatcher _uiDispatcher;

        public static void ClearGameInstance()
        {
            CurrentInstance = null;
        }

        protected virtual void Awake()
        {
            CurrentInstance = this;
            _uiDispatcher = new UnityMessageEventDispatcher();
            DestructionFunctions.DontDestroyOnLoadGameObject(gameObject);

            InitializeInput();
        }

        public UnityMessageEventDispatcher GetUIMessageDispatcher()
        {
            return _uiDispatcher;
        }

        public void LoadLevel(string inLevelName)
        {
            NextSceneToLoad = inLevelName;
            LoadingFunctions.LoadScene(LoadingConstants.LoadingScreenSceneName);
        }

        private void InitializeInput()
        {
            var input = GetComponent<IInputInterface>();

            var rawInputs = DefaultTranslatedInputRepository.GetDefaultMappings()
                .Select(defaultInput => defaultInput.Key).ToList();

            input.SetInputMappingProvider(new DefaultInputMappingProvider(rawInputs, new DefaultTranslatedInputRepository(new PlayerPrefsRepository())));
            input.SetUnityInputInterface(new DefaultUnityInput());
        }
    }
}
