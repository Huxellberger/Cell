// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using System;
using Assets.Scripts.Core;
using NUnit.Framework;

namespace Assets.Editor.UnitTests.Core
{
    [TestFixture]
    public class CopyExtensionsTestFixture {

        [Test]
        public void DeepClone_ObjectsAreDifferent()
        {
            var testObject = new Object();
            var clonedObject = testObject.DeepClone();

            Assert.AreNotSame(testObject, clonedObject);
        }
    }
}

#endif
