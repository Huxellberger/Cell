// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Power;
using Assets.Scripts.Input;
using Assets.Scripts.Test.Components.Power;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Power
{
    [TestFixture]
    public class PowerInputHandlerTests
    {
        private MockPowerAssignmentComponent _assignment;

        [SetUp]
        public void BeforeTest()
        {
            _assignment = new GameObject().AddComponent<MockPowerAssignmentComponent>();
        }
	
        [TearDown]
        public void AfterTest()
        {
            _assignment = null;
        }

        [Test]
        public void ReceivesPrimaryPower_PowerAssignmentInterface_ActivatesPrimaryPower()
        {
            var powerHandler = new PowerInputHandler(_assignment);

            powerHandler.HandleButtonInput(EInputKey.PrimaryPower, true);

            Assert.AreEqual(EPowerSetting.Primary, _assignment.ActivatePowerSetting);
        }

        [Test]
        public void ReceivesPrimaryPower_PowerAssignmentInterface_False_DoesNotActivatePower()
        {
            var powerHandler = new PowerInputHandler(_assignment);

            powerHandler.HandleButtonInput(EInputKey.PrimaryPower, false);

            Assert.IsNull(_assignment.ActivatePowerSetting);
        }

        [Test]
        public void ReceivesPrimaryPower_PowerAssignmentInterface_False_ReturnsHandled()
        {
            var powerHandler = new PowerInputHandler(_assignment);

            Assert.AreEqual(EInputHandlerResult.Handled, powerHandler.HandleButtonInput(EInputKey.PrimaryPower, false));
        }

        [Test]
        public void ReceivesPrimaryPower_PowerAssignmentInterface_ReturnsHandled()
        {
            var powerHandler = new PowerInputHandler(_assignment);

            Assert.AreEqual(EInputHandlerResult.Handled, powerHandler.HandleButtonInput(EInputKey.PrimaryPower, true));
        }

        [Test]
        public void ReceivesPrimaryPower_NoPowerAssignmentInterface_DoesNotActivatePower()
        {
            var powerHandler = new PowerInputHandler(null);

            powerHandler.HandleButtonInput(EInputKey.PrimaryPower, false);

            Assert.IsNull(_assignment.ActivatePowerSetting);
        }

        [Test]
        public void ReceivesPrimaryPower_NoPowerAssignmentInterface_ReturnsUnhandled()
        {
            var powerHandler = new PowerInputHandler(null);

            Assert.AreEqual(EInputHandlerResult.Unhandled, powerHandler.HandleButtonInput(EInputKey.PrimaryPower, true));
        }

        [Test]
        public void ReceivesSecondaryPower_PowerAssignmentInterface_ActivatesSecondaryPower()
        {
            var powerHandler = new PowerInputHandler(_assignment);

            powerHandler.HandleButtonInput(EInputKey.SecondaryPower, true);

            Assert.AreEqual(EPowerSetting.Secondary, _assignment.ActivatePowerSetting);
        }

        [Test]
        public void ReceivesSecondaryPower_PowerAssignmentInterface_False_DoesNotActivatePower()
        {
            var powerHandler = new PowerInputHandler(_assignment);

            powerHandler.HandleButtonInput(EInputKey.SecondaryPower, false);

            Assert.IsNull(_assignment.ActivatePowerSetting);
        }

        [Test]
        public void ReceivesSecondaryPower_PowerAssignmentInterface_False_ReturnsHandled()
        {
            var powerHandler = new PowerInputHandler(_assignment);

            Assert.AreEqual(EInputHandlerResult.Handled, powerHandler.HandleButtonInput(EInputKey.SecondaryPower, false));
        }

        [Test]
        public void ReceivesSecondaryPower_PowerAssignmentInterface_ReturnsHandled()
        {
            var powerHandler = new PowerInputHandler(_assignment);

            Assert.AreEqual(EInputHandlerResult.Handled, powerHandler.HandleButtonInput(EInputKey.SecondaryPower, true));
        }

        [Test]
        public void ReceivesSecondaryPower_NoPowerAssignmentInterface_DoesNotActivatePower()
        {
            var powerHandler = new PowerInputHandler(null);

            powerHandler.HandleButtonInput(EInputKey.SecondaryPower, false);

            Assert.IsNull(_assignment.ActivatePowerSetting);
        }

        [Test]
        public void ReceivesSecondaryPower_NoPowerAssignmentInterface_ReturnsUnhandled()
        {
            var powerHandler = new PowerInputHandler(null);

            Assert.AreEqual(EInputHandlerResult.Unhandled, powerHandler.HandleButtonInput(EInputKey.SecondaryPower, true));
        }
    }
}
