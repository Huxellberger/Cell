// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Test.Messaging;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Messaging
{
    [TestFixture]
    public class UnityMessageEventDispatcherComponentTestFixture
    {
        [Test]
        public void GetMessageEventDispatcher_ReturnsValidDispatcher()
        {
            var unityMessageEventDispatcherComponent =
                new GameObject().AddComponent<TestUnityMessageEventDispatcherComponent>();

            unityMessageEventDispatcherComponent.TestAwake();

            Assert.NotNull(unityMessageEventDispatcherComponent.GetUnityMessageEventDispatcher());
        }
    }
}

#endif
