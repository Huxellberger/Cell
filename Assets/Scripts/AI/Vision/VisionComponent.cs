// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.Components.ActionStateMachine.States.Dead;
using Assets.Scripts.Core;
using Assets.Scripts.Messaging;
using UnityEngine;

namespace Assets.Scripts.AI.Vision
{
    [RequireComponent(typeof(PolygonCollider2D))]
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

        public class VisionPointPosition
        {
            public readonly Vector2 InitialPointOffset;
            public readonly float Distance;
            public Vector2 CurrentPointOffset;

            public VisionPointPosition(Vector2 inInitialPoint)
            {
                InitialPointOffset = inInitialPoint;
                CurrentPointOffset = inInitialPoint;
                Distance = InitialPointOffset.magnitude;
            }
        }

        public GameObject DetectingObject;
        public LayerMask BlockingMask;
        public float TimeUntilDetection = 1.5f;
        public Color DebugDrawColour = Color.red;

        private readonly List<GameObject> _nonSuspiciousObjects = new List<GameObject>(10);
        private readonly List<SuspicionEntry> _currentSuspicions = new List<SuspicionEntry>(2);

        private readonly TieredLock<EVisionDisableReasons> _visionLock = new TieredLock<EVisionDisableReasons>();
        private UnityMessageEventHandle<EnterDeadActionStateMessage> _enterDeadHandle;
        private UnityMessageEventHandle<LeftDeadActionStateMessage> _leftDeadHandle;

        private List<VisionPointPosition> _points;
        private PolygonCollider2D _visionCollider;

        protected void Awake()
        {
            _visionCollider = gameObject.GetComponent<PolygonCollider2D>();

            _points = new List<VisionPointPosition>(_visionCollider.GetTotalPointCount());

            foreach (var point in _visionCollider.points)
            {
                _points.Add(new VisionPointPosition(point));
            }

            UpdateVisionBounds();

            RegisterForMessages();
        }

        private void RegisterForMessages()
        {
            _enterDeadHandle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<EnterDeadActionStateMessage>(DetectingObject,
                    OnEnterDeadActionState);

            _leftDeadHandle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<LeftDeadActionStateMessage>(DetectingObject,
                    OnLeftDeadActionState);
        }

        private void OnEnterDeadActionState(EnterDeadActionStateMessage inMessage)
        {
            AlterLock(true, EVisionDisableReasons.Dead);
        }

        private void OnLeftDeadActionState(LeftDeadActionStateMessage inMessage)
        {
            AlterLock(false, EVisionDisableReasons.Dead);
        }

        private void AlterLock(bool locking, EVisionDisableReasons inReason)
        {
            if (locking)
            {
                _visionLock.Lock(inReason);

                if (_visionLock.IsLocked())
                {
                    gameObject.SetActive(false);
                }
            }
            else
            {
                _visionLock.Unlock(inReason);

                if (!_visionLock.IsLocked())
                {
                    gameObject.SetActive(true);
                }
            }
        }

        protected void OnDestroy()
        {
            UnregisterForMessages();
        }

        private void UnregisterForMessages()
        {
            UnityMessageEventFunctions.UnregisterActionWithDispatcher(DetectingObject, _leftDeadHandle);
            UnityMessageEventFunctions.UnregisterActionWithDispatcher(DetectingObject, _enterDeadHandle);
        }

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

            UpdateVisionBounds();
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

        private void UpdateVisionBounds()
        {
            var originalPoints = _visionCollider.points;
            for (var pointIndex = 0; pointIndex < _points.Count; pointIndex++)
            {
                var point = _points[pointIndex];
                originalPoints[pointIndex] = point.InitialPointOffset;
                Vector3 transformedPosition = gameObject.transform.localToWorldMatrix * point.InitialPointOffset;
                var raycastDirection = (transformedPosition).normalized;
                var result = Physics2D.Raycast(gameObject.transform.position, raycastDirection, point.Distance, BlockingMask);

                if (result.transform != null && result.transform.position != transformedPosition)
                {
                    var newLocalPosition = gameObject.transform.InverseTransformPoint(result.point);
                    point.CurrentPointOffset = newLocalPosition;
                    originalPoints[pointIndex] = newLocalPosition;
                }
            }

            _visionCollider.points = originalPoints;
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

        private void OnDrawGizmos()
        {
            if (_visionCollider == null)
            {
                _visionCollider = gameObject.GetComponent<PolygonCollider2D>();
            }

            if (_visionCollider != null)
            {
                Gizmos.color = DebugDrawColour;

                Vector2 currentPosition = gameObject.transform.position;

                for (var currentNodeIndex = 0; currentNodeIndex < _visionCollider.GetTotalPointCount() - 1; currentNodeIndex++)
                {
                    var firstPoint = _visionCollider.points[currentNodeIndex];
                    var secondPoint = _visionCollider.points[currentNodeIndex + 1];

                    Vector2 adjustedFirstPoint = gameObject.transform.localToWorldMatrix * firstPoint;
                    Vector2 adjustedSecondPoint = gameObject.transform.localToWorldMatrix * secondPoint;
                    Gizmos.DrawLine(currentPosition + adjustedFirstPoint, currentPosition + adjustedSecondPoint);
                }

                var initialPoint = _visionCollider.points[0];
                var finalPoint = _visionCollider.points[_visionCollider.GetTotalPointCount() - 1];

                Vector2 adjustedInitialPoint = gameObject.transform.localToWorldMatrix * initialPoint;
                Vector2 adjustedFinalPoint = gameObject.transform.localToWorldMatrix * finalPoint;

                Gizmos.DrawLine(currentPosition + adjustedInitialPoint, currentPosition + adjustedFinalPoint);
            }
        }
    }
}
