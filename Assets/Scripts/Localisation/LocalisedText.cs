// Copyright (C) Threetee Gang All Rights Reserved

using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Localisation
{
    public static class LocalisedTextConstants
    {
        public static readonly string DefaultLocalisedTextEntry = "ERROR: TEXT NOT TRANSLATED";
    }

    [System.Serializable]
    public class LocalisedTextEntry
    {
        public ELanguageOptions LanguageOption;
        public string TranslatedText;

        public LocalisedTextEntry(ELanguageOptions inLanguageOption)
            : this(inLanguageOption, LocalisedTextConstants.DefaultLocalisedTextEntry)
        {
        }

        public LocalisedTextEntry(ELanguageOptions inLanguageOption, string inTranslatedText)
        {
            LanguageOption = inLanguageOption;
            TranslatedText = inTranslatedText;
        }
    }

    [System.Serializable]
    public class LocalisedTextEntries
    {
        public List<LocalisedTextEntry> Entries;

        public LocalisedTextEntries()
        {
            Entries = new List<LocalisedTextEntry>();
            foreach (ELanguageOptions languageOption in Enum.GetValues(typeof(ELanguageOptions)))
            {
                Entries.Add(new LocalisedTextEntry(languageOption));
            }
        }

        public LocalisedTextEntries(List<LocalisedTextEntry> inEntries)
        {
            Entries = inEntries;
        }
    }

    public class LocalisedText
    {
        public Dictionary<ELanguageOptions, string> LocalisedTexts;
        public ELanguageOptions CurrentLanguage { get; set; }

        public LocalisedText(LocalisedTextEntries inEntries)
            : this(inEntries.Entries.ToDictionary(entry => entry.LanguageOption, entry => entry.TranslatedText))
        {
            
        }

        private LocalisedText(Dictionary<ELanguageOptions, string> inLocalisedTexts)
        {
            LocalisedTexts = inLocalisedTexts;
            CurrentLanguage = ELanguageOptions.EnglishUK;
        }

        public override string ToString()
        {
            if (LocalisedTexts.ContainsKey(CurrentLanguage))
            {
                return LocalisedTexts[CurrentLanguage];

            }

            return LocalisedTextConstants.DefaultLocalisedTextEntry;
        }
    }

    public static class LocalisedTextFunctions
    {
        public static string GetTextFromLocalisationKey(LocalisationKey inKey)
        {
            return LocalisationManager.CurrentLocalisationInterface.GetTextForLocalisationKey(inKey).ToString();
        }
    }

    // Type to use in Game Code
    public class LocalisedTextRef
    {
        public LocalisedText InternalLocalisedText { get; private set; }

        public LocalisedTextRef(LocalisationKey inKey)
        {
            InternalLocalisedText = LocalisationManager.CurrentLocalisationInterface.GetTextForLocalisationKey(inKey);
        }

        public override string ToString()
        {
            return InternalLocalisedText.ToString();
        }
    }
}
