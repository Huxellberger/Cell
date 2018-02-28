// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Test.Components.Health;
using Assets.Scripts.Test.Components.Health.Damage;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Health.Damage
{
    [TestFixture]
    public class ProximityHealthAdjustmentComponentTestFixture
    {
        [Test]
        public void OnCollision_AdjustsHealthBySpecifiedAmount()
        {
            var healthComponent =
                new GameObject().AddComponent<MockHealthComponent>();

            var adjustComponent = new GameObject().AddComponent<TestProximityHealthAdjustmentComponent>();

            adjustComponent.HealthChangeOnContact = 12;
            adjustComponent.TestCollide(healthComponent.gameObject);

            Assert.AreEqual(adjustComponent.HealthChangeOnContact, healthComponent.AdjustHealthResult);
        }
    }
}

#endif
