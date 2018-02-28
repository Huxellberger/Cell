﻿// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Messaging;

namespace Assets.Scripts.Components.Health
{
    [System.Serializable]
    public class HealthChangedMessage
        : UnityMessagePayload
    {
        public HealthChangedMessage(int inHealthChange, int inNewHealth)
        {
            HealthChange = inHealthChange;
            NewHealth = inNewHealth;
        }

        public readonly int HealthChange;
        public readonly int NewHealth;
    }

    [System.Serializable]
    public class MaxHealthChangedMessage
        : UnityMessagePayload
    {
        public MaxHealthChangedMessage(int inMaxHealth)
        {
            MaxHealth = inMaxHealth;
        }

        public readonly int MaxHealth;
    }

    [System.Serializable]
    public class HealthChangedUIMessage
        : UnityMessagePayload
    {
        public HealthChangedUIMessage(int inNewHealth)
        {
            NewHealth = inNewHealth;
        }

        public readonly int NewHealth;
    }

    [System.Serializable]
    public class MaxHealthChangedUIMessage
        : UnityMessagePayload
    {
        public MaxHealthChangedUIMessage(int inMaxHealth)
        {
            MaxHealth = inMaxHealth;
        }

        public readonly int MaxHealth;
    }
}
