// Copyright (C) Threetee Gang All Rights Reserved 

using Assets.Scripts.Core;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Core
{
    public class PauseFunctionsTestFixture
    { 
        [Test]
        public void IsUnpaused_TimeScaleZero_True()
        {
            var priorTimeScale = Time.timeScale;
            Time.timeScale = 0.0f;

            Assert.IsFalse(PauseFunctions.IsGameUnpaused());

            Time.timeScale = priorTimeScale;
        }

        [Test]
        public void IsUnpaused_TimeScaleGreaterThanZero_False()
        {
            var priorTimeScale = Time.timeScale;
            Time.timeScale = 0.1f;

            Assert.IsTrue(PauseFunctions.IsGameUnpaused());

            Time.timeScale = priorTimeScale;
        }
    }
}
