// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Messaging;
using UnityEngine;

namespace Assets.Scripts.Services.Time
{
    public class PauseListenerComponent 
        : MonoBehaviour
        , IPauseListenerInterface
    {
        private ITimeServiceInterface _timeService;

        protected void Start()
        {
            _timeService = GameServiceProvider.CurrentInstance.GetService<ITimeServiceInterface>();

            _timeService.AddPauseListener(this);
        }
	
        protected void OnDestroy()
        {
		    _timeService.RemovePauseListener(this);

            _timeService = null;
        }

        // IPauseListenerComponent
        public void UpdatePauseStatus(EPauseStatus inStatus)
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(gameObject, new PauseStatusChangedMessage(inStatus));
        }
        // ~IPauseListenerComponent
    }
}
