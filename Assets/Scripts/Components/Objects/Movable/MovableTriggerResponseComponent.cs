// Copyright (C) Threetee Gang All Rights Reserved

using System;
using Assets.Scripts.Components.Trigger;
using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.Components.Objects.Movable
{
    public class MovableTriggerResponseComponent 
        : TriggerResponseComponent
    {
        public GameObject FinalPosition;

        public float MoveDuration;

        private Vector3 _startPosition;
        private Vector3 _startRotation;

        private Vector3 _initialLerpPosition;
        private Vector3 _initialLerpRotation;

        private Vector3 _finalPosition;
        private Vector3 _finalRotation;

        private float _currentDuration = 0.0f;
        private bool _moving = false;

        protected override void Start()
        {
            base.Start();

            _startPosition = gameObject.transform.position;
            _startRotation = gameObject.transform.eulerAngles;
        }

        protected override void OnTriggerImpl(TriggerMessage inMessage)
        {
            if (!_moving)
            {
                if (FinalPosition != null)
                {
                    _initialLerpPosition = gameObject.transform.position;
                    _initialLerpRotation = gameObject.transform.eulerAngles;

                    _finalPosition = FinalPosition.transform.position;
                    _finalRotation = FinalPosition.transform.eulerAngles;

                    BeginMovement();
                }
                else
                {
                    Debug.LogError("No final position set for moving trigger response!");
                }
            }
        }

        protected override void OnCancelTriggerImpl(CancelTriggerMessage inMessage)
        {
            _finalPosition = _startPosition;
            _finalRotation = _startRotation;

            _initialLerpPosition = gameObject.transform.position;
            _initialLerpRotation = gameObject.transform.eulerAngles;

            BeginMovement();
        }

        protected void FixedUpdate()
        {
            if (_moving)
            {
                if (Math.Abs(MoveDuration) < 0.01f)
                {
                    _moving = false;

                    gameObject.transform.position = _finalPosition;
                    gameObject.transform.eulerAngles = _finalRotation;

                    return;
                }

                var deltaTime = GetDeltaTime();

                _currentDuration += deltaTime;

                var lerpPoint = Mathf.Clamp(_currentDuration / MoveDuration, 0.0f, 1.0f);

                gameObject.transform.position =
                    VectorFunctions.LerpVector(_initialLerpPosition, _finalPosition, lerpPoint);

                gameObject.transform.eulerAngles =
                    VectorFunctions.LerpVector(_initialLerpRotation, _finalRotation, lerpPoint);

                if (_currentDuration > MoveDuration)
                {
                    _moving = false;
                    _currentDuration = 0.0f;
                }
            }
        }

        protected virtual float GetDeltaTime()
        {
            return Time.fixedDeltaTime;
        }

        private void BeginMovement()
        {
            if (_finalPosition != _initialLerpPosition || _finalRotation != _initialLerpRotation)
            {
                if (_moving)
                {
                    _currentDuration = MoveDuration - _currentDuration;
                }
                _moving = true;
            }
        }
    }
}
