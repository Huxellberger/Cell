// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.ActionStateMachine.States.Locomotion;
using Assets.Scripts.Components.ActionStateMachine.States.PushObjectActionState;
using Assets.Scripts.Test.Components.Objects.Pushable;
using Assets.Scripts.Test.Input;
using Castle.Core.Internal;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine.States.PushObject
{
    [TestFixture]
    public class PushObjectActionStateTestFixture
    {
        private MockPushableObjectComponent _pushableObject;
        private MockInputBinderComponent _inputBinder;
        private Rigidbody _rigidbody;
        private GameObject _pushSocket;

        [SetUp]
        public void BeforeTest()
        {
            _pushableObject = new GameObject().AddComponent<MockPushableObjectComponent>();
            _inputBinder = new GameObject().AddComponent<MockInputBinderComponent>();
            _rigidbody = _inputBinder.gameObject.AddComponent<Rigidbody>();

            _pushSocket = new GameObject();
            _pushSocket.transform.Rotate(1.0f, 2.0f, 3.0f);
            _pushSocket.transform.position = new Vector3(1.0f, 2.0f, 3.0f);
        }

        [TearDown]
        public void AfterTest()
        {
            _pushSocket = null;

            _rigidbody = null;
            _inputBinder = null;
            _pushableObject = null;
        }

        [Test]
        public void Creation_HasPushObjectID()
        {
            var actionState = new PushObjectActionState(new PushObjectActionStateInfo());

            Assert.AreEqual(EActionStateId.PushObject, actionState.ActionStateId);
        }

        [Test]
        public void Start_SetsOwnerParentToPushSocket()
        {
            var actionState = new PushObjectActionState(new PushObjectActionStateInfo(_inputBinder.gameObject, _pushableObject.gameObject, _pushSocket));
            actionState.Start();

            Assert.AreSame(_pushSocket.transform, _inputBinder.gameObject.transform.parent);
        }

        [Test]
        public void Start_SetsOwnerLocationToPushSocket()
        {
            var actionState = new PushObjectActionState(new PushObjectActionStateInfo(_inputBinder.gameObject, _pushableObject.gameObject, _pushSocket));
            actionState.Start();

            Assert.AreEqual(_pushSocket.transform.position, _inputBinder.gameObject.transform.position);
        }

        [Test]
        public void Start_SetsOwnerRotationToPushSocket()
        {
            var actionState = new PushObjectActionState(new PushObjectActionStateInfo(_inputBinder.gameObject, _pushableObject.gameObject, _pushSocket));
            actionState.Start();

            Assert.AreEqual(_pushSocket.transform.rotation, _inputBinder.gameObject.transform.rotation);
        }

        [Test]
        public void Start_SetsRigidbodyConstraintsToFreezeAll()
        {
            var actionState = new PushObjectActionState(new PushObjectActionStateInfo(_inputBinder.gameObject, _pushableObject.gameObject, _pushSocket));
            actionState.Start();

            Assert.AreEqual(RigidbodyConstraints.FreezeAll, _rigidbody.constraints);
        }

        [Test]
        public void Start_RegistersPushObjectInputHandler()
        {
            var actionState = new PushObjectActionState(new PushObjectActionStateInfo(_inputBinder.gameObject, _pushableObject.gameObject, _pushSocket));
            actionState.Start();

            Assert.IsTrue(_inputBinder.IsHandlerOfTypeRegistered<PushObjectInputHandler>());
        }

        [Test]
        public void Start_RegistersInteractionInputHandler()
        {
            var actionState = new PushObjectActionState(new PushObjectActionStateInfo(_inputBinder.gameObject, _pushableObject.gameObject, _pushSocket));
            actionState.Start();

            Assert.IsTrue(_inputBinder.IsHandlerOfTypeRegistered<InteractionInputHandler>());
        }

        [Test]
        public void End_SetsOwnerParentToNull()
        {
            var actionState = new PushObjectActionState(new PushObjectActionStateInfo(_inputBinder.gameObject, _pushableObject.gameObject, _pushSocket));
            actionState.Start();
            actionState.End();

            Assert.IsNull(_inputBinder.gameObject.transform.parent);
        }

        [Test]
        public void End_SetsRigidbodyConstraintsToPrior()
        {
            const RigidbodyConstraints expectedConstraints = RigidbodyConstraints.FreezePositionY;

            _rigidbody.constraints = expectedConstraints;

            var actionState = new PushObjectActionState(new PushObjectActionStateInfo(_inputBinder.gameObject, _pushableObject.gameObject, _pushSocket));
            actionState.Start();
            actionState.End();

            Assert.AreEqual(expectedConstraints, _rigidbody.constraints);
        }

        [Test]
        public void End_UnregistersPushObjectInputHandler()
        {
            var actionState = new PushObjectActionState(new PushObjectActionStateInfo(_inputBinder.gameObject, _pushableObject.gameObject, _pushSocket));
            actionState.Start();

            actionState.End();

            Assert.IsTrue(_inputBinder.IsHandlerOfTypeUnregistered<PushObjectInputHandler>());
        }

        [Test]
        public void End_UnregistersInteractionInputHandler()
        {
            var actionState = new PushObjectActionState(new PushObjectActionStateInfo(_inputBinder.gameObject, _pushableObject.gameObject, _pushSocket));
            actionState.Start();

            actionState.End();

            Assert.IsTrue(_inputBinder.IsHandlerOfTypeUnregistered<InteractionInputHandler>());
        }
    }
}
