// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Core
{
    public static class PauseFunctions
    {
        public static bool IsGameUnpaused()
        {
            return Time.timeScale > 0.0f;
        }
    }
}
