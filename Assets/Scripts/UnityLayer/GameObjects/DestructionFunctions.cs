// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.UnityLayer.GameObjects
{
    public static class DestructionFunctions
    {
        public static void DestroyGameObject(GameObject inGameObject)
        {
            if (Application.isPlaying)
            {
                Object.Destroy(inGameObject);
            }
            else
            {
                Object.DestroyImmediate(inGameObject);
            }
        }

        public static void DontDestroyOnLoadGameObject(GameObject inGameObject)
        {
            if (Application.isPlaying)
            {
                Object.DontDestroyOnLoad(inGameObject);
            }
        }
    }
}
