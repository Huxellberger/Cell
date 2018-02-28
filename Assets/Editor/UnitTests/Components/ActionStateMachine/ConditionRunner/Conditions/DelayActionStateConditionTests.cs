// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.ActionStateMachine.ConditionRunner.Conditions;
using NUnit.Framework;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine.ConditionRunner.Conditions
{
    [TestFixture]
    public class DelayActionStateConditionTestFixture
    {

        [Test]
        public void ConstructedWithZeroDelay_CompletesAfterFirstUpdate()
        {
            var condition = new DelayActionStateCondition(0.0f);

            Assert.IsFalse(condition.Complete);

            condition.Update(0.0f);

            Assert.IsTrue(condition.Complete);
        }

        [Test]
        public void ConstructedWithDelay_NotCompleted()
        {
            var condition = new DelayActionStateCondition(1.0f);

            Assert.IsFalse(condition.Complete);
        }

        [Test]
        public void ConstructedWithDelay_UpdateLessThanDelay_DoesNotComplete()
        {
            const float delay = 5.0f;

            var condition = new DelayActionStateCondition(delay);

            condition.Update(delay / 2);

            Assert.IsFalse(condition.Complete);
        }

        [Test]
        public void ConstructedWithDelay_CompletesAfterUpdateTimeExceedsFullDelay()
        {
            const float delay = 4.0f;

            var condition = new DelayActionStateCondition(delay);

            condition.Update(delay / 2);
            condition.Update(delay / 2);

            Assert.IsTrue(condition.Complete);
        }
    }
}

#endif
