// Copyright (C) Threetee Gang All Rights Reserved

namespace Assets.Scripts.UnityLayer.Storage
{
    public interface IPlayerPrefsRepositoryInterface
    {
        void SetKey<TValueType>(string key, TValueType inValue);

        string GetValueForKey(string key);

        void Save();
        void DeleteAll();
    }
}
