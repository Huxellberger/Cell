// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Test.Components.Spawning;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Spawning
{
    [TestFixture]
    public class ProjectileSpawnableComponentTestFixture
    {
        private TestProjectileSpawnableComponent _projectile;
        private Rigidbody _rigidbody;

        private MockSpawnerComponent _spawner;

        [SetUp]
        public void BeforeTest()
        {
            _spawner = new GameObject().AddComponent<MockSpawnerComponent>();

            _rigidbody = new GameObject().AddComponent<Rigidbody>();
            _rigidbody.gameObject.AddComponent<AudioSource>();
            _projectile = _rigidbody.gameObject.AddComponent<TestProjectileSpawnableComponent>();
            _projectile.SetSpawner(_spawner);
            _projectile.SpawnAudioClip = new AudioClip();
            _projectile.CollisionAudioClip = new AudioClip();

            _projectile.TestAwake();
        }

        [TearDown]
        public void AfterTest()
        {
            _projectile = null;
            _rigidbody = null;

            _spawner = null;
        }

        [Test]
        public void OnSpawned_VelocitySetToForwardVectorBySpeed()
        {
            _projectile.OnSpawned();

            Assert.AreEqual(_projectile.transform.forward * _projectile.Speed, _rigidbody.velocity);
        }

        [Test]
        public void OnSpawned_PlaysSpawnedSound()
        {
            _projectile.OnSpawned();

            Assert.AreSame(_projectile.SpawnAudioClip, _projectile.LastPlayedClip);
        }

        [Test]
        public void OnCollision_RequestsRespawn()
        {
            _projectile.TestCollide(null);

            Assert.AreSame(_spawner.RequestRespawnGameObject, _projectile.gameObject);
        }

        [Test]
        public void OnCollision_PlaysCollisionSound()
        {
            _projectile.TestCollide(null);

            Assert.AreSame(_projectile.CollisionAudioClip, _projectile.LastPlayedClip);
        }
    }
}
