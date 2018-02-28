// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using NUnit.Framework;

namespace Assets.Editor.UnitTests.Helpers
{
    [TestFixture]
    public class ObjectComparisonExtensionTestFixture
    {
        public class ObjectToCompare
        {
            private readonly int _privateType;
            public ObjectToCompare(int publicValueType, OtherObjectToCompare publicRefType, int privateType)
            {
                _privateType = privateType;
                PublicValueType = publicValueType;
                PublicRefType = publicRefType;
            }

            public int PublicValueType { get; set; }
            public OtherObjectToCompare PublicRefType { get; set; }

            public static string PubPropName = "PublicValueType";
        }

        public class OtherObjectToCompare
        {
            private readonly int _privateType;
            public OtherObjectToCompare(int publicValueType, int privateType)
            {
                _privateType = privateType;
                PublicValueType = publicValueType;
            }

            public int PublicValueType { get; set; }
        }

        [Test]
        public void EqualByPublicProperties_PrivateDoesNotMatch_True()
        {
            const int publicVal = 2;
            var publicObj = new OtherObjectToCompare(2, 3);
            
            var from = new ObjectToCompare(publicVal, publicObj, 1);
            var to = new ObjectToCompare(publicVal, publicObj, 7);

            Assert.IsTrue(ObjectComparisonExtensions.EqualByPublicProperties(from, to));
        }

        [Test]
        public void EqualByPublicProperties_PublicDoesNotMatch_False()
        {
            const int privateVal = 2;
            var publicObj = new OtherObjectToCompare(2, 3);

            var from = new ObjectToCompare(3, publicObj, privateVal);
            var to = new ObjectToCompare(2, publicObj, privateVal);

            Assert.IsFalse(ObjectComparisonExtensions.EqualByPublicProperties(from, to));
        }

        [Test]
        public void EqualByPublicProperties_AllMatch_True()
        {
            const int publicVal = 2;
            const int privateVal = 2;
            var publicObj = new OtherObjectToCompare(2, 3);

            var from = new ObjectToCompare(publicVal, publicObj, privateVal);
            var to = new ObjectToCompare(publicVal, publicObj, privateVal);

            Assert.IsTrue(ObjectComparisonExtensions.EqualByPublicProperties(from, to));
        }

        [Test]
        public void EqualByPublicProperties_SubObjectSameRef_True()
        {
            const int publicVal = 2;
            const int privateVal = 2;

            var publicObj = new OtherObjectToCompare(2, 3);

            var from = new ObjectToCompare(publicVal, publicObj, privateVal);
            var to = new ObjectToCompare(publicVal, publicObj, privateVal);

            Assert.IsTrue(ObjectComparisonExtensions.EqualByPublicProperties(from, to));
        }

        [Test]
        public void EqualByPublicProperties_SubObjectDifferentRef_False()
        {
            const int publicVal = 2;
            const int privateVal = 2;
            const int publicSubVal = 4;
            const int privateSubVal = 2;

            var fromPublicObj = new OtherObjectToCompare(publicSubVal, privateSubVal);
            var toPublicObj = new OtherObjectToCompare(publicSubVal, privateSubVal);

            var from = new ObjectToCompare(publicVal, fromPublicObj, privateVal);
            var to = new ObjectToCompare(publicVal, toPublicObj, privateVal);

            Assert.IsFalse(ObjectComparisonExtensions.EqualByPublicProperties(from, to));
        }

        [Test]
        public void EqualByPublicProperties_IgnoreListUsed_IgnoresUnmatchedProperites()
        {
            const int privateVal = 2;
            var publicObj = new OtherObjectToCompare(2, 3);

            var from = new ObjectToCompare(9, publicObj, privateVal);
            var to = new ObjectToCompare(12, publicObj, privateVal);

            Assert.IsTrue(ObjectComparisonExtensions.EqualByPublicProperties(from, to, ObjectToCompare.PubPropName));
        }
    }
}

#endif
