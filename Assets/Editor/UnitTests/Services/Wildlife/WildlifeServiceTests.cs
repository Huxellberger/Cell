// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Core;
using Assets.Scripts.Services.Wildlife;
using Assets.Scripts.Test.Components.Species;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Editor.UnitTests.Services.Wildlife
{
    [TestFixture]
    public class WildlifeServiceTestFixture
    {
        private WildlifeService _wildlife;
        private MockSpeciesComponent _species;
        private MockSpeciesComponent _otherSpecies;

        [SetUp]
        public void BeforeTest()
        {
            _wildlife = new WildlifeService();

            _species = new GameObject().AddComponent<MockSpeciesComponent>();
            _otherSpecies = new GameObject().AddComponent<MockSpeciesComponent>();
        }

        [TearDown]
        public void AfterTest()
        {
            _otherSpecies = null;
            _species = null;

            _wildlife = null;
        }

        [Test]
        public void RegisterWildlife_NoSpeciesInterface_ErrorsAndNotRegistered()
        {
            LogAssert.Expect(LogType.Error, "Tried to add a wildlife entry that lacks a proper species interface!");

            _wildlife.RegisterWildlife(new GameObject());

            Assert.AreEqual(0, _wildlife.GetWildlifeInRadius(Vector3.zero, 1000f).Count);
        }

        [Test]
        public void RegisterWildlife_ReturnedInRadiusCheck()
        {
            _wildlife.RegisterWildlife(_species.gameObject);

            Assert.AreEqual(1, _wildlife.GetWildlifeInRadius(Vector3.zero, 1000f).Count);
        }

        [Test]
        public void RegisterWildlife_RegistersAllWildlife()
        {
            _wildlife.RegisterWildlife(_species.gameObject);
            _wildlife.RegisterWildlife(_otherSpecies.gameObject);

            Assert.AreEqual(2, _wildlife.GetWildlifeInRadius(Vector3.zero, 1000f).Count);
        }

        [Test]
        public void UnregisterWildlife_NotReturnedInRadiusCheck()
        {
            _wildlife.RegisterWildlife(_species.gameObject);
            _wildlife.UnregisterWildlife(_species.gameObject);

            Assert.AreEqual(0, _wildlife.GetWildlifeInRadius(Vector3.zero, 1000f).Count);
        }

        [Test]
        public void UnregisterWildlife_RemovesCorrectWildlife()
        {
            _wildlife.RegisterWildlife(_species.gameObject);
            _wildlife.RegisterWildlife(_otherSpecies.gameObject);

            _wildlife.UnregisterWildlife(_otherSpecies.gameObject);

            var retrievedWildlife = _wildlife.GetWildlifeInRadius(Vector3.zero, 1000f);
            Assert.AreEqual(1, retrievedWildlife.Count);
            Assert.AreSame(_species, retrievedWildlife[0].Wildlife);
        }

        [Test]
        public void UnregisterWildlife_RemovesCorrectWildlifeGameObject()
        {
            _wildlife.RegisterWildlife(_species.gameObject);
            _wildlife.RegisterWildlife(_otherSpecies.gameObject);

            _wildlife.UnregisterWildlife(_otherSpecies.gameObject);

            var retrievedWildlife = _wildlife.GetWildlifeInRadius(Vector3.zero, 1000f);
            Assert.AreEqual(1, retrievedWildlife.Count);
            Assert.AreSame(_species.gameObject, retrievedWildlife[0].WildlifeGameObject);
        }

        [Test]
        public void GetWildlifeInRadius_OutsideRadius_Excluded()
        {
            _wildlife.RegisterWildlife(_species.gameObject);

            var retrievedWildlife = _wildlife.GetWildlifeInRadius(new Vector3(2.0f, 2.0f, 2.0f), 1f);
            Assert.AreEqual(0, retrievedWildlife.Count);
        }

        [Test]
        public void GetWildlifeInRadius_ReturnsCorrectDistanceSquared()
        {
            var checkLocation = new Vector3(2.0f, 20.0f, -10.0f);

            _wildlife.RegisterWildlife(_species.gameObject);

            var retrievedWildlife = _wildlife.GetWildlifeInRadius(checkLocation, 1000f);
            Assert.AreEqual
            (
                VectorFunctions.DistanceSquared
                (
                    checkLocation, _species.gameObject.transform.position
                ), 
                retrievedWildlife[0].DistanceSquared
            );
        }
    }
}
