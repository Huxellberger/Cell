// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Messaging;
using UnityEngine;

namespace Assets.Scripts.Components.Trigger
{
    public abstract class TriggerComponent 
        : MonoBehaviour
    {
        protected abstract bool CanTrigger(GameObject inGameObject);
        protected abstract bool CanCancelTrigger(GameObject inGameObject);

        private void OnTriggerEnter2D(Collider2D inCollider)
        {
            if (inCollider != null && inCollider.gameObject != null)
            {
                OnGameObjectCollides(inCollider.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D inCollider)
        {
            if (inCollider != null && inCollider.gameObject != null)
            {
                OnGameObjectStopsColliding(inCollider.gameObject);
            }
        }

        protected void OnGameObjectCollides(GameObject inCollidingObject)
        {
            if (CanTrigger(inCollidingObject))
            {
                OnTrigger(inCollidingObject);
            }
        }

        protected void OnGameObjectStopsColliding(GameObject inCollidingObject)
        {
            if (CanCancelTrigger(inCollidingObject))
            {
                OnCancelTrigger(inCollidingObject);
            }
        }

        private void OnTrigger(GameObject inTriggerGameObject)
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(gameObject, new TriggerMessage(inTriggerGameObject));
        }

        private void OnCancelTrigger(GameObject inTriggerGameObject)
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(gameObject, new CancelTriggerMessage(inTriggerGameObject));
        }
    }
}
