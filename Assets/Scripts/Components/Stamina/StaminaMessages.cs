// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Messaging;

namespace Assets.Scripts.Components.Stamina
{
    public class StaminaChangedMessage
        : UnityMessagePayload
    {
        public readonly int NewStamina;

        public StaminaChangedMessage(int inNewStamina)
            : base()
        {
            NewStamina = inNewStamina;
        }

    }

    public class MaxStaminaChangedMessage
        : UnityMessagePayload
    {
        public readonly int NewMaxStamina;

        public MaxStaminaChangedMessage(int inNewMaxStamina)
            : base()
        {
            NewMaxStamina = inNewMaxStamina;
        }
    }


    public class StaminaChangedUIMessage
        : UnityMessagePayload
    {
        public readonly int NewStamina;

        public StaminaChangedUIMessage(int inNewStamina)
            : base()
        {
            NewStamina = inNewStamina;
        }
    }

    public class MaxStaminaChangedUIMessage
        : UnityMessagePayload
    {
        public readonly int NewMaxStamina;

        public MaxStaminaChangedUIMessage(int inNewMaxStamina)
            : base()
        {
            NewMaxStamina = inNewMaxStamina;
        }
    }
}
