// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Components.Health.Damage
{
    public class ProximityHealthAdjustmentComponent
        : MonoBehaviour
    {
        public int HealthChangeOnContact;

        void OnTriggerEnter2D(Collider2D inCollider)
        {
            if (inCollider != null && inCollider.gameObject != null)
            {
                OnGameObjectCollides(inCollider.gameObject);
            }
        }

        protected void OnGameObjectCollides(GameObject inCollidingObject)
        {
            var healthComponentInterface = inCollidingObject.GetComponent<IHealthInterface>();
            if (healthComponentInterface != null)
            {
                healthComponentInterface.AdjustHealth(new HealthAdjustmentUnit(HealthChangeOnContact, gameObject));
            }
        }
    }
}
