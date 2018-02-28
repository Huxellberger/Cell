// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections;
using Assets.Scripts.Messaging;
using UnityEngine;

namespace Assets.Scripts.Instance.Loading
{
    public class LevelLoaderComponent 
        : MonoBehaviour
    {
        public float LoadDelay = 1.5f;

        private string _levelName;
        private UnityMessageEventDispatcher _uiDispatcher;

        private void Start ()
        {
            Cursor.visible = false;

            var instance = GameInstance.CurrentInstance;

            _levelName = instance.NextSceneToLoad;
            _uiDispatcher = instance.GetUIMessageDispatcher();

            StartCoroutine(LoadNextLevel());
        }

        private void OnDestroy()
        {
            Cursor.visible = true;

            _uiDispatcher = null;
        }

        private IEnumerator LoadNextLevel()
        {
            // So it doesn't insta load
            yield return new WaitForSeconds(LoadDelay);

            var result = LoadingFunctions.LoadSceneAsync(_levelName);
            result.allowSceneActivation = false;

            while (result.progress < 0.9f)
            {
                _uiDispatcher.InvokeMessageEvent(new LoadingProgressUpdatedUIMessage(result.progress));
                yield return null;
            }

            _uiDispatcher.InvokeMessageEvent(new LoadingProgressUpdatedUIMessage(result.progress));

            yield return new WaitForSeconds(LoadDelay);
            
            result.allowSceneActivation = true;
        }
    }
}
