// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Components.Objects.Pushable
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PushableObjectComponent 
        : MonoBehaviour
        , IPushableObjectInterface
    {
        public float PushModifier = 1.0f;

        private Rigidbody2D _rigidbody;
        private Vector3 _pushModifierThisFrame = new Vector3();

        protected void Start()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        }

        protected void FixedUpdate ()
        {
            if (_pushModifierThisFrame != Vector3.zero)
            {
                var deltaTime = GetDeltaTime();

                _rigidbody.velocity = _pushModifierThisFrame * PushModifier * deltaTime;

                _pushModifierThisFrame = Vector3.zero;
            }
        }

        protected virtual float GetDeltaTime()
        {
            return Time.fixedDeltaTime;
        }

        public void Push(Vector3 inVector)
        {
            _pushModifierThisFrame += inVector;
        }
    }
}
