// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.AI.Vision;
using Assets.Scripts.Messaging;
using UnityEngine;

namespace Assets.Scripts.AI.Goals.CustomGoals
{
    public class InvestigateVisualDisturbanceGoal 
        : InvestigateDisturbanceGoal
    {
        private UnityMessageEventHandle<SuspiciousObjectSightedMessage> _suspiciousObjectSightedHandle;

        public InvestigateVisualDisturbanceGoal(GameObject inOwner, InvestigateDisturbanceGoalParams inParams) 
            : base(inOwner, inParams)
        {
        }

        protected override void RegisterForDisturbance()
        {
            _suspiciousObjectSightedHandle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<SuspiciousObjectSightedMessage>(
                    Owner.gameObject, OnSuspiciousObjectSighted);
        }

        protected override void UnregisterForDisturbance()
        {
            UnityMessageEventFunctions.UnregisterActionWithDispatcher(Owner.gameObject, _suspiciousObjectSightedHandle);
        }

        private void OnSuspiciousObjectSighted(SuspiciousObjectSightedMessage inMessage)
        {
            RecordDisturbance(inMessage.SuspiciousGameObject.transform.position);
        }
    }
}
