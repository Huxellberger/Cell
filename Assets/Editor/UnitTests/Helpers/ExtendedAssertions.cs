// Copyright (C) Threetee Gang All Rights Reserved

using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Helpers
{
    public static class ExtendedAssertions
    {
        public static void AssertVectorsNotNearlyEqual(Vector3 first, Vector3 second)
        {
            Debug.Log("First Vector: " + first + "\tSecond Vector: " + second);

            Assert.IsTrue(Mathf.Abs(first.x - second.x) > 0.1f || Mathf.Abs(first.y - second.y) > 0.1f || Mathf.Abs(first.z - second.z) > 0.1f);
        }

        public static void AssertVectorsNearlyEqual(Vector3 first, Vector3 second)
        {
            Debug.Log("First Vector: " + first + "\tSecond Vector: " + second);

            Assert.IsTrue(Mathf.Abs(first.x - second.x) <= 0.1f);
            Assert.IsTrue(Mathf.Abs(first.y - second.y) <= 0.1f);
            Assert.IsTrue(Mathf.Abs(first.z - second.z) <= 0.1f);
        }
    }
}
