// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Services.Noise;
using Assets.Scripts.Services.Spawn;
using Assets.Scripts.Services.Time;
using Assets.Scripts.Services.Wildlife;

namespace Assets.Scripts.Services
{
    public class CellGameServiceProvider
        : GameServiceProvider
    {
        protected override void Awake()
        {
            base.Awake();

            AddService<ISpawnServiceInterface>(new SpawnService());
            AddService<ITimeServiceInterface>(new TimeService());
            AddService<IWildlifeServiceInterface>(new WildlifeService());
            AddService<INoiseServiceInterface>(gameObject.AddComponent<NoiseService>());
        }
    }
}
