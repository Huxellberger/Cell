// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.Messaging;
using UnityEngine;

namespace Assets.Scripts.Components.Power
{
    public class PowerAssignmentComponent 
        : MonoBehaviour 
        , IPowerAssignmentInterface
    {
        public class PowerStatus
        {
            public readonly IPowerInterface CurrentPower;
            public bool Activatable;

            public PowerStatus(IPowerInterface inPower, bool inActivatable)
            {
                CurrentPower = inPower;
                Activatable = inActivatable;
            }
        }

        private readonly Dictionary<EPowerSetting, PowerStatus> _registeredPowers = new Dictionary<EPowerSetting, PowerStatus>();

        protected void Update()
        {
            foreach (var registeredPowerPairing in _registeredPowers)
            {
                var newActivateResult = registeredPowerPairing.Value.CurrentPower.CanActivatePower(gameObject);

                // If state has changed or inactive send a new message
                if (newActivateResult != registeredPowerPairing.Value.Activatable || !registeredPowerPairing.Value.Activatable)
                {
                    registeredPowerPairing.Value.Activatable = newActivateResult;

                    UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(gameObject, new PowerUpdateMessage
                    (
                        registeredPowerPairing.Key, 
                        registeredPowerPairing.Value.Activatable, 
                        registeredPowerPairing.Value.CurrentPower.GetPowerCooldownPercentage()
                    ));
                }
            }
        }

        // IPowerAssignmentInterface
        public void SetPower(IPowerInterface inPower, EPowerSetting inPowerSetting)
        {
            var newStatus = new PowerStatus(inPower, inPower.CanActivatePower(gameObject));

            if (_registeredPowers.ContainsKey(inPowerSetting))
            {
                _registeredPowers[inPowerSetting].CurrentPower.OnPowerCleared();
                _registeredPowers[inPowerSetting] = newStatus;
            }
            else
            {
                _registeredPowers.Add(inPowerSetting, newStatus);
            }

            inPower.OnPowerSet(gameObject);

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(gameObject, new PowerSetMessage(inPower, inPowerSetting));
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(gameObject, new PowerUpdateMessage(inPowerSetting, newStatus.Activatable, inPower.GetPowerCooldownPercentage()));
        }
        
        public void ActivatePower(EPowerSetting inPowerSetting)
        {
            var activatedPower = false;

            if (_registeredPowers.ContainsKey(inPowerSetting) &&
                _registeredPowers[inPowerSetting].CurrentPower.CanActivatePower(gameObject))
            {
                _registeredPowers[inPowerSetting].CurrentPower.ActivatePower(gameObject);
                activatedPower = true;
            }

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(gameObject, new PowerActivationAttemptMessage(inPowerSetting, activatedPower));
        }
        // ~IPowerAssignmentInterface
    }
}
