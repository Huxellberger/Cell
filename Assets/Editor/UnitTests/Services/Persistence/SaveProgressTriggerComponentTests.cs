// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Editor.UnitTests.Messaging;
using Assets.Scripts.Messaging;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Persistence;
using Assets.Scripts.Test.Components.ActionStateMachine;
using Assets.Scripts.Test.Components.Character;
using Assets.Scripts.Test.Input;
using Assets.Scripts.Test.Messaging;
using Assets.Scripts.Test.Services;
using Assets.Scripts.Test.Services.Persistence;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Services.Persistence
{
    [TestFixture]
    public class SaveProgressTriggerComponentTestFixture
    {
        private TestSaveProgressTriggerComponent _trigger;
        private TestCharacterComponent _character;

        private string _entityKey;
        private MockPersistentEntityComponent _entity;
        private MockPersistenceService _service;

        [SetUp]
        public void BeforeTest()
        {
            _trigger = new GameObject().AddComponent<TestSaveProgressTriggerComponent>();

            var characterObject = new GameObject();
            characterObject.AddComponent<MockActionStateMachineComponent>();
            characterObject.AddComponent<MockInputBinderComponent>();
            characterObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();

            _character = characterObject.AddComponent<TestCharacterComponent>();

            _entity = new GameObject().AddComponent<MockPersistentEntityComponent>();
            _entityKey = "Whatever";

            _service = new MockPersistenceService
            {
                GetEntitiesResult = new Dictionary<string, IPersistentEntityInterface>
                {
                    {_entityKey, _entity}
                }
            };

            new GameObject().AddComponent<TestGameServiceProvider>().TestAwake();

            GameServiceProvider.CurrentInstance.AddService<IPersistenceServiceInterface>(_service);
        }
	
        [TearDown]
        public void AfterTest()
        {
            GameServiceProvider.ClearGameServiceProvider();

            _service = null;

            _entity = null;

            _character = null;

            _trigger = null;
        }
	
        [Test]
        public void OnCollides_NoCharacterComponent_NoMessageSent()
        {
            var messageSpy =
                new UnityTestMessageHandleResponseObject<SaveGameTriggerActivatedMessage>();

            var collider = new GameObject();
            collider.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<SaveGameTriggerActivatedMessage>(collider,
                    messageSpy.OnResponse);

            _trigger.TestCollide(collider);

            Assert.IsFalse(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(collider, handle);
        }

        [Test]
        public void OnCollides_NoCharacterComponent_NoDataSaved()
        {
            _trigger.TestCollide(new GameObject());

            Assert.IsNull(_entity.WriteDataStream);
        }

        [Test]
        public void OnCollides_CharacterComponent_MessageSent()
        {
            var messageSpy =
                new UnityTestMessageHandleResponseObject<SaveGameTriggerActivatedMessage>();


            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<SaveGameTriggerActivatedMessage>(_character.gameObject,
                    messageSpy.OnResponse);

            _trigger.TestCollide(_character.gameObject);

            Assert.IsTrue(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_character.gameObject, handle);
        }

        [Test]
        public void OnCollides_CharacterComponent_DataSaved()
        {
            _trigger.TestCollide(_character.gameObject);

            Assert.IsNotNull(_entity.WriteDataStream);
        }
    }
}
