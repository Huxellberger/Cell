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

            public SuspicionEntry(GameObject inSuspiciousObject)
            {
                SuspiciousObject = inSuspiciousObject;
                TimeElapsed = 0.0f;
            }
        }

        public GameObject DetectingObject;
        public float TimeUntilDetection = 1.5f;

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
        }

        protected void OnGameObjectStopsColliding(GameObject inCollidingObject)
        {
            _currentSuspicions.RemoveAll((entry) => entry.SuspiciousObject == inCollidingObject);
        }

        protected void Update()
        {
            if (_currentSuspicions.Count > 0)
            {
                var detectedObjects = false;

                var deltaTime = GetDeltaTime();

                foreach (var suspicion in _currentSuspicions)
                {
                    suspicion.TimeElapsed += deltaTime;

                    if (suspicion.TimeElapsed > TimeUntilDetection)
                    {
                        OnDetected(suspicion.SuspiciousObject);
                        detectedObjects = true;
                    }
                }

                if (detectedObjects)
                {
                    _currentSuspicions.RemoveAll((entry) => entry.TimeElapsed > TimeUntilDetection);
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
