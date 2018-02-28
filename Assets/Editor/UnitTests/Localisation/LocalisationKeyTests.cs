// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Localisation;
using NUnit.Framework;

namespace Assets.Editor.UnitTests.Localisation
{
    [TestFixture]
    public class LocalisationKeyTestFixture
    {
        [Test]
        public void Created_UsesNamespaceAndKeyProvided()
        {
            const string expectedNamespace = "TestNameSpace";
            const string expectedKey = "TestKey";

            var localisationKey = new LocalisationKey(expectedNamespace, expectedKey);

            Assert.IsTrue(localisationKey.LocalisationNamespace.Equals(expectedNamespace));
            Assert.IsTrue(localisationKey.LocalisationKeyValue.Equals(expectedKey));
        }

        [Test]
        public void Equals_MatchingKeyAndNamespace_True()
        {
            const string expectedNamespace = "TestNameSpace";
            const string expectedKey = "TestKey";

            var localisationKey = new LocalisationKey(expectedNamespace, expectedKey);
            var otherLocalisationKey = new LocalisationKey(expectedNamespace, expectedKey);

            Assert.IsTrue(otherLocalisationKey.Equals(localisationKey));
        }

        [Test]
        public void Equals_MatchingKeyNotNamespace_False()
        {
            const string expectedNamespace = "TestNameSpace";
            const string expectedKey = "TestKey";

            var localisationKey = new LocalisationKey(expectedNamespace, expectedKey);
            var otherLocalisationKey = new LocalisationKey("CRAAAAP", expectedKey);

            Assert.IsFalse(otherLocalisationKey.Equals(localisationKey));
        }

        [Test]
        public void Equals_NotMatchingKeyButMatchingNamespace_False()
        {
            const string expectedNamespace = "TestNameSpace";
            const string expectedKey = "TestKey";

            var localisationKey = new LocalisationKey(expectedNamespace, expectedKey);
            var otherLocalisationKey = new LocalisationKey(expectedNamespace, "CRAAAAP");

            Assert.IsFalse(otherLocalisationKey.Equals(localisationKey));
        }

        [Test]
        public void ToString_NamespaceFollowedByKey()
        {
            const string expectedNamespace = "TestNameSpace";
            const string expectedKey = "TestKey";

            var localisationKey = new LocalisationKey(expectedNamespace, expectedKey);

            Assert.IsTrue(localisationKey.ToString().Equals(expectedNamespace + ", " + expectedKey));
        }

        [Test]
        public void GetHashCode_NamespaceMultipliedByKeySquared()
        {
            const string expectedNamespace = "TestNameSpace";
            const string expectedKey = "TestKey";

            var localisationKey = new LocalisationKey(expectedNamespace, expectedKey);

            Assert.AreEqual(expectedNamespace.GetHashCode() * expectedKey.GetHashCode() * expectedKey.GetHashCode(), localisationKey.GetHashCode());
        }
    }
}
