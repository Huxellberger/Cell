// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;

namespace Assets.Scripts.Services.EventsOfInterest
{
    public class EventsOfInterestService 
        : IEventsOfInterestServiceInterface 
    {
        private readonly Dictionary<string, List<EventOfInterestRegistration>> _registrations = new Dictionary<string, List<EventOfInterestRegistration>>();
        private readonly List<EventOfInterestRegistration> _registrationsToRemove = new List<EventOfInterestRegistration>(12);

        public void ListenForEventOfInterest(EventOfInterestRegistration registration)
        {
            if (!_registrations.ContainsKey(registration.EventKey))
            {
                _registrations.Add(registration.EventKey, new List<EventOfInterestRegistration>());
            }

            _registrations[registration.EventKey].Add(registration);
        }

        public void StopListeningForEventOfInterest(EventOfInterestRegistration registration)
        {
            if (_registrations.ContainsKey(registration.EventKey))
            {
                _registrations[registration.EventKey].Remove(registration);
            }
        }

        public void RecordEventOfInterest(string inEventKey)
        {
            if (_registrations.ContainsKey(inEventKey))
            {
                foreach (var registration in _registrations[inEventKey])
                {
                    registration.Response(inEventKey);
                    if (registration.EventType == EEventOfInterestType.OneShot)
                    {
                        _registrationsToRemove.Add(registration);
                    }
                }

                foreach (var registration in _registrationsToRemove)
                {
                    _registrations[registration.EventKey].Remove(registration);
                }
            }

            _registrationsToRemove.Clear();
        }
    }
}
