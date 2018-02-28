// Copyright (C) Threetee Gang All Rights Reserved

using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Helpers
{
    public class ExtendedAssertionsTestFixture
    {

        [Test]
        public void AssertVectorsNearlyEqual_NearlyEqual_Passes()
        {
            var expectedVector = new Vector3(1.0f, 2.0f, 3.0f);

            ExtendedAssertions.AssertVectorsNearlyEqual(expectedVector, expectedVector);
        }

        [Test]
        public void AssertVectorsNearlyEqual_NotNearlyEqual_Fails()
        {
            var expectedVector = new Vector3(1.0f, 2.0f, 3.0f);

            Assert.Throws<AssertionException>(() => ExtendedAssertions.AssertVectorsNearlyEqual(expectedVector, expectedVector + new Vector3(10.0f, 3.0f, 11.0f)));
        }

        [Test]
        public void AssertVectorsNotNearlyEqual_NearlyEqual_Fails()
        {
            var expectedVector = new Vector3(1.0f, 2.0f, 3.0f);

            Assert.Throws<AssertionException>(() => ExtendedAssertions.AssertVectorsNotNearlyEqual(expectedVector, expectedVector));
        }

        [Test]
        public void AssertVectorsNotNearlyEqual_NotNearlyEqual_Passes()
        {
            var expectedVector = new Vector3(1.0f, 2.0f, 3.0f);

            ExtendedAssertions.AssertVectorsNotNearlyEqual(expectedVector, expectedVector + new Vector3(1.0f, 2.0f, 3.0f));
        }
    }
}
