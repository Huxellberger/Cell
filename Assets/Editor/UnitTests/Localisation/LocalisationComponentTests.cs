// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.Core;
using Assets.Scripts.Localisation;
using Assets.Scripts.Test.Input;
using Assets.Scripts.Test.Localisation;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Editor.UnitTests.Localisation
{
    [TestFixture]
    public class LocalisationComponentTestFixture
    {
        private TestLocalisationComponent _localisationComponent;
        private LocalisedTextDatabase _localisedText;

        [SetUp]
        public void BeforeTest()
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

            _localisedText = new LocalisedTextDatabase { LocalisedDatabase = entries };

            _localisationComponent = new GameObject().AddComponent<TestLocalisationComponent>();
            _localisationComponent.LocalisedDatabase = _localisedText;
            _localisationComponent.PlayerPrefsMock = new MockPlayerPrefsRepository();
        }

        [TearDown]
        public void AfterTest()
        {
            LocalisationManager.CurrentLocalisationInterface = null;

            _localisationComponent = null;
            _localisedText = null;
        }

        [Test]
        public void LocalisationComponent_Awake_SetToLocalisationManagerInstance()
        {
            _localisationComponent.TestAwake();

            Assert.AreSame(_localisationComponent, LocalisationManager.CurrentLocalisationInterface);
        }

        [Test]
        public void LocalisationComponent_Awake_UsesDatabaseToGenerateMappings()
        {
            _localisationComponent.TestAwake();

            foreach (var localisedEntry in _localisedText.LocalisedDatabase)
            {
                Assert.IsFalse(LocalisedTextConstants.DefaultLocalisedTextEntry.Equals(_localisationComponent.GetTextForLocalisationKey(localisedEntry.TextKey).LocalisedTexts[0]));
            }
        }

        [Test]
        public void LocalisationComponent_Awake_UsesPlayerPrefsToCheckLanguageSettings()
        {
            _localisationComponent.TestAwake();

            Assert.IsTrue(_localisationComponent.PlayerPrefsMock.GetValueForKeySentKey.Equals(LocalisationComponentConstants.LanguageSettingKey));
        }

        [Test]
        public void LocalisationComponent_Awake_UpdatesLanguageSettingBasedOnPlayerPrefs()
        {
            const ELanguageOptions expectedLanguage = ELanguageOptions.German;
            _localisationComponent.PlayerPrefsMock.GetValueForKeyResult = expectedLanguage.ToString();
            _localisationComponent.TestAwake();

            var actualText =
                _localisationComponent.GetTextForLocalisationKey(_localisedText.LocalisedDatabase[0].TextKey);
            Assert.IsTrue(actualText.LocalisedTexts[actualText.CurrentLanguage]
                .Equals(actualText.LocalisedTexts[expectedLanguage]));
        }

        [Test]
        public void LocalisationComponent_AwakeWithLanguageSettings_DoesNotSave()
        {
            const ELanguageOptions expectedLanguage = ELanguageOptions.German;
            _localisationComponent.PlayerPrefsMock.GetValueForKeyResult = expectedLanguage.ToString();
            _localisationComponent.TestAwake();

            Assert.IsFalse(_localisationComponent.PlayerPrefsMock.SaveCalled);
        }

        [Test]
        public void LocalisationComponent_GetTextForLocalisationKey_NotExisting_ThrowsErrorAndReturnsDefaultEntry()
        {
            _localisationComponent.TestAwake();

            var errornousKey = new LocalisationKey("OH NO ERROR", "BAD KEY BADDDDD");
            LogAssert.Expect(LogType.Error, "Could not find key " + errornousKey + "! Adding to loctext file");

            var generatedText = _localisationComponent.GetTextForLocalisationKey(errornousKey);

            foreach (var textEntries in generatedText.LocalisedTexts)
            {
                Assert.IsTrue(LocalisedTextConstants.DefaultLocalisedTextEntry.Equals(textEntries.Value));
            }
        }

        [Test]
        public void LocalisationComponent_GetTextForLocalisationKey_ReturnsTextWithExpectedSettings()
        {
            _localisationComponent.TestAwake();

            const ELanguageOptions expectedLanguage = ELanguageOptions.German;

            _localisationComponent.SetCurrentLanguage(expectedLanguage);

            var generatedText = _localisationComponent.GetTextForLocalisationKey(_localisedText.LocalisedDatabase[0].TextKey);

            var expectedText = new LocalisedText(_localisedText.LocalisedDatabase[0].LocalisedTexts);
            expectedText.CurrentLanguage = expectedLanguage;

            Assert.IsTrue(expectedText.ToString().Equals(generatedText.ToString()));
        }

        [Test]
        public void LocalisationComponent_GetTextForLocalisationKey_NotExisting_ThrowsErrorAndUpdatesDatabaseAsset()
        {
            _localisationComponent.TestAwake();

            var errornousKey = new LocalisationKey("OH NO ERROR", "BAD KEY BADDDDD");
            LogAssert.Expect(LogType.Error, "Could not find key " + errornousKey + "! Adding to loctext file");

            _localisationComponent.GetTextForLocalisationKey(errornousKey);

            var foundKey = false;

            foreach (var localisedEntry in _localisedText.LocalisedDatabase)
            {
                if (localisedEntry.TextKey.Equals(errornousKey))
                {
                    foundKey = true;
                }
            }

            Assert.IsTrue(foundKey);
        }

        [Test]
        public void LocalisationComponent_SetCurrentLanguage_Saves()
        {
            _localisationComponent.TestAwake();

            _localisationComponent.SetCurrentLanguage(ELanguageOptions.German);

            Assert.IsTrue(_localisationComponent.PlayerPrefsMock.SaveCalled);
        }

        [Test]
        public void LocalisationComponent_SetCurrentLanguage_UpdatesPlayerPrefsWithCorrectKey()
        {
            _localisationComponent.TestAwake();

            const ELanguageOptions expectedLanguageOption = ELanguageOptions.German;

            _localisationComponent.SetCurrentLanguage(expectedLanguageOption);

            Assert.AreEqual(expectedLanguageOption, EnumExtensions.TryParse<ELanguageOptions>(_localisationComponent.PlayerPrefsMock.SetKeyResult.ToString()).Get());
        }

        [Test]
        public void LocalisationComponent_SetCurrentLanguage_UpdatesLocalisedTextsWithResult()
        {
            _localisationComponent.TestAwake();

            const ELanguageOptions expectedLanguageOption = ELanguageOptions.German;

            _localisationComponent.SetCurrentLanguage(expectedLanguageOption);

            Assert.AreEqual(expectedLanguageOption, _localisationComponent.GetTextForLocalisationKey(_localisedText.LocalisedDatabase[0].TextKey).CurrentLanguage);
        }

        [Test]
        public void LocalisationComponent_OnDestroy_NullsLocalisationManagerInstance()
        {
            _localisationComponent.TestAwake();
            _localisationComponent.TestDestroy();

            Assert.IsNull(LocalisationManager.CurrentLocalisationInterface);
        }
    }
}
