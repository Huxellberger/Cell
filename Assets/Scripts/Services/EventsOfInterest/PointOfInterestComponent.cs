// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Character;
using UnityEngine;

namespace Assets.Scripts.Services.EventsOfInterest
{
    public class PointOfInterestComponent 
        : MonoBehaviour
    {
        [EventOfInterestKey]
        public string PointOfInterestEventKey;

        private readonly LazyServiceProvider<IEventsOfInterestServiceInterface> _eventOfInterestService 
            = new LazyServiceProvider<IEventsOfInterestServiceInterface>();

        void OnTriggerEnter2D(Collider2D inCollider)
        {
            if (inCollider != null && inCollider.gameObject != null)
            {
                OnGameObjectCollides(inCollider.gameObject);
            }
        }

        protected void OnGameObjectCollides(GameObject inCollidingObject)
        {
            // Current check for player character until we have a better system.
            if (inCollidingObject != null && inCollidingObject.GetComponent<CharacterComponent>() != null)
            {
                _eventOfInterestService.Get().RecordEventOfInterest(PointOfInterestEventKey);
            }
        }
    }
}
