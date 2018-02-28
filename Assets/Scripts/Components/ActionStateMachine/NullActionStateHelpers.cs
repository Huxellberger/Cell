// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Components.ActionStateMachine
{
    public static class NullActionStateHelpers
    {
        public static void TransitionIntoNullActionState(GameObject inGameObject)
        {
            if (inGameObject != null)
            {
                var actionStateMachineInterface = inGameObject.GetComponent<IActionStateMachineInterface>();
                if (actionStateMachineInterface != null)
                {
                    actionStateMachineInterface.RequestActionState
                    (
                        EActionStateMachineTrack.Locomotion,
                        EActionStateId.Null,
                        new ActionStateInfo(inGameObject)
                    );
                }
            }
        }
    }
}
