// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Emote;
using Assets.Scripts.Messaging;
using Assets.Scripts.Services.Noise;
using UnityEngine;

namespace Assets.Scripts.AI.Goals.CustomGoals
{
    public class InvestigateDisturbanceGoal 
        : CompositeGoal
    {
        private readonly InvestigateDisturbanceGoalParams _params;

        private UnityMessageEventHandle<NoiseHeardMessage> _noiseHeardMessageHandle;

        private Vector3 _initialRotation;
        private NoiseData _lastHeardNoise;

        private bool _justHeardNoise = false;
        private bool _inProgress = false;
        

        public InvestigateDisturbanceGoal(GameObject inOwner, InvestigateDisturbanceGoalParams inParams) 
            : base(inOwner)
        {
            _params = inParams;
        }

        public override void RegisterGoal()
        {
            _noiseHeardMessageHandle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<NoiseHeardMessage>(Owner, OnNoiseHeard);
        }

        public override void UnregisterGoal()
        {
            UnityMessageEventFunctions.UnregisterActionWithDispatcher(Owner, _noiseHeardMessageHandle);
        }

        protected override void OnInitialised()
        {
            Owner.GetComponent<IEmoteInterface>().SetEmoteState(EEmoteState.Suspicious);
            _initialRotation = Owner.transform.eulerAngles;

            AddSubGoal(new MoveToTargetGoal(Owner, Owner.transform.position));
            AddSubGoal(new DelayGoal(Owner, _params.IdleDelay));
            AddSubGoal(new MoveToTargetGoal(Owner, _lastHeardNoise.NoiseLocation));

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

            if (_justHeardNoise || _inProgress)
            {
                desirability = _params.DesirabilityOnDetection;
            }

            _justHeardNoise = false;

            return desirability;
        }

        private void OnNoiseHeard(NoiseHeardMessage inMessage)
        {
            _lastHeardNoise = inMessage.HeardNoise;
            _justHeardNoise = true;
        }
    }
}
