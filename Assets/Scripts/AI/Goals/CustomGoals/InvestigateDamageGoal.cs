// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Health;
using Assets.Scripts.Messaging;
using UnityEngine;

namespace Assets.Scripts.AI.Goals.CustomGoals
{
    public class InvestigateDamageGoal 
        : InvestigateDisturbanceGoal
    {
        private UnityMessageEventHandle<HealthChangedMessage> _healthChangedHandle;

        public InvestigateDamageGoal(GameObject inOwner, InvestigateDisturbanceGoalParams inParams) 
            : base(inOwner, inParams)
        {
        }

        protected override void RegisterForDisturbance()
        {
            _healthChangedHandle = UnityMessageEventFunctions.RegisterActionWithDispatcher<HealthChangedMessage>(Owner, OnHealthChanged);
        }

        protected override void UnregisterForDisturbance()
        {
            UnityMessageEventFunctions.UnregisterActionWithDispatcher(Owner, _healthChangedHandle);
        }

        private void OnHealthChanged(HealthChangedMessage inMessage)
        {
            if (inMessage.Author != null)
            {
                if (inMessage.HealthChange < 0)
                {
                    RecordDisturbance(inMessage.Author.transform.position);
                }
            }
        }
    }
}
