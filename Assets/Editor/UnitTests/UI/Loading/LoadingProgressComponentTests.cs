// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Instance.Loading;
using Assets.Scripts.Test.UI.Loading;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Editor.UnitTests.UI.Loading
{
    [TestFixture]
    public class LoadingProgressComponentTestFixture
    {
        private Slider _slider;
        private TestLoadingProgressComponent _loading;

        [SetUp]
        public void BeforeTest()
        {
            _slider = new GameObject().AddComponent<Slider>();

            _loading = _slider.gameObject.AddComponent<TestLoadingProgressComponent>();
        }

        [TearDown]
        public void AfterTest()
        {
            _loading = null;

            _slider = null;
        }

        [Test]
        public void OnStart_SliderHasCorrectDefaults()
        {
            _slider.value = 100;
            _slider.minValue = -10;
            _slider.maxValue = -5;

            _loading.TestStart();

            Assert.IsFalse(_slider.wholeNumbers);
            Assert.AreEqual(1f, _slider.maxValue);
            Assert.AreEqual(0f, _slider.minValue);
            Assert.AreEqual(0f, _slider.value);

            _loading.TestDestroy();
        }

        [Test]
        public void OnStart_UpdatesSliderValueOnReceivingLoadingUpdatedEvent()
        {
            _loading.TestStart();

            const float expectedUpdate = 0.2f;

            _loading.TestDispatcher.InvokeMessageEvent(new LoadingProgressUpdatedUIMessage(expectedUpdate));

            Assert.AreEqual(expectedUpdate, _slider.value);

            _loading.TestDestroy();
        }

        [Test]
        public void OnEnd_DoesNotUpdateSliderValueOnReceivingLoadingUpdatedEvent()
        {
            _loading.TestStart();
            _loading.TestDestroy();

            const int expectedUpdate = 100;

            _loading.TestDispatcher.InvokeMessageEvent(new LoadingProgressUpdatedUIMessage(expectedUpdate));

            Assert.AreNotEqual(expectedUpdate, _slider.value);
        }
    }
}
