// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine.States.Spawning;
using UnityEngine;

namespace Assets.Scripts.Components.ActionStateMachine.Builder
{
    [CreateAssetMenu(fileName = "Params", menuName = "Cell/ActionState/Params", order = 1)]
    public class ActionStateParams 
        : ScriptableObject
    {
        public SpawningActionStateParams SpawningParams;
    }
}
