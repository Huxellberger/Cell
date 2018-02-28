// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.ActionStateMachine.Builder;
using Assets.Scripts.Components.ActionStateMachine.States.CinematicCamera;
using Assets.Scripts.Components.ActionStateMachine.States.Dead;
using Assets.Scripts.Components.ActionStateMachine.States.FirstPerson;
using Assets.Scripts.Components.ActionStateMachine.States.Locomotion;
using Assets.Scripts.Components.ActionStateMachine.States.MainMenu;
using Assets.Scripts.Components.ActionStateMachine.States.OpenMenuUI;
using Assets.Scripts.Components.ActionStateMachine.States.PushObjectActionState;
using Assets.Scripts.Components.ActionStateMachine.States.Spawning;
using Assets.Scripts.Components.ActionStateMachine.States.SurfaceSticking;
using Assets.Scripts.Components.ActionStateMachine.States.Transforming;
using NUnit.Framework;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine.Builder
{
    [TestFixture]
    public class ActionStateDefinitionsTestFixture
    {
        [Test]
        public void Creation_LocomotionStateExists()
        {
            var definition = new ActionStateDefinitions(new ActionStateParams());

            Assert.IsNotNull((LocomotionActionState)definition.Definitions[EActionStateId.Locomotion](new ActionStateInfo()));
        }

        [Test]
        public void Creation_DeadStateExists()
        {
            var definition = new ActionStateDefinitions(new ActionStateParams());

            Assert.IsNotNull((DeadActionState)definition.Definitions[EActionStateId.Dead](new ActionStateInfo()));
        }

        [Test]
        public void Creation_NullStateExists()
        {
            var definition = new ActionStateDefinitions(new ActionStateParams());

            Assert.IsNotNull((NullActionState)definition.Definitions[EActionStateId.Null](new ActionStateInfo()));
        }

        [Test]
        public void Creation_OpenMenuUIStateExists()
        {
            var definition = new ActionStateDefinitions(new ActionStateParams());

            Assert.IsNotNull((OpenMenuUIActionState)definition.Definitions[EActionStateId.OpenMenuUI](new ActionStateInfo()));
        }

        [Test]
        public void Creation_MainMenuStateExists()
        {
            var definition = new ActionStateDefinitions(new ActionStateParams());

            Assert.IsNotNull((MainMenuActionState)definition.Definitions[EActionStateId.MainMenu](new ActionStateInfo()));
        }

        [Test]
        public void Creation_TransformingStateExists()
        {
            var definition = new ActionStateDefinitions(new ActionStateParams());

            Assert.IsNotNull((TransformingActionState)definition.Definitions[EActionStateId.Transforming](new TransformingActionStateInfo()));
        }

        [Test]
        public void Creation_SpawningStateExists()
        {
            var definition = new ActionStateDefinitions(new ActionStateParams());

            Assert.IsNotNull((SpawningActionState)definition.Definitions[EActionStateId.Spawning](new ActionStateInfo()));
        }

        [Test]
        public void Creation_PushObjectStateExists()
        {
            var definition = new ActionStateDefinitions(new ActionStateParams());

            Assert.IsNotNull((PushObjectActionState)definition.Definitions[EActionStateId.PushObject](new PushObjectActionStateInfo()));
        }

        [Test]
        public void Creation_CinematicCameraStateExists()
        {
            var definition = new ActionStateDefinitions(new ActionStateParams());

            Assert.IsNotNull((CinematicCameraActionState)definition.Definitions[EActionStateId.CinematicCamera](new CinematicCameraActionStateInfo()));
        }

        [Test]
        public void Creation_FirstPersonStateExists()
        {
            var definition = new ActionStateDefinitions(new ActionStateParams());

            Assert.IsNotNull((FirstPersonActionState)definition.Definitions[EActionStateId.FirstPerson](new FirstPersonActionStateInfo()));
        }

        [Test]
        public void Creation_SurfaceStickingStateExists()
        {
            var definition = new ActionStateDefinitions(new ActionStateParams());

            Assert.IsNotNull((SurfaceStickingActionState)definition.Definitions[EActionStateId.SurfaceSticking](new SurfaceStickingActionStateInfo()));
        }
    }
}
