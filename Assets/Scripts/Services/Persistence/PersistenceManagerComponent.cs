// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Instance;
using UnityEngine;

namespace Assets.Scripts.Services.Persistence
{
    public class PersistenceManagerComponent 
        : MonoBehaviour 
    {
        private readonly LazyServiceProvider<IPersistenceServiceInterface> _persistenceService 
            = new LazyServiceProvider<IPersistenceServiceInterface>();

        protected void Awake() 
        {
            if (GameInstance.CurrentInstance.NextSceneSaveData != null)
            {
                PersistenceFunctions.LoadData(GameInstance.CurrentInstance.NextSceneSaveData, _persistenceService.Get());
            }
        }
    }
}
