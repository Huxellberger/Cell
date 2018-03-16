// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Spawning;
using UnityEngine;

namespace Assets.Scripts.Components.Equipment.Holdables.Spawner
{
    [RequireComponent(typeof(ISpawnerInterface))]
    public class HoldableSpawnerComponent 
        : HoldableItemComponent
    {
        private ISpawnerInterface _spawner;

        protected void Start()
        {
            _spawner = gameObject.GetComponent<ISpawnerInterface>();
        }

        protected override void UseHoldableImpl(EHoldableAction inAction)
        {
            _spawner.Spawn();
        }

        protected override void OnHeldImpl()
        {
        }

        protected override void OnDroppedImpl()
        {
        }
    }
}
