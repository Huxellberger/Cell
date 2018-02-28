// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Components.ActionStateMachine.States.SurfaceSticking
{
    public class StickySurfaceComponent
        : MonoBehaviour
    {
        private void OnTriggerExit(Collider inCollider)
        {
            if (inCollider != null && inCollider.gameObject != null)
            {
                OnGameObjectStopsColliding(inCollider.gameObject);
            }
        }

        protected void OnGameObjectStopsColliding(GameObject inCollidingObject)
        {
            var actionStateMachine = inCollidingObject.GetComponent<IActionStateMachineInterface>();
            if (actionStateMachine.IsActionStateActiveOnTrack(EActionStateMachineTrack.Locomotion, EActionStateId.SurfaceSticking))
            {
                actionStateMachine.RequestActionState(EActionStateMachineTrack.Locomotion, EActionStateId.Locomotion, new ActionStateInfo(inCollidingObject));
            }
        }
    }
}
