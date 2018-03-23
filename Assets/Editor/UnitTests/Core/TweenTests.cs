// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Core;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Core
{
    [TestFixture]
    public class TweenTestFixture
    {
        private float _tweenedValue = 0.0f;
        private bool _delegateUpdated = false;

        [SetUp]
        public void BeforeTest()
        {
            _tweenedValue = 0.0f;
            _delegateUpdated = false;
        }

        [Test]
        public void Creation_ZeroTimeToCompletion_StartsCompleted()
        {
            var tween = new Tween(0.0f, 1.0f, 0.0f, UpdateDelegate);

            Assert.IsTrue(tween.Complete);
        }

        [Test]
        public void Creation_NonZeroTimeToCompletion_StartsUncompleted()
        {
            var tween = new Tween(0.0f, 1.0f, 0.1f, UpdateDelegate);

            Assert.IsFalse(tween.Complete);
        }

        [Test]
        public void Update_ZeroTimeToCompletion_DoesNotCallDelegate()
        {
            var tween = new Tween(0.0f, 1.0f, 0.0f, UpdateDelegate);
            tween.UpdateTween(1.0f);

            Assert.IsFalse(_delegateUpdated);
        }

        [Test]
        public void Update_NonZeroTimeToCompletion_CallsDelegate()
        {
            var tween = new Tween(0.0f, 1.0f, 0.1f, UpdateDelegate);
            tween.UpdateTween(1.0f);

            Assert.IsTrue(_delegateUpdated);
        }

        [Test]
        public void Update_ExceedsTimeToCompletion_Completes()
        {
            const float timeToComplete = 1.0f;
            var tween = new Tween(0.0f, 1.0f, timeToComplete, UpdateDelegate);
            tween.UpdateTween(timeToComplete);

            Assert.IsTrue(tween.Complete);
        }

        [Test]
        public void Update_LerpsToExpectedValue()
        {
            const float startValue = 1.2f;
            const float endValue = 2.4f;
            const float timeToComplete = 1.0f;
            const float updateRatio = 0.4f;

            var tween = new Tween(startValue, endValue, timeToComplete, UpdateDelegate);
            tween.UpdateTween(timeToComplete * updateRatio);

            Assert.AreEqual(_tweenedValue, Mathf.Lerp(startValue, endValue, (timeToComplete * updateRatio / timeToComplete)));
        }

        public void UpdateDelegate(float newValue)
        {
            _tweenedValue = newValue;
            _delegateUpdated = true;
        }
    }
}
