// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.AI.Pathfinding.Nav;
using Assets.Scripts.Services.Navigation;
using Assets.Scripts.Services.Noise;
using Assets.Scripts.Services.Spawn;
using Assets.Scripts.Services.Time;
using Assets.Scripts.Services.Wildlife;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class CellGameServiceProvider
        : GameServiceProvider
    {
        public TilemapNavData NavData;
        public List<Color> GizmoColors;

        private int _currentGizmoColorIndex = 0;

        protected override void Awake()
        {
            base.Awake();

            var data = Instantiate<TilemapNavData>(NavData);

            AddService<ISpawnServiceInterface>(new SpawnService());
            AddService<ITimeServiceInterface>(new TimeService());
            AddService<IWildlifeServiceInterface>(new WildlifeService());
            AddService<INoiseServiceInterface>(gameObject.AddComponent<NoiseService>());
            AddService<INavigationServiceInterface>(new NavigationService(data));
        }

        private void OnDrawGizmosSelected()
        {
            if (GizmoColors.Count > 1)
            {
                _currentGizmoColorIndex = 0;

                foreach (var region in NavData.NavigationTable.Regions)
                {
                    Gizmos.color = GizmoColors[_currentGizmoColorIndex];
                    Gizmos.DrawCube(region.RegionBounds.center, new Vector3(region.RegionBounds.size.x, region.RegionBounds.size.y, 5.0f));

                    NextGizmoColor();
                }
            }
        }

        private void NextGizmoColor()
        {
            _currentGizmoColorIndex++;

            if (_currentGizmoColorIndex >= GizmoColors.Count)
            {
                _currentGizmoColorIndex = 0;
            }
        }
    }
}
