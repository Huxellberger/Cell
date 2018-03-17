// Copyright (C) Threetee Gang All Rights Reserved

namespace Assets.Scripts.Services.EventsOfInterest
{
    public delegate void EventOfInterestResponseDelegate(string eventKey);

    public class EventOfInterestRegistration
    {
        public readonly string EventKey;
        public readonly EventOfInterestResponseDelegate Response;
        public readonly EEventOfInterestType EventType;

        public EventOfInterestRegistration(string inEventKey, EventOfInterestResponseDelegate inDelegate,
            EEventOfInterestType inType)
        {
            EventKey = inEventKey;
            Response = inDelegate;
            EventType = inType;
        }
    }

    public interface IEventsOfInterestServiceInterface
    {
        void ListenForEventOfInterest(EventOfInterestRegistration inRegistration);
        void StopListeningForEventOfInterest(EventOfInterestRegistration registration);
        void RecordEventOfInterest(string inEventKey);
    }
}
