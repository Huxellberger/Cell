// Copyright (C) Threetee Gang All Rights Reserved

namespace Assets.Scripts.Services
{
    public class LazyServiceProvider<TServiceType>
    {
        private TServiceType _cachedService;

        public TServiceType Get()
        {
            if (_cachedService == null)
            {
                if (GameServiceProvider.CurrentInstance != null)
                {
                    _cachedService = GameServiceProvider.CurrentInstance.GetService<TServiceType>();
                }
            }

            return _cachedService;
        }
    }
}
