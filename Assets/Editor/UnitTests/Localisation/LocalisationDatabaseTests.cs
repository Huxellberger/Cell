// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.Localisation;
using NUnit.Framework;

namespace Assets.Editor.UnitTests.Localisation
{
    public class LocalisationDatabaseTestFixture
    {
        [Test]
        public void ToLocalisationMap_UsesGivenEntries()
        {
            var entries = new List<LocalisationDatabaseEntry>
            {
                new LocalisationDatabaseEntry(new LocalisationKey("BLAH", "OTHER BLAH"), new LocalisedTextEntries
                ( 
                    new List<LocalisedTextEntry>
                    {
                        new LocalisedTextEntry(ELanguageOptions.EnglishUK, "TRANSLATED TEXT"),
                        new LocalisedTextEntry(ELanguageOptions.German, "OTHER TRANSLATED TEXT")
                    }

                )),
                new LocalisationDatabaseEntry(new LocalisationKey("SECOND BLAH", "SECOND OTHER BLAH"), new LocalisedTextEntries
                (
                    new List<LocalisedTextEntry>{ new LocalisedTextEntry(ELanguageOptions.EnglishUK, "SECOND TRANSLATED TEXT")}
                ))
            };

            var localisedTextDatabase = new LocalisedTextDatabase {LocalisedDatabase = entries};

            var generatedDictionary = localisedTextDatabase.ToLocalisationMap();

            foreach (var entry in entries)
            {
                Assert.IsTrue(generatedDictionary.ContainsKey(entry.TextKey));

                foreach (var localisedTextEntry in entry.LocalisedTexts.Entries)
                {
                    Assert.IsTrue(generatedDictionary[entry.TextKey].LocalisedTexts.ContainsKey(localisedTextEntry.LanguageOption));
                    Assert.IsTrue(generatedDictionary[entry.TextKey].LocalisedTexts[localisedTextEntry.LanguageOption]
                        .Equals(localisedTextEntry.TranslatedText));
                }
            }
        }
    }
}
