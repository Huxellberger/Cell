// Copyright (C) Threetee Gang All Rights Reserved

namespace Assets.Scripts.Localisation
{
    [System.Serializable]
    public class LocalisationKey
    {
        public string LocalisationNamespace;
        public string LocalisationKeyValue;

        public LocalisationKey(string inLocalisationNamespace, string inLocalisationKeyValue)
        {
            LocalisationNamespace = inLocalisationNamespace;
            LocalisationKeyValue = inLocalisationKeyValue;
        }

        public bool Equals(LocalisationKey inKey)
        {
            return LocalisationNamespace.Equals(inKey.LocalisationNamespace) &&
                   LocalisationKeyValue.Equals(inKey.LocalisationKeyValue);
        }

        public override bool Equals(object obj)
        {
            return Equals((LocalisationKey)obj);
        }

        public override string ToString()
        {
            return LocalisationNamespace + ", " + LocalisationKeyValue;
        }

        public override int GetHashCode()
        {
            var keyValueHash = LocalisationKeyValue.GetHashCode();
            return LocalisationNamespace.GetHashCode() * keyValueHash * keyValueHash;
        }
    }
}
