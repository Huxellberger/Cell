// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Test.Components.ActionStateMachine;
using Assets.Scripts.Test.Components.ActionStateMachine.States.SurfaceSticking;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine.States.SurfaceSticking
{
    [TestFixture]
    public class StickySurfaceComponentTestFixture
    {
        private TestStickySurfaceComponent _stickySurface;
        private MockActionStateMachineComponent _actionStateMachine;

        [SetUp]
        public void BeforeTest()
        {
            _actionStateMachine = new GameObject().AddComponent<MockActionStateMachineComponent>();

            _stickySurface = new GameObject().AddComponent<TestStickySurfaceComponent>();
        }

        [TearDown]
        public void AfterTest()
        {
            _stickySurface = null;
            _actionStateMachine = null;
        }

        [Test]
        public void StopsColliding_StickySurfaceState_EntersLocomotion()
        {
            _actionStateMachine.IsActionStateActiveResult = true;
            _stickySurface.TestStopColliding(_actionStateMachine.gameObject);

            Assert.AreEqual(EActionStateMachineTrack.Locomotion, _actionStateMachine.RequestedTrack);
            Assert.AreEqual(EActionStateId.Locomotion, _actionStateMachine.RequestedId);
            Assert.AreSame(_actionStateMachine.gameObject, _actionStateMachine.RequestedInfo.Owner);
        }

        [Test]
        public void StopsColliding_StickySurfaceState_QueriesStickySurface()
        {
            _actionStateMachine.IsActionStateActiveResult = true;
            _stickySurface.TestStopColliding(_actionStateMachine.gameObject);

            Assert.AreEqual(EActionStateMachineTrack.Locomotion, _actionStateMachine.IsActionStateActiveTrackQuery);
            Assert.AreEqual(EActionStateId.SurfaceSticking, _actionStateMachine.IsActionStateActiveIdQuery);
        }

        [Test]
        public void StopsColliding_NoStickySurfaceState_DoesNotEnterLocomotion()
        {
            _actionStateMachine.IsActionStateActiveResult = false;
            _stickySurface.TestStopColliding(_actionStateMachine.gameObject);

            Assert.IsNull(_actionStateMachine.RequestedId);
        }
    }
}
