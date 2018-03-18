// Copyright (C) Threetee Gang All Rights Reserved

using System;

namespace Assets.Scripts.Services.EventsOfInterest
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class EventOfInterestKeyAttribute 
        : Attribute
    {
    }
}
