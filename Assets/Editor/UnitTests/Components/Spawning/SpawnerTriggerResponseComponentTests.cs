// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Trigger;
using Assets.Scripts.Messaging;
using Assets.Scripts.Test.Components.Spawning;
using Assets.Scripts.Test.Messaging;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Spawning
{
    [TestFixture]
    public class SpawnerTriggerResponseComponentTestFixture
    {
        private MockSpawnerComponent _spawner;
        private TestSpawnerTriggerResponseComponent _triggerResponse;

        [SetUp]
        public void BeforeTest()
        {
            _spawner = new GameObject().AddComponent<MockSpawnerComponent>();
            _triggerResponse = _spawner.gameObject.AddComponent<TestSpawnerTriggerResponseComponent>();

            _triggerResponse.TriggerObject = new GameObject();
            _triggerResponse.TriggerObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();

            _triggerResponse.TestStart();
        }

        [TearDown]
        public void AfterTest()
        {
            _triggerResponse.TestDestroy();

            _triggerResponse = null;
            _spawner = null;
        }

        [Test]
        public void ReceiveTrigger_SendSpawnRequest()
        {
            TestTrigger();

            Assert.IsTrue(_spawner.SpawnCalled);
        }

        private void TestTrigger()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_triggerResponse.TriggerObject, new TriggerMessage(null));
        }
    }
}
