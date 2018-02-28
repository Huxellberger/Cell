// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Localisation
{
    [System.Serializable]
    public class LocalisationDatabaseEntry
    {
        public LocalisationKey TextKey;
        public LocalisedTextEntries LocalisedTexts;

        public LocalisationDatabaseEntry(LocalisationKey inKey, LocalisedTextEntries inLocalisedTexts)
        {
            TextKey = inKey;
            LocalisedTexts = inLocalisedTexts;
        }
    }

    [CreateAssetMenu(fileName = "LocalisedText", menuName = "ProjectQ/Localisation/LocalisedTextDatabase", order = 1)]
    public class LocalisedTextDatabase : ScriptableObject
    {
        public List<LocalisationDatabaseEntry> LocalisedDatabase;

        public Dictionary<LocalisationKey, LocalisedText> ToLocalisationMap()
        {
            return LocalisedDatabase.ToDictionary(localisationDatabaseEntry => localisationDatabaseEntry.TextKey, localisationDatabaseEntry => new LocalisedText(localisationDatabaseEntry.LocalisedTexts));
        }
    }
}
