// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Stamina;
using Assets.Scripts.Test.UI.HUD;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Editor.UnitTests.UI.HUD
{
    [TestFixture]
    public class StaminaHUDComponentTestFixture
    {
        private Slider _slider;
        private TestStaminaHUDComponent _stamina;

        [SetUp]
        public void BeforeTest()
        {
            _slider = new GameObject().AddComponent<Slider>();
            _slider.maxValue = 1000000;

            _stamina = _slider.gameObject.AddComponent<TestStaminaHUDComponent>();
        }

        [TearDown]
        public void AfterTest()
        {
            _stamina = null;

            _slider = null;
        }

        [Test]
        public void OnStart_SliderIsMinValueZeroAndValueZero()
        {
            _slider.value = 100;
            _slider.minValue = -10;

            _stamina.TestStart();

            Assert.IsTrue(_slider.wholeNumbers);
            Assert.AreEqual(0, _slider.minValue);
            Assert.AreEqual(0, _slider.value);

            _stamina.TestDestroy();
        }

        [Test]
        public void OnStart_UpdatesSliderValueOnReceivingStaminaEvent()
        {
            _stamina.TestStart();

            const int expectedUpdate = 100;

            _stamina.TestDispatcher.InvokeMessageEvent(new StaminaChangedUIMessage(expectedUpdate));

            Assert.AreEqual(expectedUpdate, _slider.value);

            _stamina.TestDestroy();
        }

        [Test]
        public void OnEnd_DoesNotUpdateSliderValueOnReceivingStaminaEvent()
        {
            _stamina.TestStart();
            _stamina.TestDestroy();

            const int expectedUpdate = 100;

            _stamina.TestDispatcher.InvokeMessageEvent(new StaminaChangedUIMessage(expectedUpdate));

            Assert.AreNotEqual(expectedUpdate, _slider.value);
        }

        [Test]
        public void OnStart_UpdatesSliderMaxValueOnReceivingMaxStaminaEvent()
        {
            _stamina.TestStart();

            const int expectedUpdate = 100;

            _stamina.TestDispatcher.InvokeMessageEvent(new MaxStaminaChangedUIMessage(expectedUpdate));

            Assert.AreEqual(expectedUpdate, _slider.maxValue);

            _stamina.TestDestroy();
        }

        [Test]
        public void OnEnd_DoesNotUpdateSliderMaxValueOnReceivingMaxStaminaEvent()
        {
            _stamina.TestStart();
            _stamina.TestDestroy();

            const int expectedUpdate = 100;

            _stamina.TestDispatcher.InvokeMessageEvent(new MaxStaminaChangedUIMessage(expectedUpdate));

            Assert.AreNotEqual(expectedUpdate, _slider.maxValue);
        }
    }
}
