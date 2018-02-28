// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Components.ActionStateMachine.ConditionRunner.Conditions;
using Assets.Scripts.Input;
using Assets.Scripts.Test.Input;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine.ConditionRunner.Conditions
{
    [TestFixture]
    public class InputReceivedActionStateConditionTestFixture
    {
        private readonly List<EInputKey> _expectedInputs = new List<EInputKey> {EInputKey.JumpButton, EInputKey.HorizontalAnalog};
        private readonly EInputKey _unexpectedInput = EInputKey.VerticalAnalog;

        [Test]
        public void Start_RegistersHandler()
        {
            var inputBinder = new GameObject().AddComponent<MockInputBinderComponent>();

            var condition = new InputReceivedActionStateCondition(_expectedInputs, inputBinder);

            condition.Start();

            Assert.AreEqual(1, inputBinder.RegisteredHandlers.Count);
        }

        [Test]
        public void ReceivesWrongInput_DoesNotComplete()
        {
            var inputBinder = new GameObject().AddComponent<MockInputBinderComponent>();

            var condition = new InputReceivedActionStateCondition(_expectedInputs, inputBinder);

            condition.Start();

            inputBinder.RegisteredHandlers[0].HandleButtonInput(_unexpectedInput, true);

            Assert.IsFalse(condition.Complete);
        }

        [Test]
        public void ReceivesCorrectInput_Completes()
        {
            var inputBinder = new GameObject().AddComponent<MockInputBinderComponent>();

            var condition = new InputReceivedActionStateCondition(_expectedInputs, inputBinder);

            condition.Start();

            inputBinder.RegisteredHandlers[0].HandleButtonInput(_expectedInputs.First(), true);

            Assert.IsTrue(condition.Complete);
        }

        [Test]
        public void End_UnregistersHandler()
        {
            var inputBinder = new GameObject().AddComponent<MockInputBinderComponent>();

            var condition = new InputReceivedActionStateCondition(_expectedInputs, inputBinder);

            condition.Start();
            condition.End();

            Assert.AreEqual(1, inputBinder.UnregisteredHandlers.Count);
        }
    }
}

#endif
