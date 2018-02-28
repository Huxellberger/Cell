// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine.States.FirstPerson;
using Assets.Scripts.Components.ActionStateMachine.States.Spawning;
using Assets.Scripts.Components.ActionStateMachine.States.SurfaceSticking;
using UnityEngine;

namespace Assets.Scripts.Components.ActionStateMachine.Builder
{
    [CreateAssetMenu(fileName = "Params", menuName = "ProjectQ/ActionState/Params", order = 1)]
    public class ActionStateParams 
        : ScriptableObject
    {
        public SpawningActionStateParams SpawningParams;
        public FirstPersonActionStateParams FirstPersonParams;
        public SurfaceStickingActionStateParams SurfaceStickingParams;
    }
}
