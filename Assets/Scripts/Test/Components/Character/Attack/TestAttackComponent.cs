// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Character.Attack;

namespace Assets.Scripts.Test.Components.Character.Attack
{
    public class TestAttackComponent 
        : AttackComponent
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
