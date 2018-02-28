// Copyright (C) Threetee Gang All Rights Reserved

using System;
using Assets.Scripts.Localisation;
using Assets.Scripts.Test.Localisation;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Localisation
{
    public class LocalisationDropdownTestFixture
    {
        private MockLocalisationInterface _locInterface;
        private TestLocalisationDropdown _dropdown;

        [SetUp]
        public void BeforeTest()
        {
            _locInterface = new MockLocalisationInterface();
            LocalisationManager.CurrentLocalisationInterface = _locInterface;

            _dropdown = new GameObject().AddComponent<TestLocalisationDropdown>();
            _dropdown.TestStart();
        }

        [TearDown]
        public void AfterTest()
        {
            _dropdown = null;

            LocalisationManager.CurrentLocalisationInterface = null;
            _locInterface = null;
        }

        [Test]
        public void LocalisationDropdown_Start_NumberOfOptionsEqualToLanguages()
        {
            _dropdown.TestStart();
            Assert.AreEqual(_dropdown.options.Count, Enum.GetValues(typeof(ELanguageOptions)).Length);
        }

        [Test]
        public void LocalisationDropdown_OnSelection_SetsCurrentLanguageToCorrespondingOption()
        {
            var index = 0;
            foreach (var languageOption in Enum.GetValues(typeof(ELanguageOptions)))
            {
                Assert.IsTrue(_dropdown.options[index].text.Equals(languageOption.ToString()));
                _dropdown.OnSelection(index);
                Assert.AreEqual(languageOption, _locInterface.SetCurrentLanguageResult);
                index++;
            }
        }

        [Test]
        public void LocalisationDropdown_OnValueChanged_SetsCurrentLanguageToCorrespondingOption()
        {
            _dropdown.onValueChanged.Invoke(0);
            Assert.IsNotNull(_locInterface.SetCurrentLanguageResult);
        }
    }
}
