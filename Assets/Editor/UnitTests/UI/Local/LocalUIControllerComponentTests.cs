// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Editor.UnitTests.Messaging;
using Assets.Scripts.Components.Emote;
using Assets.Scripts.Messaging;
using Assets.Scripts.Test.Messaging;
using Assets.Scripts.Test.UI.Local;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.UI.Local
{
    [TestFixture]
    public class LocalUIControllerComponentTestFixture
    {
        private TestLocalUIControllerComponent _local;
        private GameObject _prefab;
	
        [SetUp]
        public void BeforeTest()
        {
	        _prefab = new GameObject();
            _prefab.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();

            _local = new GameObject().AddComponent<TestLocalUIControllerComponent>();
            _local.gameObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();

            _local.LocalUIPrefab = _prefab;
            _local.LocalUIOffset = new Vector3(1.0f, 2.0f, 4.0f);

            _local.TestAwake();
        }
	
        [TearDown]
        public void AfterTest()
        {
            _local.TestDestroy();

            _local = null;
            _prefab = null;
        }
	
        [Test]
        public void Awake_InstantiatesPrefabInCorrectSetup()
        {
            var instantiatedUI = _local.GetInstantiatedUI();
            Assert.NotNull(instantiatedUI);

            Assert.AreSame(_local.gameObject.transform, instantiatedUI.gameObject.transform.parent);
            Assert.AreEqual(_local.LocalUIOffset, instantiatedUI.gameObject.transform.localPosition);
        }

        [Test]
        public void ReceivesEmoteStateChangedMessage_PropogatesToUI()
        {
            var messageSpy = new UnityTestMessageHandleResponseObject<EmoteStatusChangedUIMessage>();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<EmoteStatusChangedUIMessage>(_local.GetInstantiatedUI(),
                    messageSpy.OnResponse);

            const EEmoteState expectedState = EEmoteState.Alerted;

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_local.gameObject, new EmoteStatusChangedMessage(expectedState));

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreEqual(expectedState, messageSpy.MessagePayload.State);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_local.GetInstantiatedUI(), handle);
        }
    }
}
