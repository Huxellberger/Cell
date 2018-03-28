// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.AI.Pathfinding;
using Assets.Scripts.AI.Vision;
using Assets.Scripts.Components.Emote;
using Assets.Scripts.Core;
using Assets.Scripts.Messaging;
using UnityEngine;

namespace Assets.Scripts.AI.Goals.CustomGoals
{
    public class PursuitTargetGoal 
        : CompositeGoal
    {
        private readonly PursuitTargetGoalParams _params;
        private readonly IPathfindingInterface _pathfinding;
        private readonly IEmoteInterface _emote;

        private UnityMessageEventHandle<SuspiciousObjectDetectedMessage> _suspiciousObjectDetectedHandle;

        private Vector3 _initialLocation;
        private GameObject _detectedObject;
        private bool _justDetectedObject = false;

        private bool _inProgress = false;

        public PursuitTargetGoal(GameObject inOwner, PursuitTargetGoalParams inParams)
            : base(inOwner)
        {
            _params = inParams;
            _pathfinding = inOwner.GetComponent<IPathfindingInterface>();
            _emote = inOwner.GetComponent<IEmoteInterface>();
        }

        public override void RegisterGoal()
        {
            _suspiciousObjectDetectedHandle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<SuspiciousObjectDetectedMessage>(Owner, OnSuspiciousObjectDetected);
        }

        public override void UnregisterGoal()
        {
            UnityMessageEventFunctions.UnregisterActionWithDispatcher(Owner, _suspiciousObjectDetectedHandle);
        }

        protected override void OnInitialised()
        {
            if (_detectedObject != null)
            {
                Owner.transform.up = (_detectedObject.transform.position - Owner.gameObject.transform.position).normalized;
            }

            _initialLocation = Owner.transform.position;
            _inProgress = true;
            _emote.SetEmoteState(EEmoteState.Alerted);
            _pathfinding.SetFollowTarget(_detectedObject);

            AddSubGoal(new AttackTargetGoal(Owner, _detectedObject));
        }

        protected override void OnTerminated()
        {
            _pathfinding.SetFollowTarget(null);
            _emote.SetEmoteState(EEmoteState.None);
            _inProgress = false;

            _pathfinding.SetTargetLocation(_initialLocation, () => {});
        }

        public override float CalculateDesirability()
        {
            var desirability = 0.0f;

            if (_justDetectedObject || _inProgress)
            {
                if (VectorFunctions.DistanceSquared(Owner.transform.position, _detectedObject.transform.position) <
                    _params.AbandonPursuitRadiusSquared)
                {
                    desirability = _params.TargetDetectedDesirability;
                }
            }

            _justDetectedObject = false;

            return desirability;
        }

        private void OnSuspiciousObjectDetected(SuspiciousObjectDetectedMessage inMessage)
        {
            if (inMessage.SuspiciousGameObject != null)
            {
                _detectedObject = inMessage.SuspiciousGameObject;
                _justDetectedObject = true;
            }
        }
    }
}
