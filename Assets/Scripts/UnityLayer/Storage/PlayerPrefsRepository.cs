// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.UnityLayer.Storage
{
    public class PlayerPrefsRepository 
        : IPlayerPrefsRepositoryInterface 
    {
        public void SetKey<TValueType>(string key, TValueType inValue)
        {
            PlayerPrefs.SetString(key, inValue.ToString());
        }

        public string GetValueForKey(string key)
        {
            return PlayerPrefs.GetString(key, null);
        }

        public void Save()
        {
            PlayerPrefs.Save();
        }

        public void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
