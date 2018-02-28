// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Spawning;

namespace Assets.Scripts.Test.Components.Spawning
{
    public class TestAISpawnableComponent 
        : AISpawnableComponent {

        public void TestStart ()
        {
		    Start();
        }

        public void TestDestroy()
        {
            OnDestroy();
        }
    }
}

#endif // UNITY_EDITOR
