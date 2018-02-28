// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.ActionStateMachine.Builder;
using NUnit.Framework;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine.Builder
{
    [TestFixture]
    public class ActionStateCreatorTestFixture
    {
        [Test]
        public void CreateActionState_UsesDefinitionsToCreateState()
        {
            var creator = new ActionStateCreator(new ActionStateDefinitions(new ActionStateParams()));

            Assert.IsNotNull(creator.CreateActionState(EActionStateId.Null, new ActionStateInfo()));
        }
    }
}
