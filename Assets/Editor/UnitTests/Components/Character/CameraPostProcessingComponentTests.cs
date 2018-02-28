// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Test.Components.Character;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Character
{
    [TestFixture]
    public class CameraPostProcessingComponentTestFixture
    {
        private TestCameraPostProcessingComponent _camera;

        [SetUp]
        public void BeforeTest()
        {
            _camera = new GameObject().AddComponent<TestCameraPostProcessingComponent>();
            _camera.TestStart();
        }

        [TearDown]
        public void AfterTest()
        {
            _camera = null;
        }

        [Test]
        public void RequestCameraFade_PropogatesToFaderCorrectly()
        {
            const float expectedFinalAlpha = 0.1f;
            const float expectedFinalTime = 3.0f;

            _camera.RequestCameraFade(expectedFinalAlpha, expectedFinalTime);

            Assert.IsTrue(_camera.GetCameraFader().FadeInProgress);
            Assert.AreEqual(expectedFinalTime, _camera.GetCameraFader().FinishTime);
            Assert.AreEqual(expectedFinalAlpha, _camera.GetCameraFader().FinalAlpha);
        }

        [Test]
        public void Update_UpdatesFader()
        {
            const float expectedFinalTime = 3.0f;
            const float expectedElapsedTime = expectedFinalTime * 0.5f;

            _camera.RequestCameraFade(1.0f, expectedFinalTime);

            _camera.TestUpdate(expectedElapsedTime);

            Assert.AreEqual(expectedElapsedTime, _camera.GetCameraFader().CurrentFadeTime);

        }
    }
}
