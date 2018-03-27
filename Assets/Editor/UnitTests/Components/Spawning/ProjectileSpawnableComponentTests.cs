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
        private Rigidbody2D _rigidbody;
        private GameObject _collidingObject;

        private MockSpawnerComponent _spawner;

        [SetUp]
        public void BeforeTest()
        {
            _spawner = new GameObject().AddComponent<MockSpawnerComponent>();

            _rigidbody = new GameObject().AddComponent<Rigidbody2D>();
            _rigidbody.gameObject.AddComponent<AudioSource>();
            _projectile = _rigidbody.gameObject.AddComponent<TestProjectileSpawnableComponent>();
            _projectile.SetSpawner(_spawner);
            _projectile.SpawnAudioClip = new AudioClip();
            _projectile.CollisionAudioClip = new AudioClip();

            _collidingObject = new GameObject();

            _projectile.TestAwake();
        }

        [TearDown]
        public void AfterTest()
        {
            _collidingObject = null;
            _projectile = null;
            _rigidbody = null;

            _spawner = null;
        }

        [Test]
        public void OnSpawned_VelocitySetToUpVectorBySpeed()
        {
            _projectile.OnSpawned();

            Vector2 expectedVelocity = _projectile.transform.up * _projectile.Speed;
            Assert.AreEqual(expectedVelocity, _rigidbody.velocity);
        }

        [Test]
        public void OnSpawned_PlaysSpawnedSound()
        {
            _projectile.OnSpawned();

            Assert.AreSame(_projectile.SpawnAudioClip, _projectile.LastPlayedClip);
        }

        [Test]
        public void OnCollision_Null_DoesNotRequestRespawn()
        {
            _projectile.TestCollide(null);

            Assert.IsNull(_spawner.RequestRespawnGameObject);
        }

        [Test]
        public void OnCollision_Null_DoesNotPlayCollisionSound()
        {
            _projectile.TestCollide(null);

            Assert.IsNull(_projectile.LastPlayedClip);
        }

        [Test]
        public void OnCollision_WrongLayer_DoesNotRequestRespawn()
        {
            _collidingObject.layer = 0;
            _projectile.HitLayers.value = 2;
            _projectile.TestCollide(_collidingObject);

            Assert.IsNull(_spawner.RequestRespawnGameObject);
        }

        [Test]
        public void OnCollision_WrongLayer_DoesNotPlayCollisionSound()
        {
            _collidingObject.layer = 0;
            _projectile.HitLayers.value = 2;
            _projectile.TestCollide(_collidingObject);

            Assert.IsNull(_projectile.LastPlayedClip);
        }

        [Test]
        public void OnCollision_RequestsRespawn()
        {
            _collidingObject.layer = 0;
            _projectile.HitLayers.value = 1;
            _projectile.TestCollide(_collidingObject);

            Assert.AreSame(_spawner.RequestRespawnGameObject, _projectile.gameObject);
        }

        [Test]
        public void OnCollision_PlaysCollisionSound()
        {
            _collidingObject.layer = 0;
            _projectile.HitLayers.value = 1;
            _projectile.TestCollide(_collidingObject);

            Assert.AreSame(_projectile.CollisionAudioClip, _projectile.LastPlayedClip);
        }
    }
}
