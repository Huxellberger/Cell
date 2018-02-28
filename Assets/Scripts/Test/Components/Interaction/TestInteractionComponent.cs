// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Interaction;

namespace Assets.Scripts.Test.Components.Interaction
{
    public class TestInteractionComponent 
        : InteractionComponent
    {
        public void TestStart()
        {
            Start();
        }

        public void TestUpdate()
        {
            Update();
        }
    }
}

#endif // UNITY_EDITOR
