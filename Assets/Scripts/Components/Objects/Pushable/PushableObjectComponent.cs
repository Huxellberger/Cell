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
        private Vector2 _pushModifierThisFrame = new Vector2();

        protected void Start()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        }

        protected void FixedUpdate ()
        {
            if (_pushModifierThisFrame != Vector2.zero)
            {
                var deltaTime = GetDeltaTime();

                _rigidbody.velocity = _pushModifierThisFrame * PushModifier * deltaTime;

                _pushModifierThisFrame = Vector2.zero;
            }
            else
            {
                _rigidbody.velocity = Vector2.zero;
            }
        }

        protected virtual float GetDeltaTime()
        {
            return Time.fixedDeltaTime;
        }

        public void Push(Vector2 inVector)
        {
            _pushModifierThisFrame += inVector;
        }
    }
}
