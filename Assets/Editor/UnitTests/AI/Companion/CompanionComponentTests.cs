// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Editor.UnitTests.Messaging;
using Assets.Scripts.AI.Chatter;
using Assets.Scripts.AI.Companion;
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
        public const string SpritePath = "Test/Sprites/TestSprite";

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
            _companion.CompanionUIIcon = Resources.Load<Sprite>(SpritePath);
            _companion.MaxPowerCharges = 3;
            _companion.CompanionAssetReference = "AssetReference";

            _companion.DialogueEntries.DialogueEntries = new List<DialogueEntry>
            {
                new DialogueEntry{DialogueEntryKey = _companion.DefaultDialogueEntry, ChatterType = EEventOfInterestType.OneShot, Lines = new List<DialogueLineEntry>(), Priority = 2},
                new DialogueEntry{DialogueEntryKey = "Blah"}
            };

            _companion.TestAwake();
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
            _companion.SetLeader(new GameObject());
            _companion.CanUseCompanionPowerImplResult = false;
            Assert.IsFalse(_companion.CanUseCompanionPower());
        }

        [Test]
        public void CanUseCompanionPower_ImplReturnsTrueAndLeader_True()
        {
            _companion.SetLeader(new GameObject());
            _companion.CanUseCompanionPowerImplResult = true;
            Assert.IsTrue(_companion.CanUseCompanionPower());
        }

        [Test]
        public void CanUseCompanionPower_ImplReturnsTrueAndLeaderCleared_False()
        {
            _companion.SetLeader(new GameObject());
            _companion.ClearLeader();
            _companion.CanUseCompanionPowerImplResult = true;
            Assert.IsFalse(_companion.CanUseCompanionPower());
        }

        [Test]
        public void CanUseCompanionPower_ImplReturnsTrueAndLeaderButOnCooldown_False()
        {
            _companion.SetLeader(new GameObject());
            _companion.CanUseCompanionPowerImplResult = true;
            _companion.UseCompanionPower();
            Assert.IsFalse(_companion.CanUseCompanionPower());
        }

        [Test]
        public void CanUseCompanionPower_NoChargesRemain_False()
        {
            _companion.SetLeader(new GameObject());
            _companion.CanUseCompanionPowerImplResult = true;

            for (var i = 0; i < _companion.MaxPowerCharges; i++)
            {
                _companion.UseCompanionPower();
                _companion.TestUpdate(_companion.PowerCooldownTime + 0.1f);
            }

            Assert.IsFalse(_companion.CanUseCompanionPower());
        }

        [Test]
        public void CanUseCompanionPower_UnlimitedCharges_True()
        {
            _companion.SetLeader(new GameObject());
            _companion.CanUseCompanionPowerImplResult = true;
            _companion.MaxPowerCharges = CompanionConstants.UnlimitedCharges;
            _companion.TestAwake();

            Assert.IsTrue(_companion.CanUseCompanionPower());
        }

        [Test]
        public void CanUseCompanionPower_FinishedCoolingDown_True()
        {
            _companion.SetLeader(new GameObject());
            _companion.CanUseCompanionPowerImplResult = true;
            _companion.UseCompanionPower();
            _companion.TestUpdate(_companion.PowerCooldownTime + 0.1f);
            Assert.IsTrue(_companion.CanUseCompanionPower());
        }

        [Test]
        public void UseCompanionPower_CanUse_UseImplCalled()
        {
            _companion.SetLeader(new GameObject());
            _companion.CanUseCompanionPowerImplResult = true;
            _companion.UseCompanionPower();
            Assert.IsTrue(_companion.CompanionPowerImplCalled);
        }

        [Test]
        public void UseCompanionPower_CanUse_ChargesReduced()
        {
            _companion.SetLeader(new GameObject());
            _companion.CanUseCompanionPowerImplResult = true;
            _companion.UseCompanionPower();
            Assert.AreEqual(_companion.MaxPowerCharges -1, _companion.GetCompanionData().PowerUseCount);
        }

        [Test]
        public void UseCompanionPower_CannotUse_NoUseImplCalled()
        {
            _companion.SetLeader(new GameObject());
            _companion.CanUseCompanionPowerImplResult = false;
            _companion.UseCompanionPower();
            Assert.IsFalse(_companion.CompanionPowerImplCalled);
        }

        [Test]
        public void UseCompanionPower_CanUse_ChargesNotReduced()
        {
            _companion.SetLeader(new GameObject());
            _companion.CanUseCompanionPowerImplResult = false;
            _companion.UseCompanionPower();
            Assert.AreEqual(_companion.MaxPowerCharges, _companion.GetCompanionData().PowerUseCount);
        }

        [Test]
        public void SetLeader_SetLeaderImplCalled()
        {
            _companion.SetLeader(new GameObject());
            Assert.IsTrue(_companion.OnLeaderSetImplCalled);
        }

        [Test]
        public void SetLeader_AlreadySet_ClearsFirstLeader()
        {
            _companion.SetLeader(new GameObject());
            _companion.SetLeader(new GameObject());

            Assert.IsTrue(_companion.OnLeaderClearedImplCalled);
        }

        [Test]
        public void SetLeader_Null_NoSetImplCalled()
        {
            _companion.SetLeader(null);

            Assert.IsFalse(_companion.OnLeaderSetImplCalled);
        }

        [Test]
        public void ClearLeader_NoLeader_NoClear()
        {
            _companion.ClearLeader();

            Assert.IsFalse(_companion.OnLeaderClearedImplCalled);
        }

        [Test]
        public void ClearLeader_Leader_ClearImplCalled()
        {
            _companion.SetLeader(new GameObject());
            _companion.ClearLeader();

            Assert.IsTrue(_companion.OnLeaderClearedImplCalled);
        }

        [Test]
        public void GetCompanionData_Ready_1()
        {
            Assert.AreEqual(1.0f, _companion.GetCompanionData().PowerCooldown);
        }

        [Test]
        public void GetCompanionData_CorrectSprite()
        {
            Assert.AreSame(_companion.CompanionUIIcon, _companion.GetCompanionData().Image);
        }

        [Test]
        public void GetCompanionData_CorrectPrefabReference()
        {
            Assert.AreEqual(_companion.CompanionAssetReference, _companion.GetCompanionData().CompanionPrefabReference);
        }

        [Test]
        public void GetCompanionData_JustUsed_0()
        {
            _companion.SetLeader(new GameObject());
            _companion.CanUseCompanionPowerImplResult = true;
            _companion.UseCompanionPower();

            Assert.AreEqual(0.0f, _companion.GetCompanionData().PowerCooldown);
        }

        [Test]
        public void GetCompanionData_ScalesToPercentageCooledDown()
        {
            _companion.SetLeader(new GameObject());
            _companion.CanUseCompanionPowerImplResult = true;
            _companion.UseCompanionPower();

            _companion.TestUpdate(_companion.PowerCooldownTime * 0.5f);

            Assert.AreEqual(0.5f, _companion.GetCompanionData().PowerCooldown);
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
