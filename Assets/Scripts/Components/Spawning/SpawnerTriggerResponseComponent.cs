// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Trigger;
using UnityEngine;

namespace Assets.Scripts.Components.Spawning
{
    [RequireComponent(typeof(ISpawnerInterface))]
    public class SpawnerTriggerResponseComponent 
        : TriggerResponseComponent
    {
        private ISpawnerInterface _spawnerInterface;

        protected override void Start()
        {
            base.Start();

            _spawnerInterface = gameObject.GetComponent<ISpawnerInterface>();
        }

        protected override void OnTriggerImpl(TriggerMessage inMessage)
        {
            _spawnerInterface.Spawn();
        }

        protected override void OnCancelTriggerImpl(CancelTriggerMessage inMessage)
        {
        }
    }
}
