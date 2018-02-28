// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.Core;
using Assets.Scripts.UnityLayer.Storage;
using UnityEngine;

namespace Assets.Scripts.Localisation
{
    public static class LocalisationComponentConstants
    {
        public static readonly string LanguageSettingKey = "LanguageSetting";
    }

    public class LocalisationComponent 
        : MonoBehaviour
        , ILocalisationInterface
    {
        public LocalisedTextDatabase LocalisedDatabase;

        protected IPlayerPrefsRepositoryInterface PlayerPrefsRepo;

        private IDictionary<LocalisationKey, LocalisedText> _localisationDictionary;

        protected void Awake ()
        {
            _localisationDictionary = LocalisedDatabase.ToLocalisationMap();

            LocalisationManager.CurrentLocalisationInterface = this;

            SetPlayerPrefsRepo();

            InitialiseLanguage();
        }

        protected virtual void SetPlayerPrefsRepo()
        {
            PlayerPrefsRepo = new PlayerPrefsRepository();

            PlayerPrefsRepo.Save();
        }

        private void InitialiseLanguage()
        {
            var languageSetting = EnumExtensions.TryParse<ELanguageOptions>(PlayerPrefsRepo.GetValueForKey(LocalisationComponentConstants.LanguageSettingKey));

            if (languageSetting.IsSet())
            {
                UpdateLanguageSetting(languageSetting.Get());
            }
        }

        protected void OnDestroy()
        {
            LocalisationManager.CurrentLocalisationInterface = null;
        }

        // ILocalisationInterface
        public LocalisedText GetTextForLocalisationKey(LocalisationKey inKey)
        {
            if (!_localisationDictionary.ContainsKey(inKey))
            {
                Debug.LogError("Could not find key " + inKey + "! Adding to loctext file");
                _localisationDictionary.Add(inKey, new LocalisedText(new LocalisedTextEntries()));

                LocalisedDatabase.LocalisedDatabase.Add(new LocalisationDatabaseEntry(inKey, new LocalisedTextEntries()));
            }

            return _localisationDictionary[inKey];
        }

        public void SetCurrentLanguage(ELanguageOptions inLanguageSetting)
        {
            UpdateLanguageSetting(inLanguageSetting);
            UpdatePlayerPrefs(inLanguageSetting);
        }
        // ~ILocalisationInterface

        private void UpdateLanguageSetting(ELanguageOptions inLanguageSetting)
        {
            foreach (var localisationEntry in _localisationDictionary)
            {
                localisationEntry.Value.CurrentLanguage = inLanguageSetting;
            }
        }

        private void UpdatePlayerPrefs(ELanguageOptions inLanguageOption)
        {
            PlayerPrefsRepo.SetKey(LocalisationComponentConstants.LanguageSettingKey, inLanguageOption);
            PlayerPrefsRepo.Save();
        }
    }
}
