// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Messaging;
using Assets.Scripts.Services.Noise;
using UnityEngine;

namespace Assets.Scripts.AI.Goals.CustomGoals
{
    public class InvestigateAudioDisturbanceGoal 
        : InvestigateDisturbanceGoal
    {
        private UnityMessageEventHandle<NoiseHeardMessage> _noiseHeardMessageHandle;

        public InvestigateAudioDisturbanceGoal(GameObject inOwner, InvestigateDisturbanceGoalParams inParams) 
            : base(inOwner, inParams)
        {
        }

        protected override void RegisterForDisturbance()
        {
            _noiseHeardMessageHandle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<NoiseHeardMessage>(Owner, OnNoiseHeard);
        }

        protected override void UnregisterForDisturbance()
        {
            UnityMessageEventFunctions.UnregisterActionWithDispatcher(Owner, _noiseHeardMessageHandle);
        }

        private void OnNoiseHeard(NoiseHeardMessage inMessage)
        {
            RecordDisturbance(inMessage.HeardNoise.NoiseLocation);
        }
    }
}
