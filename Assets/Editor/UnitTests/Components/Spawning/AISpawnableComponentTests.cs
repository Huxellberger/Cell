// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.ActionStateMachine.States.Dead;
using Assets.Scripts.Messaging;
using Assets.Scripts.Test.Components.ActionStateMachine;
using Assets.Scripts.Test.Components.Health;
using Assets.Scripts.Test.Components.Spawning;
using Assets.Scripts.Test.Messaging;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Spawning
{
    [TestFixture]
    public class AISpawnableComponentTestFixture
    {
        private MockHealthComponent _health;
        private MockActionStateMachineComponent _actionStateMachine;
        private TestAISpawnableComponent _spawnable;

        private MockSpawnerComponent _spawner;

        [SetUp]
        public void BeforeTest()
        {
            _spawner = new GameObject().AddComponent<MockSpawnerComponent>();

            _health = new GameObject().AddComponent<MockHealthComponent>();
            _actionStateMachine = _health.gameObject.AddComponent<MockActionStateMachineComponent>();
            _health.gameObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();
            _spawnable = _health.gameObject.AddComponent<TestAISpawnableComponent>();
            _spawnable.SetSpawner(_spawner);
        }

        [TearDown]
        public void AfterTest()
        {
            _spawnable = null;
            _actionStateMachine = null;
            _health = null;

            _spawner = null;
        }

        [Test]
        public void Start_Spawner_DoesNotRefreshesHealth()
        {
            _spawnable.TestStart();

            Assert.IsFalse(_health.ReplenishHealthCalled);

            _spawnable.TestDestroy();
        }

        [Test]
        public void Start_Spawner_DoesNotSetsInitialActionStateToNull()
        {
            _spawnable.TestStart();

            Assert.IsNull(_actionStateMachine.RequestedId);

            _spawnable.TestDestroy();
        }

        [Test]
        public void Start_NoSpawner_RefreshesHealth()
        {
            _spawnable.SetSpawner(null);
            _spawnable.TestStart();

            Assert.IsTrue(_health.ReplenishHealthCalled);

            _spawnable.TestDestroy();
        }

        [Test]
        public void Start_NoSpawner_SetsInitialActionStateToNull()
        {
            _spawnable.SetSpawner(null);
            _spawnable.TestStart();

            Assert.AreEqual(EActionStateMachineTrack.Locomotion, _actionStateMachine.RequestedTrack);
            Assert.AreEqual(EActionStateId.Null, _actionStateMachine.RequestedId);
            Assert.AreSame(_actionStateMachine.gameObject, _actionStateMachine.RequestedInfo.Owner);

            _spawnable.TestDestroy();
        }

        [Test]
        public void OnSpawned_RefreshesHealth()
        {
            _spawnable.TestStart();
            _spawnable.OnSpawned();

            Assert.IsTrue(_health.ReplenishHealthCalled);

            _spawnable.TestDestroy();
        }

        [Test]
        public void OnSpawned_SetsInitialActionStateToNull()
        {
            _spawnable.TestStart();
            _spawnable.OnSpawned();

            Assert.AreEqual(EActionStateMachineTrack.Locomotion, _actionStateMachine.RequestedTrack);
            Assert.AreEqual(EActionStateId.Null, _actionStateMachine.RequestedId);
            Assert.AreSame(_actionStateMachine.gameObject, _actionStateMachine.RequestedInfo.Owner);

            _spawnable.TestDestroy();
        }

        [Test]
        public void OnEnterDeadActionStateMessage_RequestsRespawn()
        {
            _spawnable.TestStart();
            _spawnable.OnSpawned();

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_spawnable.gameObject, new EnterDeadActionStateMessage());

            Assert.AreSame(_spawnable.gameObject, _spawner.RequestRespawnGameObject);

            _spawnable.TestDestroy();
        }
    }
}
