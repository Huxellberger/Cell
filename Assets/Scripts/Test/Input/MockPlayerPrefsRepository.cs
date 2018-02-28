// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.UnityLayer.Storage;

namespace Assets.Scripts.Test.Input
{
    public class MockPlayerPrefsRepository 
        : IPlayerPrefsRepositoryInterface
    {
        public object SetKeyResult { get; private set; }
        public string SetKeyString { get; private set; }

        public string GetValueForKeyResult { get; set; }
        public string GetValueForKeySentKey { get; private set; }

        public bool SaveCalled = false;
        public bool DeleteAllCalled = false;

        public void SetKey<TValueType>(string key, TValueType inValue)
        {
            SetKeyString = key;
            SetKeyResult = inValue;
        }

        public string GetValueForKey(string key)
        {
            GetValueForKeySentKey = key;
            return GetValueForKeyResult;
        }

        public void Save()
        {
            SaveCalled = true;
        }

        public void DeleteAll()
        {
            DeleteAllCalled = true;
        }
    }
}

#endif // UNITY_EDITOR
