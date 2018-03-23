// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Core
{
    public class Tween
    {
        public delegate void OnTweenValueUpdatedDelegate(float newValue);

        private readonly OnTweenValueUpdatedDelegate _tweenDelegate;
        private readonly float _startValue;
        private readonly float _endValue;
        private readonly float _timeToComplete;

        private float _currentTimeValue;
        public bool Complete { get; private set; }

        public Tween(float inStartValue, float inEndValue, float inTimeToComplete, OnTweenValueUpdatedDelegate inDelegate)
        {
            _tweenDelegate = inDelegate;
            _startValue = inStartValue;
            _endValue = inEndValue;
            _timeToComplete = inTimeToComplete;

            _currentTimeValue = .0f;
            Complete = false;

            if (_timeToComplete <= 0.0f)
            {
                Complete = true;
            }
        }

        public void UpdateTween(float deltaValue)
        {
            if (!Complete)
            {
                _currentTimeValue += deltaValue;

                var percentageElapsed = _currentTimeValue / _timeToComplete;

                if (_tweenDelegate != null)
                {
                    _tweenDelegate(Mathf.Lerp(_startValue, _endValue, percentageElapsed));
                }

                if (percentageElapsed >= 1.0f)
                {
                    Complete = true;
                }
            }
        }
    }
}
