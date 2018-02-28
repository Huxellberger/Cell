// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.Components.ActionStateMachine.States.CinematicCamera;
using Assets.Scripts.Components.ActionStateMachine.States.Dead;
using Assets.Scripts.Components.ActionStateMachine.States.Locomotion;
using Assets.Scripts.Components.ActionStateMachine.States.MainMenu;
using Assets.Scripts.Components.ActionStateMachine.States.OpenMenuUI;
using Assets.Scripts.Components.ActionStateMachine.States.PushObjectActionState;
using Assets.Scripts.Components.ActionStateMachine.States.Spawning;
using Assets.Scripts.Components.ActionStateMachine.States.Transforming;

namespace Assets.Scripts.Components.ActionStateMachine.Builder
{
    public delegate ActionState ActionStateCreatorDelegate(ActionStateInfo inInfo);

    public class ActionStateDefinitions
    {
        public IDictionary<EActionStateId, ActionStateCreatorDelegate> Definitions { get; private set; }

        public ActionStateDefinitions(ActionStateParams inParams)
        {
            Definitions = new Dictionary<EActionStateId, ActionStateCreatorDelegate>();
            InitialiseDefinitions(inParams);
        }

        private void InitialiseDefinitions(ActionStateParams inParams)
        {
            Definitions.Add(EActionStateId.Locomotion, info => new LocomotionActionState(info));
            Definitions.Add(EActionStateId.Dead, info => new DeadActionState(info));
            Definitions.Add(EActionStateId.Null, info => new NullActionState());
            Definitions.Add(EActionStateId.OpenMenuUI, info => new OpenMenuUIActionState(info));
            Definitions.Add(EActionStateId.MainMenu, info => new MainMenuActionState(info));
            Definitions.Add(EActionStateId.Transforming, info => new TransformingActionState((TransformingActionStateInfo)info));
            Definitions.Add(EActionStateId.Spawning, info => new SpawningActionState(info, inParams.SpawningParams));
            Definitions.Add(EActionStateId.PushObject, info => new PushObjectActionState((PushObjectActionStateInfo) info));
            Definitions.Add(EActionStateId.CinematicCamera, info => new CinematicCameraActionState((CinematicCameraActionStateInfo)info));
        }
    }
}
