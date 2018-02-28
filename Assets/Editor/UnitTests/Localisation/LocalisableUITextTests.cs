// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.Localisation;
using Assets.Scripts.Test.Localisation;
using Castle.Core.Internal;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Localisation
{
    [TestFixture]
    public class LocalisableUITextTestFixture
    {
        private MockLocalisationInterface _localisationInterface;
        private LocalisedText _localisedText;
        private TestLocalisableUIText _localisableUIText;

        [SetUp]
        public void BeforeTest()
        {
            _localisationInterface = new MockLocalisationInterface();
            LocalisationManager.CurrentLocalisationInterface = _localisationInterface;

            _localisedText = new LocalisedText(new LocalisedTextEntries{Entries =  new List<LocalisedTextEntry>
            {
                new LocalisedTextEntry(ELanguageOptions.EnglishUK, "TEXTYTEXT"),
                new LocalisedTextEntry(ELanguageOptions.German, "OtherTextyText")
            }});

            _localisationInterface.GetTextForLocalisationKeyResult = _localisedText;

            _localisableUIText = new GameObject().AddComponent<TestLocalisableUIText>();
        }

        [TearDown]
        public void AfterTest()
        {
            _localisableUIText = null;
            _localisedText = null;

            LocalisationManager.CurrentLocalisationInterface = null;
            _localisationInterface = null;
        }

        [Test]
        public void Creation_TextIsEmpty()
        {
            Assert.IsTrue(_localisableUIText.text.IsNullOrEmpty());
        }

        [Test]
        public void OnEnable_NoStart_DoesNotQueryLocalisationInterface()
        {
            _localisableUIText.LocalisedTextKey = new LocalisationKey("TEST", "STILL A TEST");
            _localisableUIText.enabled = false;
            _localisableUIText.enabled = true;

            Assert.IsNull(_localisationInterface.SubmittedGetTextLocalisationKey);
        }

        [Test]
        public void OnEnable_Start_QueriesLocalisationInterface()
        {
            _localisableUIText.BlockStart = false;
            _localisableUIText.TestStart();
            _localisableUIText.LocalisedTextKey = new LocalisationKey("TEST", "STILL A TEST");
            _localisableUIText.enabled = false;
            _localisableUIText.enabled = true;

            Assert.IsTrue(_localisationInterface.SubmittedGetTextLocalisationKey.Equals(_localisableUIText.LocalisedTextKey));
        }

        [Test]
        public void OnEnable_NoStart_SetsNoText()
        {
            _localisableUIText.LocalisedTextKey = new LocalisationKey("TEST", "STILL A TEST");
            _localisableUIText.enabled = false;
            _localisableUIText.enabled = true;

            Assert.IsTrue(_localisableUIText.text.IsNullOrEmpty());
        }

        [Test]
        public void OnEnable_Start_SetsTextToCurrentLanguageSetting()
        {
            _localisableUIText.BlockStart = false;
            _localisableUIText.TestStart();
            _localisationInterface.GetTextForLocalisationKeyResult.CurrentLanguage = ELanguageOptions.German;
            _localisableUIText.LocalisedTextKey = new LocalisationKey("TEST", "STILL A TEST");
            _localisableUIText.enabled = false;
            _localisableUIText.enabled = true;

            Assert.IsTrue(_localisableUIText.text.Equals(_localisationInterface.GetTextForLocalisationKeyResult.LocalisedTexts[_localisationInterface.GetTextForLocalisationKeyResult.CurrentLanguage]));
        }

        [Test]
        public void OnEnable_NoKey_NoQueryMade()
        {
            _localisableUIText.BlockStart = false;
            _localisableUIText.TestStart();

            _localisableUIText.enabled = false;
            _localisableUIText.enabled = true;

            Assert.IsNull(_localisationInterface.SubmittedGetTextLocalisationKey);
        }

        [Test]
        public void Start_QueriesLocalisationInterface()
        {
            _localisableUIText.LocalisedTextKey = new LocalisationKey("TEST", "STILL A TEST");
            _localisableUIText.BlockStart = false;
            _localisableUIText.TestStart();

            Assert.IsTrue(_localisationInterface.SubmittedGetTextLocalisationKey.Equals(_localisableUIText.LocalisedTextKey));
        }

        [Test]
        public void Start_SetTextToCurrentLanguageSetting()
        {
            _localisableUIText.LocalisedTextKey = new LocalisationKey("TEST", "STILL A TEST");
            _localisableUIText.BlockStart = false;
            _localisableUIText.TestStart();

            Assert.IsTrue(_localisableUIText.text.Equals(_localisationInterface.GetTextForLocalisationKeyResult.LocalisedTexts[_localisationInterface.GetTextForLocalisationKeyResult.CurrentLanguage]));
        }

        [Test]
        public void Start_NoKey_NoQueryMade()
        {
            _localisableUIText.BlockStart = false;
            _localisableUIText.TestStart();

            Assert.IsNull(_localisationInterface.SubmittedGetTextLocalisationKey);
        }
    }
}
