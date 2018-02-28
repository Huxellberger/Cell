// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Test.Components.ActionStateMachine;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine
{
    [TestFixture]
    public class NullActionStateHelpersTestFixture
    {
        [Test]
        public void TransitionIntoNullActionState_SetsNullActionStateActive()
        {
            var actionStateMachineComponent = new GameObject().AddComponent<MockActionStateMachineComponent>();

            NullActionStateHelpers.TransitionIntoNullActionState(actionStateMachineComponent.gameObject);

            Assert.AreEqual(EActionStateMachineTrack.Locomotion, actionStateMachineComponent.RequestedTrack);
            Assert.AreEqual(EActionStateId.Null, actionStateMachineComponent.RequestedId);
        }
    }
}

#endif
