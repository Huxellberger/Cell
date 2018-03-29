// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Emote;
using UnityEngine;

namespace Assets.Scripts.AI.Goals.CustomGoals
{
    public abstract class InvestigateDisturbanceGoal 
        : CompositeGoal
    {
        private readonly InvestigateDisturbanceGoalParams _params;

        private Vector3 _initialRotation;
        private Vector3 ? _lastDisturbance;
        
        private bool _justDisturbed = false;
        private bool _inProgress = false;

        protected InvestigateDisturbanceGoal(GameObject inOwner, InvestigateDisturbanceGoalParams inParams) 
            : base(inOwner)
        {
            _params = inParams;
        }

        public override void RegisterGoal()
        {
            RegisterForDisturbance();
        }

        public override void UnregisterGoal()
        {
            UnregisterForDisturbance();
        }

        protected abstract void RegisterForDisturbance();

        protected abstract void UnregisterForDisturbance();

        protected override void OnInitialised()
        {
            Owner.GetComponent<IEmoteInterface>().SetEmoteState(EEmoteState.Suspicious);
            _initialRotation = Owner.transform.eulerAngles;

            if (_lastDisturbance != null)
            {
                Owner.gameObject.transform.up = (_lastDisturbance.Value - Owner.gameObject.transform.position).normalized;
            }

            AddSubGoal(new MoveToTargetGoal(Owner, Owner.transform.position));
            AddSubGoal(new DelayGoal(Owner, _params.IdleDelayOnObservation));
            if (_lastDisturbance != null)
            {
                AddSubGoal(new MoveToTargetGoal(Owner, _lastDisturbance.Value));
            }
            AddSubGoal(new DelayGoal(Owner, _params.IdleDelayOnDetection));

            _inProgress = true;
        }

        protected override void OnTerminated()
        {
            _inProgress = false;

            Owner.transform.eulerAngles = _initialRotation;
            Owner.GetComponent<IEmoteInterface>().SetEmoteState(EEmoteState.None);
        }

        public override float CalculateDesirability()
        {
            var desirability = 0.0f;

            if (_justDisturbed || _inProgress)
            {
                desirability = _params.DesirabilityOnDetection;
            }

            _justDisturbed = false;

            return desirability;
        }

        protected void RecordDisturbance(Vector3 disturbanceLocation)
        {
            _lastDisturbance = disturbanceLocation;
            _justDisturbed = true;
        }
    }
}
