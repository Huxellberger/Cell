// Copyright (C) Threetee Gang All Rights Reserved

using System;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public static class EnumExtensions
    {
        public static Optional<TEnumType>TryParse<TEnumType>(string inputString)
        {
            try
            {
                return new Optional<TEnumType>( (TEnumType)Enum.Parse(typeof(TEnumType), inputString));
            }
            catch (Exception exception)
            {
                Debug.Log(exception.Message);
                return new Optional<TEnumType>();
            }
        }
    }
}
