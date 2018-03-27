// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine;
using UnityEngine;

namespace Assets.Scripts.Components.Objects.Pushable
{
    public class OrganicPushObjectPointComponent 
        : PushObjectPointComponent
    {
        protected override bool ExtendedPushConditionsValid(GameObject inGameObject)
        {
            var actionStateMachine = PushableObject.GetComponent<IActionStateMachineInterface>();

            if (actionStateMachine != null)
            {
                return actionStateMachine.IsActionStateActiveOnTrack(EActionStateMachineTrack.Locomotion,
                    EActionStateId.Dead);
            }

            return false;
        }
    }
}
