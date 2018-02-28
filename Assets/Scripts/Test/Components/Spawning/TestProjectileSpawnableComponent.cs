// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Spawning;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Spawning
{
    public class TestProjectileSpawnableComponent 
        : ProjectileSpawnableComponent
    {
        public AudioClip LastPlayedClip { get; private set; }

        public void TestAwake()
        {
            Awake();
        }

        public void TestCollide(GameObject inGameObject)
        {
            OnGameObjectCollides(inGameObject);
        }

        protected override void PlaySound(AudioClip inClip)
        {
            LastPlayedClip = inClip;
        }
    }
}

#endif // UNITY_EDITOR
