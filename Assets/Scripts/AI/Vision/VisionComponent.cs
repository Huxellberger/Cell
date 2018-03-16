// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.Messaging;
using UnityEngine;

namespace Assets.Scripts.AI.Vision
{
    public abstract class VisionComponent 
        : MonoBehaviour
    {
        public class SuspicionEntry
        {
            public GameObject SuspiciousObject;
            public float TimeElapsed;
            public bool Alerted;

            public SuspicionEntry(GameObject inSuspiciousObject)
            {
                SuspiciousObject = inSuspiciousObject;
                TimeElapsed = 0.0f;
                Alerted = false;
            }
        }

        public GameObject DetectingObject;
        public float TimeUntilDetection = 1.5f;

        private readonly List<GameObject> _nonSuspiciousObjects = new List<GameObject>(10);
        private readonly List<SuspicionEntry> _currentSuspicions = new List<SuspicionEntry>(2);

        protected abstract bool IsSuspicious(GameObject inDetectedObject);

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
            if (IsSuspicious(inCollidingObject))
            {
                OnSighted(inCollidingObject);
                _currentSuspicions.Add(new SuspicionEntry(inCollidingObject));
            }
            else
            {
                _nonSuspiciousObjects.Add(inCollidingObject);
            }
        }

        protected void OnGameObjectStopsColliding(GameObject inCollidingObject)
        {
            var removedCount = _currentSuspicions.RemoveAll((entry) => entry.SuspiciousObject == inCollidingObject);

            if (removedCount == 0)
            {
                _nonSuspiciousObjects.RemoveAll((entry) => entry == inCollidingObject);
            }
        }

        protected void Update()
        {
            var deltaTime = GetDeltaTime();

            if (_currentSuspicions.Count > 0)
            {
                UpdateSuspiciousObjects(deltaTime);
            }

            if (_nonSuspiciousObjects.Count > 0)
            {
                UpdateNonSuspiciousObjects(deltaTime);
            }
        }

        private void UpdateSuspiciousObjects(float inDeltaTime)
        {
            RemoveNonSuspiciousObjects();

            if (_currentSuspicions.Count > 0)
            {
                foreach (var suspicion in _currentSuspicions)
                {
                    suspicion.TimeElapsed += inDeltaTime;

                    if (suspicion.TimeElapsed > TimeUntilDetection && !suspicion.Alerted)
                    {
                        OnDetected(suspicion.SuspiciousObject);
                        suspicion.Alerted = true;
                    }
                }
            }
        }

        private void RemoveNonSuspiciousObjects()
        {
            if (_currentSuspicions.Count > 0)
            {
                var noLongerSuspicious = _currentSuspicions.FindAll((entry) => !IsSuspicious(entry.SuspiciousObject));

                if (noLongerSuspicious.Count > 0)
                {
                    foreach (var removableEntry in noLongerSuspicious)
                    {
                        _nonSuspiciousObjects.Add(removableEntry.SuspiciousObject);
                        _currentSuspicions.Remove(removableEntry);
                    }
                }
            }
        }

        private void UpdateNonSuspiciousObjects(float inDeltaTime)
        {
            var newlySuspicious = _nonSuspiciousObjects.FindAll(IsSuspicious);

            if (newlySuspicious.Count > 0)
            {
                foreach (var currentObject in newlySuspicious)
                {
                    OnSighted(currentObject);
                    _currentSuspicions.Add(new SuspicionEntry(currentObject));
                    _nonSuspiciousObjects.Remove(currentObject);
                }
            }
        }

        protected virtual float GetDeltaTime()
        {
            return Time.deltaTime;
        }

        private void OnSighted(GameObject inSightedGameObject)
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(DetectingObject, new SuspiciousObjectSightedMessage(inSightedGameObject));
        }

        private void OnDetected(GameObject inDetectedGameObject)
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(DetectingObject, new SuspiciousObjectDetectedMessage(inDetectedGameObject));
        }
    }
}
