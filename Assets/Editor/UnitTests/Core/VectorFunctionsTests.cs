// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Core;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Core
{
    [TestFixture]
    public class VectorFunctionsTestFixture
    {
        [Test]
        public void DistSquared_ReturnsDistanceBetween2Vectors()
        {
            var firstVector = new Vector3(-2.0f, 101.1f, 30.0f);
            var secondVector = new Vector3(12.0f, -30.0f, 27.0f);

            Assert.AreEqual(Mathf.Pow((firstVector.x - secondVector.x), 2) + Mathf.Pow((firstVector.y - secondVector.y), 2) +
                            Mathf.Pow((firstVector.z - secondVector.z), 2), VectorFunctions.DistanceSquared(firstVector, secondVector));
        }

        [Test]
        public void LerpVector_ReturnsLerpedVector()
        {
            var firstVector = new Vector3(-2.0f, 101.1f, 30.0f);
            var secondVector = new Vector3(12.0f, -30.0f, 27.0f);

            const float lerpPoint = 0.7f;

            Assert.AreEqual
            (   new Vector3
                (
                    Mathf.Lerp(firstVector.x, secondVector.x, lerpPoint),
                    Mathf.Lerp(firstVector.y, secondVector.y, lerpPoint),
                    Mathf.Lerp(firstVector.z, secondVector.z, lerpPoint)
                ), 
                VectorFunctions.LerpVector(firstVector, secondVector, lerpPoint)
            );
        }

        [Test]
        public void LerpVector_ClampsAt1()
        {
            var firstVector = new Vector3(-2.0f, 101.1f, 30.0f);
            var secondVector = new Vector3(12.0f, -30.0f, 27.0f);

            Assert.AreEqual
            (   new Vector3
                (
                    Mathf.Lerp(firstVector.x, secondVector.x, 1.0f),
                    Mathf.Lerp(firstVector.y, secondVector.y, 1.0f),
                    Mathf.Lerp(firstVector.z, secondVector.z, 1.0f)
                ),
                VectorFunctions.LerpVector(firstVector, secondVector, 1.2f)
            );
        }

        [Test]
        public void LerpVector_ClampsAt0()
        {
            var firstVector = new Vector3(-2.0f, 101.1f, 30.0f);
            var secondVector = new Vector3(12.0f, -30.0f, 27.0f);

            Assert.AreEqual
            (   new Vector3
                (
                    Mathf.Lerp(firstVector.x, secondVector.x, 0.0f),
                    Mathf.Lerp(firstVector.y, secondVector.y, 0.0f),
                    Mathf.Lerp(firstVector.z, secondVector.z, 0.0f)
                ),
                VectorFunctions.LerpVector(firstVector, secondVector, -1.2f)
            );
        }
    }
}
