// Copyright (C) Threetee Gang All Rights Reserved

using System;
using System.Collections.Generic;
using Assets.Scripts.Localisation;
using Assets.Scripts.Test.Localisation;
using NUnit.Framework;

namespace Assets.Editor.UnitTests.Localisation
{
    [TestFixture]
    public class LocalisedTextTestFixture
    {
        [Test]
        public void LocalisedTextEntryWithEnum_InitialisedWithDefaultLocalisdText()
        {
            const ELanguageOptions chosenLanguage = ELanguageOptions.EnglishUK;
            var localisedTextEntry = new LocalisedTextEntry(chosenLanguage);

            Assert.AreEqual(chosenLanguage, localisedTextEntry.LanguageOption);
            Assert.IsTrue(LocalisedTextConstants.DefaultLocalisedTextEntry.Equals(localisedTextEntry.TranslatedText));
        }

        [Test]
        public void LocalisedTextEntryWithEnumAndText_InitialisedWithExpectedValues()
        {
            const ELanguageOptions chosenLanguage = ELanguageOptions.EnglishUK;
            var chosenText = "TEXT";

            var localisedTextEntry = new LocalisedTextEntry(chosenLanguage, chosenText);

            Assert.AreEqual(chosenLanguage, localisedTextEntry.LanguageOption);
            Assert.IsTrue(chosenText.Equals(localisedTextEntry.TranslatedText));
        }

        [Test]
        public void LocalisedTextEntriesDefault_ElementForAllEnumValues()
        {
            var localisedTextEntries = new LocalisedTextEntries();

            var languageOptions = Enum.GetValues(typeof(ELanguageOptions));
            Assert.IsTrue(localisedTextEntries.Entries.Count == languageOptions.Length);

            foreach (var languageOption in languageOptions)
            {
                bool foundOption = false;
                foreach (var entry in localisedTextEntries.Entries)
                {
                    if (entry.LanguageOption == (ELanguageOptions)languageOption)
                    {
                        foundOption = true;
                    }
                }
                Assert.IsTrue(foundOption);
            }
        }

        [Test]
        public void LocalisedTextEntriesListOfEntries_SetsAsEntries()
        {
            var entries = new List<LocalisedTextEntry>
            {
                new LocalisedTextEntry(ELanguageOptions.German),
                new LocalisedTextEntry(ELanguageOptions.EnglishUK)
            };

            var localisedTextEntries = new LocalisedTextEntries(entries);

            Assert.AreSame(entries, localisedTextEntries.Entries);
        }

        [Test]
        public void LocalisedTextWithListOfLocalisedTextEntries_ConvertsToExpectedDictionary()
        {
            var entries = new List<LocalisedTextEntry>
            {
                new LocalisedTextEntry(ELanguageOptions.German, "TEST"),
                new LocalisedTextEntry(ELanguageOptions.EnglishUK, "OTHERTEST")
            };

            var localisedText = new LocalisedText(new LocalisedTextEntries(entries));

            foreach (var entry in entries)
            {
                Assert.IsTrue(localisedText.LocalisedTexts.ContainsKey(entry.LanguageOption));
                Assert.IsTrue(localisedText.LocalisedTexts[entry.LanguageOption].Equals(entry.TranslatedText));
            }
        }

        [Test]
        public void LocalisedText_DefaultLanguageIsEnglishUK()
        {
            var entries = new List<LocalisedTextEntry>
            {
                new LocalisedTextEntry(ELanguageOptions.German, "TEST"),
                new LocalisedTextEntry(ELanguageOptions.EnglishUK, "OTHERTEST")
            };

            var localisedText = new LocalisedText(new LocalisedTextEntries(entries));

            Assert.AreEqual(ELanguageOptions.EnglishUK, localisedText.CurrentLanguage);
        }

        [Test]
        public void LocalisedText_ToString_UsesCurrentLanguage()
        {
            var expectedEntry = new LocalisedTextEntry(ELanguageOptions.EnglishUK, "OTHERTEST");

            var entries = new List<LocalisedTextEntry>
            {
                new LocalisedTextEntry(ELanguageOptions.German, "TEST"),
                expectedEntry
            };

            var localisedText = new LocalisedText(new LocalisedTextEntries(entries));

            Assert.IsTrue(localisedText.ToString().Equals(expectedEntry.TranslatedText));
        }

        [Test]
        public void LocalisedText_NoEntryForKey_ReturnsTextConstant()
        {
            var entries = new List<LocalisedTextEntry>
            {
                new LocalisedTextEntry(ELanguageOptions.German, "TEST")
            };

            var localisedText = new LocalisedText(new LocalisedTextEntries(entries));

            Assert.IsTrue(localisedText.ToString().Equals(LocalisedTextConstants.DefaultLocalisedTextEntry));
        }

        [Test]
        public void GetTextFromLocalisationKey_QueriesLocalisationManagerToGetLocalisedText()
        {
            var locInterface = new MockLocalisationInterface();
            LocalisationManager.CurrentLocalisationInterface = locInterface;

            var entries = new List<LocalisedTextEntry>
            {
                new LocalisedTextEntry(ELanguageOptions.EnglishUK, "TEST")
            };

            locInterface.GetTextForLocalisationKeyResult = new LocalisedText(new LocalisedTextEntries(entries));

            var expectedKey = new LocalisationKey("Testy", "Test");

            Assert.AreEqual(locInterface.GetTextForLocalisationKeyResult.ToString(), LocalisedTextFunctions.GetTextFromLocalisationKey(expectedKey));
            Assert.AreSame(locInterface.SubmittedGetTextLocalisationKey, expectedKey);

            LocalisationManager.CurrentLocalisationInterface = null;
        }

        [Test]
        public void LocalisedTextRef_QueriesLocalisationManagerToGetLocalisedText()
        {
            var locInterface = new MockLocalisationInterface();
            LocalisationManager.CurrentLocalisationInterface = locInterface;

            var entries = new List<LocalisedTextEntry>
            {
                new LocalisedTextEntry(ELanguageOptions.German, "TEST")
            };

            locInterface.GetTextForLocalisationKeyResult = new LocalisedText(new LocalisedTextEntries(entries));
            
            var expectedKey = new LocalisationKey("Testy", "Test");

            var localisedTextRef = new LocalisedTextRef(expectedKey);

            Assert.AreSame(expectedKey, locInterface.SubmittedGetTextLocalisationKey);
            Assert.AreSame(locInterface.GetTextForLocalisationKeyResult, localisedTextRef.InternalLocalisedText);

            LocalisationManager.CurrentLocalisationInterface = null;
        }

        [Test]
        public void LocalisedTextRef_ToString_UsesUnderlyingLocalisedText()
        {
            var locInterface = new MockLocalisationInterface();
            LocalisationManager.CurrentLocalisationInterface = locInterface;

            var entries = new List<LocalisedTextEntry>
            {
                new LocalisedTextEntry(ELanguageOptions.EnglishUK, "TEST")
            };

            locInterface.GetTextForLocalisationKeyResult = new LocalisedText(new LocalisedTextEntries(entries));

            var expectedKey = new LocalisationKey("Testy", "Test");

            var localisedTextRef = new LocalisedTextRef(expectedKey);

            Assert.IsTrue(localisedTextRef.ToString().Equals(localisedTextRef.InternalLocalisedText.ToString()));

            LocalisationManager.CurrentLocalisationInterface = null;
        }
    }
}
