// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Localisation;

namespace Assets.Scripts.Test.Localisation
{
    public class MockLocalisationInterface
        : ILocalisationInterface
    {
        public LocalisedText GetTextForLocalisationKeyResult { get; set; }
        public LocalisationKey SubmittedGetTextLocalisationKey { get; private set; }

        public ELanguageOptions ? SetCurrentLanguageResult { get; private set; }


        public LocalisedText GetTextForLocalisationKey(LocalisationKey inKey)
        {
            SubmittedGetTextLocalisationKey = inKey;

            return GetTextForLocalisationKeyResult;
        }

        public void SetCurrentLanguage(ELanguageOptions inLanguageSetting)
        {
            SetCurrentLanguageResult = inLanguageSetting;
        }
    }
}

#endif // UNITY_EDITOR
