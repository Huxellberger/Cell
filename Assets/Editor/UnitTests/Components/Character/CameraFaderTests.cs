// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Character;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Assets.Editor.UnitTests.Components.Character
{
    [TestFixture]
    public class CameraFaderTestFixture
    {
        private CameraFader _fader;
        private ColorGrading _volumeColorGrading;
        private ColorGrading _otherVolumeColorGrading;

        private const float StartingExposure = 12.0f;
        private const float OtherStartingExposure = 6.0f;

        [SetUp]
        public void BeforeTest()
        {
            var volume = new GameObject().AddComponent<PostProcessVolume>();
            volume.profile.AddSettings<ColorGrading>().postExposure.value = StartingExposure;

            var otherVolume = volume.gameObject.AddComponent<PostProcessVolume>();
            otherVolume.profile.AddSettings<ColorGrading>().postExposure.value = OtherStartingExposure;

            volume.profile.TryGetSettings(out _volumeColorGrading);
            otherVolume.profile.TryGetSettings(out _otherVolumeColorGrading);

            _fader = new CameraFader(volume.gameObject);
        }

        [TearDown]
        public void AfterTest()
        {
            _fader = null;

            _otherVolumeColorGrading = null;
            _volumeColorGrading = null;
        }

        [Test]
        public void Creation_FadeInProgress_False()
        {
            Assert.IsFalse(_fader.FadeInProgress);
        }

        [Test]
        public void StartFade_FadeInProgress_True()
        {
            _fader.StartFade(1.0f, 10.0f);

            Assert.IsTrue(_fader.FadeInProgress);
        }

        [Test]
        public void FadeIncomplete_FadeInProgress_True()
        {
            const float fullFadeTime = 10.0f;

            _fader.StartFade(1.0f, fullFadeTime);
            _fader.Update(fullFadeTime * 0.5f);

            Assert.IsTrue(_fader.FadeInProgress);
        }

        [Test]
        public void FadeComplete_FadeInProgress_False()
        {
            const float fullFadeTime = 10.0f;

            _fader.StartFade(1.0f, fullFadeTime);
            _fader.Update(fullFadeTime + 0.1f);

            Assert.IsFalse(_fader.FadeInProgress);
        }

        [Test]
        public void FadeComplete_ScalesToAlpha()
        {
            const float fadeTime = 1.0f;
            const float fadeAlpha = 0.5f;

            _fader.StartFade(fadeAlpha, fadeTime);

            _fader.Update(fadeTime + 0.1f);

            Assert.AreEqual(Mathf.Lerp(StartingExposure, CameraFaderConstants.DefaultFinalPostExposure, fadeAlpha), _volumeColorGrading.postExposure.value);
            Assert.AreEqual(Mathf.Lerp(OtherStartingExposure, CameraFaderConstants.DefaultFinalPostExposure, fadeAlpha), _otherVolumeColorGrading.postExposure.value);
        }

        [Test]
        public void StartFade_ValuesUpdatedCorrectly()
        {
            const float expectedFadeTime = 10.0f;
            const float expectedFadeAlpha = 0.5f;

            _fader.StartFade(expectedFadeAlpha, expectedFadeTime);

            Assert.AreEqual(expectedFadeAlpha, _fader.FinalAlpha);
            Assert.AreEqual(expectedFadeTime, _fader.FinishTime);
        }

        [Test]
        public void StartFade_AlphaHasMaxClamp()
        {
            _fader.StartFade(100.0f, 10.0f);

            Assert.AreEqual(_fader.FinalAlpha, 1.0f);
        }

        [Test]
        public void StartFade_AlphaHasMinClamp()
        {
            _fader.StartFade(-100.0f, 10.0f);

            Assert.AreEqual(_fader.FinalAlpha, 0.0f);
        }

        [Test]
        public void InstantFade_FinishesAfterCall()
        {
            const float fadeAlpha = 0.5f;

            _fader.StartFade(fadeAlpha, 0.0f);

            Assert.AreEqual(Mathf.Lerp(StartingExposure, CameraFaderConstants.DefaultFinalPostExposure, fadeAlpha), _volumeColorGrading.postExposure.value);
            Assert.AreEqual(Mathf.Lerp(OtherStartingExposure, CameraFaderConstants.DefaultFinalPostExposure, fadeAlpha), _otherVolumeColorGrading.postExposure.value);
        }

        [Test]
        public void Update_FadeLerpsToExpectedPositionForAllVolumes()
        {
            const float fadeTime = 1.0f;
            const float fadeAlpha = 0.5f;

            _fader.StartFade(fadeAlpha, fadeTime);

            _fader.Update(fadeTime * 0.5f);

            Assert.AreEqual(Mathf.Lerp(StartingExposure, CameraFaderConstants.DefaultFinalPostExposure, fadeAlpha * 0.5f), _volumeColorGrading.postExposure.value);
            Assert.AreEqual(Mathf.Lerp(OtherStartingExposure, CameraFaderConstants.DefaultFinalPostExposure, fadeAlpha * 0.5f), _otherVolumeColorGrading.postExposure.value);
        }

        [Test]
        public void Update_TakesCurrentAlphaAsStartingPoint()
        {
            const float fadeTime = 1.0f;
            const float fadeAlpha = 0.5f;

            _fader.StartFade(fadeAlpha, fadeTime);
            _fader.Update(fadeTime * 0.5f);

            // Start New Fade from current alpha
            _fader.StartFade(0.0f, 1.0f);
            _fader.Update(0.0f);

            Assert.AreEqual(Mathf.Lerp(StartingExposure, CameraFaderConstants.DefaultFinalPostExposure, fadeAlpha * 0.5f), _volumeColorGrading.postExposure.value);
            Assert.AreEqual(Mathf.Lerp(OtherStartingExposure, CameraFaderConstants.DefaultFinalPostExposure, fadeAlpha * 0.5f), _otherVolumeColorGrading.postExposure.value);
        }
    }
}
