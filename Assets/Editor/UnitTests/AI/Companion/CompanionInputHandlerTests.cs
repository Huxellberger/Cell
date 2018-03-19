// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.AI.Companion;
using Assets.Scripts.Input;
using Assets.Scripts.Test.AI.Companion;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.AI.Companion
{
    [TestFixture]
    public class CompanionInputHandlerTestFixture
    {
        private MockCompanionSetComponent _companionSet;

        [SetUp]
        public void BeforeTest()
        {
            _companionSet = new GameObject().AddComponent<MockCompanionSetComponent>();
        }
	
        [TearDown]
        public void AfterTest()
        {
            _companionSet = null;
        }

        [Test]
        public void ReceivesPrimaryPowerButton_CompanionSet_UsesPower()
        {
            var companionHandler = new CompanionInputHandler(_companionSet);

            companionHandler.HandleButtonInput(EInputKey.PrimaryPower, true);

            Assert.AreEqual(ECompanionSlot.Primary, _companionSet.UseCompanionPowerSlotResult);
        }

        [Test]
        public void ReceivesPrimaryPowerButton_CompanionSet_DoesNotUsePower()
        {
            var companionHandler = new CompanionInputHandler(_companionSet);

            companionHandler.HandleButtonInput(EInputKey.PrimaryPower, false);

            Assert.IsNull(_companionSet.UseCompanionPowerSlotResult);
        }

        [Test]
        public void ReceivesPrimaryPowerButton_CompanionSet_False_ReturnsHandled()
        {
            var companionHandler = new CompanionInputHandler(_companionSet);

            Assert.AreEqual(EInputHandlerResult.Handled, companionHandler.HandleButtonInput(EInputKey.PrimaryPower, false));
        }

        [Test]
        public void ReceivesPrimaryPowerButton_CompanionSet_ReturnsHandled()
        {
            var companionHandler = new CompanionInputHandler(_companionSet);

            Assert.AreEqual(EInputHandlerResult.Handled, companionHandler.HandleButtonInput(EInputKey.PrimaryPower, true));
        }

        [Test]
        public void ReceivesPrimaryPowerButton_NoCompanionSet_DoesNotUsePrimaryPower()
        {
            var companionHandler = new CompanionInputHandler(null);

            companionHandler.HandleButtonInput(EInputKey.PrimaryPower, false);

            Assert.IsNull(_companionSet.UseCompanionPowerSlotResult);
        }

        [Test]
        public void ReceivesPrimaryPowerButton_NoCompanionSet_ReturnsUnhandled()
        {
            var companionHandler = new CompanionInputHandler(null);

            Assert.AreEqual(EInputHandlerResult.Unhandled, companionHandler.HandleButtonInput(EInputKey.PrimaryPower, true));
        }

        [Test]
        public void ReceivesSecondaryPowerButton_CompanionSet_UsesPower()
        {
            var companionHandler = new CompanionInputHandler(_companionSet);

            companionHandler.HandleButtonInput(EInputKey.SecondaryPower, true);

            Assert.AreEqual(ECompanionSlot.Secondary, _companionSet.UseCompanionPowerSlotResult);
        }

        [Test]
        public void ReceivesSecondaryPowerButton_CompanionSet_DoesNotUsePower()
        {
            var companionHandler = new CompanionInputHandler(_companionSet);

            companionHandler.HandleButtonInput(EInputKey.SecondaryPower, false);

            Assert.IsNull(_companionSet.UseCompanionPowerSlotResult);
        }

        [Test]
        public void ReceivesSecondaryPowerButton_CompanionSet_False_ReturnsHandled()
        {
            var companionHandler = new CompanionInputHandler(_companionSet);

            Assert.AreEqual(EInputHandlerResult.Handled, companionHandler.HandleButtonInput(EInputKey.SecondaryPower, false));
        }

        [Test]
        public void ReceivesSecondaryPowerButton_CompanionSet_ReturnsHandled()
        {
            var companionHandler = new CompanionInputHandler(_companionSet);

            Assert.AreEqual(EInputHandlerResult.Handled, companionHandler.HandleButtonInput(EInputKey.SecondaryPower, true));
        }

        [Test]
        public void ReceivesSecondaryPowerButton_NoCompanionSet_DoesNotUseSecondaryPower()
        {
            var companionHandler = new CompanionInputHandler(null);

            companionHandler.HandleButtonInput(EInputKey.SecondaryPower, false);

            Assert.IsNull(_companionSet.UseCompanionPowerSlotResult);
        }

        [Test]
        public void ReceivesSecondaryPowerButton_NoCompanionSet_ReturnsUnhandled()
        {
            var companionHandler = new CompanionInputHandler(null);

            Assert.AreEqual(EInputHandlerResult.Unhandled, companionHandler.HandleButtonInput(EInputKey.SecondaryPower, true));
        }

        [Test]
        public void ReceivesPrimaryDialogueButton_CompanionSet_UsesDialogue()
        {
            var companionHandler = new CompanionInputHandler(_companionSet);

            companionHandler.HandleButtonInput(EInputKey.PrimaryDialogue, true);

            Assert.AreEqual(ECompanionSlot.Primary, _companionSet.RequestCompanionDialogueSlotResult);
        }

        [Test]
        public void ReceivesPrimaryDialogueButton_CompanionSet_DoesNotUseDialogue()
        {
            var companionHandler = new CompanionInputHandler(_companionSet);

            companionHandler.HandleButtonInput(EInputKey.PrimaryDialogue, false);

            Assert.IsNull(_companionSet.RequestCompanionDialogueSlotResult);
        }

        [Test]
        public void ReceivesPrimaryDialogueButton_CompanionSet_False_ReturnsHandled()
        {
            var companionHandler = new CompanionInputHandler(_companionSet);

            Assert.AreEqual(EInputHandlerResult.Handled, companionHandler.HandleButtonInput(EInputKey.PrimaryDialogue, false));
        }

        [Test]
        public void ReceivesPrimaryDialogueButton_CompanionSet_ReturnsHandled()
        {
            var companionHandler = new CompanionInputHandler(_companionSet);

            Assert.AreEqual(EInputHandlerResult.Handled, companionHandler.HandleButtonInput(EInputKey.PrimaryDialogue, true));
        }

        [Test]
        public void ReceivesPrimaryDialogueButton_NoCompanionSet_DoesNotUsePrimaryDialogue()
        {
            var companionHandler = new CompanionInputHandler(null);

            companionHandler.HandleButtonInput(EInputKey.PrimaryDialogue, false);

            Assert.IsNull(_companionSet.RequestCompanionDialogueSlotResult);
        }

        [Test]
        public void ReceivesPrimaryDialogueButton_NoCompanionSet_ReturnsUnhandled()
        {
            var companionHandler = new CompanionInputHandler(null);

            Assert.AreEqual(EInputHandlerResult.Unhandled, companionHandler.HandleButtonInput(EInputKey.PrimaryDialogue, true));
        }

        [Test]
        public void ReceivesSecondaryDialogueButton_CompanionSet_UsesDialogue()
        {
            var companionHandler = new CompanionInputHandler(_companionSet);

            companionHandler.HandleButtonInput(EInputKey.SecondaryDialogue, true);

            Assert.AreEqual(ECompanionSlot.Secondary, _companionSet.RequestCompanionDialogueSlotResult);
        }

        [Test]
        public void ReceivesSecondaryDialogueButton_CompanionSet_DoesNotUseDialogue()
        {
            var companionHandler = new CompanionInputHandler(_companionSet);

            companionHandler.HandleButtonInput(EInputKey.SecondaryDialogue, false);

            Assert.IsNull(_companionSet.RequestCompanionDialogueSlotResult);
        }

        [Test]
        public void ReceivesSecondaryDialogueButton_CompanionSet_False_ReturnsHandled()
        {
            var companionHandler = new CompanionInputHandler(_companionSet);

            Assert.AreEqual(EInputHandlerResult.Handled, companionHandler.HandleButtonInput(EInputKey.SecondaryDialogue, false));
        }

        [Test]
        public void ReceivesSecondaryDialogueButton_CompanionSet_ReturnsHandled()
        {
            var companionHandler = new CompanionInputHandler(_companionSet);

            Assert.AreEqual(EInputHandlerResult.Handled, companionHandler.HandleButtonInput(EInputKey.SecondaryDialogue, true));
        }

        [Test]
        public void ReceivesSecondaryDialogueButton_NoCompanionSet_DoesNotUseSecondaryDialogue()
        {
            var companionHandler = new CompanionInputHandler(null);

            companionHandler.HandleButtonInput(EInputKey.SecondaryDialogue, false);

            Assert.IsNull(_companionSet.RequestCompanionDialogueSlotResult);
        }

        [Test]
        public void ReceivesSecondaryDialogueButton_NoCompanionSet_ReturnsUnhandled()
        {
            var companionHandler = new CompanionInputHandler(null);

            Assert.AreEqual(EInputHandlerResult.Unhandled, companionHandler.HandleButtonInput(EInputKey.SecondaryDialogue, true));
        }
    }
}
