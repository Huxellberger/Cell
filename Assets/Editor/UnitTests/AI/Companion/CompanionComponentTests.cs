// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Editor.UnitTests.Messaging;
using Assets.Scripts.AI.Chatter;
using Assets.Scripts.Instance;
using Assets.Scripts.Services.EventsOfInterest;
using Assets.Scripts.Test.AI.Companion;
using Assets.Scripts.Test.Input;
using Assets.Scripts.Test.Instance;
using Assets.Scripts.UI.HUD.Conversation;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.AI.Companion
{
    [TestFixture]
    public class CompanionComponentTestFixture
    {
        private TestCompanionComponent _companion;

        [SetUp]
        public void BeforeTest()
        {
            var mockInput = new GameObject().AddComponent<MockInputComponent>();
            mockInput.gameObject.AddComponent<TestGameInstance>().TestAwake();

            _companion = new GameObject().AddComponent<TestCompanionComponent>();
            _companion.PowerCooldownTime = 2.0f;
            _companion.DefaultDialogueEntry = "TestEntry";
            _companion.DialogueEntries = ScriptableObject.CreateInstance<DialogueData>();

            _companion.DialogueEntries.DialogueEntries = new List<DialogueEntry>
            {
                new DialogueEntry{DialogueEntryKey = _companion.DefaultDialogueEntry, ChatterType = EEventOfInterestType.OneShot, Lines = new List<DialogueLineEntry>(), Priority = 2},
                new DialogueEntry{DialogueEntryKey = "Blah"}
            };

            _companion.TestStart();
        }
	
        [TearDown]
        public void AfterTest()
        {
            _companion = null;

		    GameInstance.ClearGameInstance();
        }
	
        [Test]
        public void CanUseCompanionPower_NoLeader_False()
        {
            _companion.CanUseCompanionPowerImplResult = true;
            Assert.IsFalse(_companion.CanUseCompanionPower());
        }

        [Test]
        public void CanUseCompanionPower_ImplReturnsFalse_False()
        {
            _companion.SetCompanion(new GameObject());
            _companion.CanUseCompanionPowerImplResult = false;
            Assert.IsFalse(_companion.CanUseCompanionPower());
        }

        [Test]
        public void CanUseCompanionPower_ImplReturnsTrueAndLeader_True()
        {
            _companion.SetCompanion(new GameObject());
            _companion.CanUseCompanionPowerImplResult = true;
            Assert.IsTrue(_companion.CanUseCompanionPower());
        }

        [Test]
        public void CanUseCompanionPower_ImplReturnsTrueAndLeaderCleared_False()
        {
            _companion.SetCompanion(new GameObject());
            _companion.ClearCompanion();
            _companion.CanUseCompanionPowerImplResult = true;
            Assert.IsFalse(_companion.CanUseCompanionPower());
        }

        [Test]
        public void CanUseCompanionPower_ImplReturnsTrueAndLeaderButOnCooldown_False()
        {
            _companion.SetCompanion(new GameObject());
            _companion.CanUseCompanionPowerImplResult = true;
            _companion.UseCompanionPower();
            Assert.IsFalse(_companion.CanUseCompanionPower());
        }

        [Test]
        public void CanUseCompanionPower_FinishedCoolingDown_True()
        {
            _companion.SetCompanion(new GameObject());
            _companion.CanUseCompanionPowerImplResult = true;
            _companion.UseCompanionPower();
            _companion.TestUpdate(_companion.PowerCooldownTime + 0.1f);
            Assert.IsTrue(_companion.CanUseCompanionPower());
        }

        [Test]
        public void UseCompanionPower_CanUse_UseImplCalled()
        {
            _companion.SetCompanion(new GameObject());
            _companion.CanUseCompanionPowerImplResult = true;
            _companion.UseCompanionPower();
            Assert.IsTrue(_companion.CompanionPowerImplCalled);
        }

        [Test]
        public void UseCompanionPower_CannotUse_NoUseImplCalled()
        {
            _companion.SetCompanion(new GameObject());
            _companion.CanUseCompanionPowerImplResult = false;
            _companion.UseCompanionPower();
            Assert.IsFalse(_companion.CompanionPowerImplCalled);
        }

        [Test]
        public void SetCompanion_SetCompanionImplCalled()
        {
            _companion.SetCompanion(new GameObject());
            Assert.IsTrue(_companion.OnCompanionSetImplCalled);
        }

        [Test]
        public void SetCompanion_AlreadySet_ClearsFirstCompanion()
        {
            _companion.SetCompanion(new GameObject());
            _companion.SetCompanion(new GameObject());

            Assert.IsTrue(_companion.OnCompanionClearedImplCalled);
        }

        [Test]
        public void SetCompanion_Null_NoSetImplCalled()
        {
            _companion.SetCompanion(null);

            Assert.IsFalse(_companion.OnCompanionSetImplCalled);
        }

        [Test]
        public void ClearCompanion_NoCompanion_NoClear()
        {
            _companion.ClearCompanion();

            Assert.IsFalse(_companion.OnCompanionClearedImplCalled);
        }

        [Test]
        public void ClearCompanion_Companion_ClearImplCalled()
        {
            _companion.SetCompanion(new GameObject());
            _companion.ClearCompanion();

            Assert.IsTrue(_companion.OnCompanionClearedImplCalled);
        }

        [Test]
        public void GetCompanionPowerCooldown_Ready_1()
        {
            Assert.AreEqual(1.0f, _companion.GetCompanionPowerCooldown());
        }

        [Test]
        public void GetCompanionPowerCooldown_JustUsed_0()
        {
            _companion.SetCompanion(new GameObject());
            _companion.CanUseCompanionPowerImplResult = true;
            _companion.UseCompanionPower();

            Assert.AreEqual(0.0f, _companion.GetCompanionPowerCooldown());
        }

        [Test]
        public void GetCompanionPowerCooldown_ScalesToPercentageCooledDown()
        {
            _companion.SetCompanion(new GameObject());
            _companion.CanUseCompanionPowerImplResult = true;
            _companion.UseCompanionPower();

            _companion.TestUpdate(_companion.PowerCooldownTime * 0.5f);

            Assert.AreEqual(0.5f, _companion.GetCompanionPowerCooldown());
        }

        [Test]
        public void RequestDialogue_SendsRequestEventCorrespondingToDefaultMessage()
        {
            var messageSpy = new UnityTestMessageHandleResponseObject<RequestDialogueUIMessage>();

            var handle =
                GameInstance.CurrentInstance.GetUIMessageDispatcher().RegisterForMessageEvent<RequestDialogueUIMessage>(messageSpy.OnResponse);

            _companion.RequestDialogue();

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreSame(_companion.DialogueEntries.DialogueEntries[0].Lines, messageSpy.MessagePayload.Lines);

            GameInstance.CurrentInstance.GetUIMessageDispatcher().UnregisterForMessageEvent(handle);
        }
    }
}
