// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Core;
using Assets.Scripts.Messaging;
using UnityEngine;

namespace Assets.Scripts.Components.Health
{
    [RequireComponent(typeof(IActionStateMachineInterface))]
    public class HealthComponent 
        : MonoBehaviour
        , IHealthInterface
    {
        public int InitialHealth = 100;

        private int _maxHealth;
        private int MaxHealth { get { return _maxHealth; } set { OnMaxHealthSet(value); } }

        private IActionStateMachineInterface _stateMachine;
        private TieredLock<EHealthLockReason> _healthChangeEnabled;
        private int _currentHealth;
        private int CurrentHealth { get { return _currentHealth; } set { OnHealthSet(value);} }

        protected void Start()
        {
            _healthChangeEnabled = new TieredLock<EHealthLockReason>();

            _stateMachine = gameObject.GetComponent<IActionStateMachineInterface>();
            MaxHealth = InitialHealth;
            ReplenishHealth();
        }

        public void AdjustHealth(HealthAdjustmentUnit inAdjustment)
        {
            OnHealthSet(CurrentHealth + inAdjustment.AdjustAmount, inAdjustment.Author);
        }

        public void SetHealthChangedEnabled(bool isEnabled, EHealthLockReason inReason)
        {
            if (isEnabled)
            {
                _healthChangeEnabled.Unlock(inReason);
            }
            else
            {
                _healthChangeEnabled.Lock(inReason);
            }
        }

        public void ReplenishHealth()
        {
            CurrentHealth = MaxHealth;
        }

        public int GetCurrentHealth()
        {
            return CurrentHealth;
        }

        public int GetMaxHealth()
        {
            return MaxHealth;
        }

        private bool CanAdjustHealth()
        {
            if (_healthChangeEnabled != null)
            {
                return !_healthChangeEnabled.IsLocked();
            }

            return false;
        }

        private void OnHealthSet(int newHealth, GameObject healthAdjuster = null)
        {
            if (CanAdjustHealth())
            {
                var healthChange = newHealth - _currentHealth;
                _currentHealth = newHealth;
                _currentHealth = Mathf.Clamp(_currentHealth, 0, GetMaxHealth());

                UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(gameObject, new HealthChangedMessage(healthChange, _currentHealth, healthAdjuster));

                if (_currentHealth <= 0)
                {
                    _stateMachine.RequestActionState(EActionStateMachineTrack.Locomotion, EActionStateId.Dead, new ActionStateInfo(gameObject));
                }
            }
        }

        private void OnMaxHealthSet(int inMax)
        {
            _maxHealth = inMax;
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(gameObject, new MaxHealthChangedMessage(_maxHealth));
        }
    }
}
