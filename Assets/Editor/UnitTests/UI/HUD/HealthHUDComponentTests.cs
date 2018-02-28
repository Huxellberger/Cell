// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Health;
using Assets.Scripts.Test.UI.HUD;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Editor.UnitTests.UI.HUD
{
    [TestFixture]
    public class HealthHUDComponentTestFixture
    {
        private Slider _slider;
        private TestHealthHUDComponent _health;

        [SetUp]
        public void BeforeTest()
        {
            _slider = new GameObject().AddComponent<Slider>();
            _slider.maxValue = 1000000;

            _health = _slider.gameObject.AddComponent<TestHealthHUDComponent>();
        }

        [TearDown]
        public void AfterTest()
        {
            _health = null;

            _slider = null;
        }

        [Test]
        public void OnStart_SliderIsMinValueZeroAndValueZero()
        {
            _slider.value = 100;
            _slider.minValue = -10;

            _health.TestStart();

            Assert.IsTrue(_slider.wholeNumbers);
            Assert.AreEqual(0, _slider.minValue);
            Assert.AreEqual(0, _slider.value);

            _health.TestDestroy();
        }

        [Test]
        public void OnStart_UpdatesSliderValueOnReceivingHealthEvent()
        {
            _health.TestStart();

            const int expectedUpdate = 100;

            _health.TestDispatcher.InvokeMessageEvent(new HealthChangedUIMessage(expectedUpdate));

            Assert.AreEqual(expectedUpdate, _slider.value);

            _health.TestDestroy();
        }

        [Test]
        public void OnEnd_DoesNotUpdateSliderValueOnReceivingHealthEvent()
        {
            _health.TestStart();
            _health.TestDestroy();

            const int expectedUpdate = 100;

            _health.TestDispatcher.InvokeMessageEvent(new HealthChangedUIMessage(expectedUpdate));

            Assert.AreNotEqual(expectedUpdate, _slider.value);
        }

        [Test]
        public void OnStart_UpdatesSliderMaxValueOnReceivingMaxHealthEvent()
        {
            _health.TestStart();

            const int expectedUpdate = 100;

            _health.TestDispatcher.InvokeMessageEvent(new MaxHealthChangedUIMessage(expectedUpdate));

            Assert.AreEqual(expectedUpdate, _slider.maxValue);

            _health.TestDestroy();
        }

        [Test]
        public void OnEnd_DoesNotUpdateSliderMaxValueOnReceivingMaxHealthEvent()
        {
            _health.TestStart();
            _health.TestDestroy();

            const int expectedUpdate = 100;

            _health.TestDispatcher.InvokeMessageEvent(new MaxHealthChangedUIMessage(expectedUpdate));

            Assert.AreNotEqual(expectedUpdate, _slider.maxValue);
        }
    }
}
