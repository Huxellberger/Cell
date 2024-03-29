﻿// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.AI.Pathfinding.Nav;
using Assets.Scripts.Services.EventsOfInterest;
using Assets.Scripts.Services.Navigation;
using Assets.Scripts.Services.Noise;
using Assets.Scripts.Services.Persistence;
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
            AddService<IEventsOfInterestServiceInterface>(new EventsOfInterestService());
            AddService<IPersistenceServiceInterface>(new PersistenceService());
        }

        private void OnDrawGizmosSelected()
        {
            if (GizmoColors.Count > 1)
            {
                _currentGizmoColorIndex = 0;

                foreach (var region in NavData.NavigationTable.Regions)
                {
                    Gizmos.color = GizmoColors[_currentGizmoColorIndex];
                    Gizmos.DrawLine(new Vector2(region.RegionBounds.xMin, region.RegionBounds.yMin), new Vector2(region.RegionBounds.xMin, region.RegionBounds.yMax));
                    Gizmos.DrawLine(new Vector2(region.RegionBounds.xMin, region.RegionBounds.yMax), new Vector2(region.RegionBounds.xMax, region.RegionBounds.yMax));
                    Gizmos.DrawLine(new Vector2(region.RegionBounds.xMax, region.RegionBounds.yMax), new Vector2(region.RegionBounds.xMax, region.RegionBounds.yMin));
                    Gizmos.DrawLine(new Vector2(region.RegionBounds.xMax, region.RegionBounds.yMin), new Vector2(region.RegionBounds.xMin, region.RegionBounds.yMin));

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
