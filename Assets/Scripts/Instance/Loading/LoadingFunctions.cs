// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Instance.Loading
{
    public static class LoadingFunctions
    {
        public static void LoadScene(string inSceneName)
        {
            if (Application.isPlaying)
            {
                SceneManager.LoadScene(inSceneName);
            }
        }

        public static AsyncOperation LoadSceneAsync(string inScene)
        {
            if (Application.isPlaying)
            {
                return SceneManager.LoadSceneAsync(inScene);
            }

            return new AsyncOperation();
        }
    }
}
