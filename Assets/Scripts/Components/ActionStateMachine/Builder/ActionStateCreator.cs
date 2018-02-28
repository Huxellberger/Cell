// Copyright (C) Threetee Gang All Rights Reserved

namespace Assets.Scripts.Components.ActionStateMachine.Builder
{
    public class ActionStateCreator
        : IActionStateCreatorInterface
    {
        private readonly ActionStateDefinitions _definitions;

        public ActionStateCreator(ActionStateDefinitions inDefinitions)
        {
            _definitions = inDefinitions;
        }

        // IActionStateCreatorInterface
        public ActionState CreateActionState(EActionStateId inId, ActionStateInfo inInfo)
        {
            return _definitions.Definitions[inId](inInfo);
        }
        // ~IActionStateCreatorInterface
    }
}
