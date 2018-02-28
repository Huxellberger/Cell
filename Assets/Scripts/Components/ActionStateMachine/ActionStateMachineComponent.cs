// Copyright (C) Threetee Gang All Rights Reserved

using System;
using System.Collections.Generic;
using Assets.Scripts.Components.ActionStateMachine.Builder;
using UnityEngine;

namespace Assets.Scripts.Components.ActionStateMachine
{
    public class ActionStateMachineComponent : MonoBehaviour
      , IActionStateMachineInterface
    {
        public ActionStateParams Params;

        protected Dictionary<EActionStateMachineTrack, ActionState> ActiveActionStates;

        protected IActionStateCreatorInterface Creator;
        
        protected void Awake()
        {
            // Initialise all tracks to null
            ActiveActionStates = new Dictionary<EActionStateMachineTrack, ActionState>();

            Creator = new ActionStateCreator(new ActionStateDefinitions(Params));

            foreach (EActionStateMachineTrack track in Enum.GetValues(typeof(EActionStateMachineTrack)))
            {
                ActiveActionStates.Add(track, new NullActionState());
            }
        }

        protected void Update()
        {
            foreach (EActionStateMachineTrack track in Enum.GetValues(typeof(EActionStateMachineTrack)))
            {
                ActiveActionStates[track].Update(Time.deltaTime);
            }
        }

        protected void OnDestroy()
        {
            foreach (var actionState in ActiveActionStates)
            {
                actionState.Value.End();
            }
        }

        // IActionStateMachineInterface
        public virtual void RequestActionState(EActionStateMachineTrack selectedTrack, EActionStateId inId, ActionStateInfo inInfo)
        {
            var newState = Creator.CreateActionState(inId, inInfo);
            ActiveActionStates[selectedTrack].End();
            ActiveActionStates[selectedTrack] = newState;
            newState.Start();
        }

        public virtual bool IsActionStateActiveOnTrack(EActionStateMachineTrack selectedTrack, EActionStateId expectedId)
        {
            return ActiveActionStates[selectedTrack].ActionStateId == expectedId;
        }
        // ~IActionStateMachineInterface
    }
}
