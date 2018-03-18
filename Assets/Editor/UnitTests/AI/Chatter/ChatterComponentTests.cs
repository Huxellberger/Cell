// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using System.Linq;
using Assets.Editor.UnitTests.Messaging;
using Assets.Scripts.AI.Chatter;
using Assets.Scripts.Instance;
using Assets.Scripts.Localisation;
using Assets.Scripts.Services;
using Assets.Scripts.Services.EventsOfInterest;
using Assets.Scripts.Test.AI.Chatter;
using Assets.Scripts.Test.Input;
using Assets.Scripts.Test.Instance;
using Assets.Scripts.Test.Services;
using Assets.Scripts.Test.Services.EventsOfInterest;
using Assets.Scripts.UI.HUD.Conversation;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.AI.Chatter
{
    [TestFixture]
    public class ChatterComponentTestFixture
    {
        public const string SpritePath = "Test/Sprites/TestSprite";

        private TestChatterComponent _chatter;
        private MockEventsOfInterestService _events;
        private DialogueData _data;

        [SetUp]
        public void BeforeTest()
        {
            new GameObject().AddComponent<TestGameServiceProvider>().TestAwake();
            _events = new MockEventsOfInterestService();

            GameServiceProvider.CurrentInstance.AddService<IEventsOfInterestServiceInterface>(_events);

            _chatter = new GameObject().AddComponent<TestChatterComponent>();
            var input = new GameObject().AddComponent<MockInputComponent>();
            input.gameObject.AddComponent<TestGameInstance>().TestAwake();

            _data = ScriptableObject.CreateInstance<DialogueData>();
            _data.DialogueEntries = new List<DialogueEntry>
            {
                new DialogueEntry
                {
                    DialogueEntryKey = "TestKey",
                    Lines = new List<DialogueLineEntry>
                    {
                        new DialogueLineEntry
                        {
                            DialogueKey = new LocalisationKey("TestNamespace", "TestValue"),
                            NameKey = new LocalisationKey("TestNamespaceNameKey", "TestNameValue"),
                            DialogueSpeed = 1.0f,
                            Portrait = Resources.Load<Sprite>(SpritePath),
                            TalkNoise = new AudioClip()
                        },
                        new DialogueLineEntry
                        {
                            DialogueKey = new LocalisationKey("TestNamespace2", "TestValue2"),
                            NameKey = new LocalisationKey("TestNamespaceNameKey2", "TestNameValue2"),
                            DialogueSpeed = 2.0f,
                            Portrait = Resources.Load<Sprite>(SpritePath),
                            TalkNoise = new AudioClip()
                        }
                    },
                    Priority = 1,
                    ChatterType = EEventOfInterestType.OneShot
                },
                new DialogueEntry
                {
                    DialogueEntryKey = "OtherTestKey",
                    Lines = new List<DialogueLineEntry>
                    {
                        new DialogueLineEntry
                        {
                            DialogueKey = new LocalisationKey("OtherTestNamespace", "TestValue"),
                            NameKey = new LocalisationKey("OtherTestNamespaceNameKey", "TestNameValue"),
                            DialogueSpeed = 1.0f,
                            Portrait = Resources.Load<Sprite>(SpritePath),
                            TalkNoise = new AudioClip()
                        },
                        new DialogueLineEntry
                        {
                            DialogueKey = new LocalisationKey("OtherTestNamespace2", "TestValue2"),
                            NameKey = new LocalisationKey("OtherTestNamespaceNameKey2", "TestNameValue2"),
                            DialogueSpeed = 2.0f,
                            Portrait = Resources.Load<Sprite>(SpritePath),
                            TalkNoise = new AudioClip()
                        }
                    },
                    Priority = 1,
                    ChatterType = EEventOfInterestType.Persistant
                }
            };
            _chatter.ChatterData = _data;
        }
	
        [TearDown]
        public void AfterTest()
        {
            _data = null;
            GameInstance.ClearGameInstance();
            _chatter = null;

            GameServiceProvider.ClearGameServiceProvider();
        }
	
        [Test]
        public void Start_RegistersForEventsOfInterestMatchingKeys() 
        {
            _chatter.TestStart();

            foreach (var dialogue in _data.DialogueEntries)
            {
                Assert.IsTrue(_events.ListenedEvents.Exists((entry) => entry.EventKey.Equals(dialogue.DialogueEntryKey) && entry.EventType == dialogue.ChatterType));
            }

            _chatter.TestDestroy();
        }

        [Test]
        public void OnEventOfInterest_SendsMatchingUIEvent()
        {
            _chatter.TestStart();

            var messageSpy = new UnityTestMessageHandleResponseObject<RequestDialogueUIMessage>();

            var handle = GameInstance.CurrentInstance.GetUIMessageDispatcher()
                .RegisterForMessageEvent<RequestDialogueUIMessage>(messageSpy.OnResponse);

            var responseEvent = _events.ListenedEvents.Last();
            var matchingDialogue =
                _data.DialogueEntries.Find((entry) => entry.DialogueEntryKey.Equals(responseEvent.EventKey));

            responseEvent.Response(responseEvent.EventKey);

            Assert.AreEqual(matchingDialogue.Priority, messageSpy.MessagePayload.Priority);
            foreach (var line in matchingDialogue.Lines)
            {
                Assert.IsTrue(messageSpy.MessagePayload.Lines.Contains(line));
            }

            GameInstance.CurrentInstance.GetUIMessageDispatcher().UnregisterForMessageEvent(handle);

            _chatter.TestDestroy();
        }

        [Test]
        public void OnDestroy_UnregistersForEventsOfInterestMatchingKeys()
        {
            _chatter.TestStart();
            _chatter.TestDestroy();

            foreach (var dialogue in _data.DialogueEntries)
            {
                Assert.IsTrue(_events.StopListeningEvents.Exists((entry) => entry.EventKey.Equals(dialogue.DialogueEntryKey) && entry.EventType == dialogue.ChatterType));
            }
        }
    }
}
