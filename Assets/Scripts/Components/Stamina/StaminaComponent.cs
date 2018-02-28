// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Core;
using Assets.Scripts.Messaging;
using UnityEngine;

namespace Assets.Scripts.Components.Stamina
{
    public class StaminaComponent 
        : MonoBehaviour
        , IStaminaInterface
    {
        public int InitialStamina = 100;
        public float RegenRate = 0.5f;
        public float RegenBlockTime = 5.0f;

        private TieredLock<ELockStaminaReason> _staminaLock;

        private int _maxStamina;
        private int MaxStamina { get { return _maxStamina; } set { _maxStamina = value; OnMaxStaminaSet(); } }

        private float CurrentRegenTimePassed { get; set; }
        private bool RegenBlocked { get; set; }
        private int _currentStamina;

        private int CurrentStamina
        {
            get
            {
                return _currentStamina;
            }
            set
            {
                var clampedValue = Mathf.Clamp(value, 0, MaxStamina);

                if (clampedValue != _currentStamina && CanAdjustStamina())
                {
                   _currentStamina = clampedValue;
                   OnStaminaSet();
                }
            }
        }

        protected void Start ()
        {
            _staminaLock = new TieredLock<ELockStaminaReason>();
            CurrentRegenTimePassed = 0.0f;
            RegenBlocked = false;
            MaxStamina = InitialStamina;
            RefreshStamina();
        }

        protected void Update()
        {
            var deltaTime = GetDeltaTime();

            CurrentRegenTimePassed += deltaTime;

            if (RegenBlocked && CurrentRegenTimePassed > RegenBlockTime)
            {
                CurrentRegenTimePassed = 0.0f;
                RegenBlocked = false;
            }
            else if (!RegenBlocked)
            {
                if (CurrentRegenTimePassed > RegenRate)
                {
                    CurrentStamina++;
                    CurrentRegenTimePassed = 0.0f;
                }
            }
        }

        protected virtual float GetDeltaTime()
        {
            return Time.deltaTime;
        }

        // IStaminaInterface
        public void AlterStamina(int inStaminaModification)
        {
            CurrentStamina = CurrentStamina + inStaminaModification;

            if (!RegenBlocked && inStaminaModification < 0)
            {
                RegenBlocked = true;
            }
        }

        public void RefreshStamina()
        {
            CurrentStamina = MaxStamina;
        }

        public void SetStaminaChangeEnabled(bool isEnabled, ELockStaminaReason inReason)
        {
            if (!isEnabled)
            {
                _staminaLock.Lock(inReason);
            }
            else
            {
                _staminaLock.Unlock(inReason);
            }
        }

        public int GetCurrentStamina()
        {
            return CurrentStamina;
        }

        public bool CanExpendStamina(int inStaminaToExpend)
        {
            if (CurrentStamina - inStaminaToExpend >= 0)
            {
                return true;
            }

            return false;
        }

        public bool IsStaminaDepleted()
        {
            return CurrentStamina == 0;
        }
        // ~IStaminaInterface

        private bool CanAdjustStamina()
        {
            return !_staminaLock.IsLocked();
        }

        private void OnStaminaSet()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(gameObject, new StaminaChangedMessage(CurrentStamina));
        }

        private void OnMaxStaminaSet()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(gameObject, new MaxStaminaChangedMessage(MaxStamina));
        }
    }
}
