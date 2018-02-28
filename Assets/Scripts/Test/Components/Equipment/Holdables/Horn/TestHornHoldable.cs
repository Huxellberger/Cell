// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Equipment.Holdables;
using Assets.Scripts.Components.Equipment.Holdables.Horn;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Equipment.Holdables.Horn
{
    public class TestHornHoldable 
        : HornHoldable
    {
        public AudioClip PlayedSound { get; private set; }

        public void TestStart ()
        {
            Start();	
        }

        public void TestUseHoldableWithAction(EHoldableAction inAction)
        {
            UseHoldableImpl(inAction);
        }

        protected override void PlaySound(AudioClip inSound)
        {
            PlayedSound = inSound;
        }
    }
}

#endif // UNITY_EDITOR
