// Copyright (C) Threetee Gang All Rights Reserved

namespace Assets.Scripts.Localisation
{
    public interface ILocalisationInterface
    {
        LocalisedText GetTextForLocalisationKey(LocalisationKey inKey);
        void SetCurrentLanguage(ELanguageOptions inLanguageSetting);
    }
}
