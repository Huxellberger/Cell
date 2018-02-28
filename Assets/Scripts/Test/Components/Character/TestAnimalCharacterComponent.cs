// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Character;

namespace Assets.Scripts.Test.Components.Character
{
    public class TestAnimalCharacterComponent 
        : AnimalCharacterComponent
    {
        public void TestStart()
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
