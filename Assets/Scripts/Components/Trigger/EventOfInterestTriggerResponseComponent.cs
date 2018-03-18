// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Services;
using Assets.Scripts.Services.EventsOfInterest;
using Castle.Core.Internal;

namespace Assets.Scripts.Components.Trigger
{
    public class EventOfInterestTriggerResponseComponent 
        : TriggerResponseComponent
    {
        [EventOfInterestKey]
        public string EventOfInterestNameForTrigger;

        [EventOfInterestKey]
        public string EventOfInterestNameForCancelTrigger;

        private readonly LazyServiceProvider<IEventsOfInterestServiceInterface> _eventsOfInterest 
            = new LazyServiceProvider<IEventsOfInterestServiceInterface>();

        protected override void OnTriggerImpl(TriggerMessage inMessage)
        {
            RecordEvent(EventOfInterestNameForTrigger);
        }

        protected override void OnCancelTriggerImpl(CancelTriggerMessage inMessage)
        {
            RecordEvent(EventOfInterestNameForCancelTrigger);
        }

        private void RecordEvent(string eventName)
        {
            if (!eventName.IsNullOrEmpty())
            {
                _eventsOfInterest.Get().RecordEventOfInterest(eventName);
            }
        }
    }
}
