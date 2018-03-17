// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using System.Collections.Generic;
using Assets.Scripts.Services.EventsOfInterest;

namespace Assets.Scripts.Test.Services.EventsOfInterest
{
    public class MockEventsOfInterestService 
        : IEventsOfInterestServiceInterface
    {
        public readonly List<EventOfInterestRegistration> ListenedEvents = new List<EventOfInterestRegistration>();
        public readonly List<EventOfInterestRegistration> StopListeningEvents = new List<EventOfInterestRegistration>();
        public string LastRecordedEvent { get; private set; }

        public void ListenForEventOfInterest(EventOfInterestRegistration registration)
        {
            ListenedEvents.Add(registration);
        }

        public void StopListeningForEventOfInterest(EventOfInterestRegistration registration)
        {
            StopListeningEvents.Add(registration);
        }

        public void RecordEventOfInterest(string inEventKey)
        {
            LastRecordedEvent = inEventKey;
        }
    }
}

#endif // UNITY_EDITOR
