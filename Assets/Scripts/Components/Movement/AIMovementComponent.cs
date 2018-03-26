// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Components.Movement
{
    public class AIMovementComponent 
        : MovementComponent
    {
        public float TurningSpeed = 1.0f;

        protected override Vector3 GetMovementVector()
        {
            var movementVector = new Vector3(HorizontalModifier, ForwardModifier, 0.0f).normalized;

            if (movementVector != Vector3.zero && movementVector != gameObject.transform.up)
            {
                var appliedRotation = Mathf.Clamp(Vector3.SignedAngle(movementVector, gameObject.transform.up, gameObject.transform.up), -TurningSpeed, TurningSpeed);
                gameObject.transform.Rotate(new Vector3(0.0f, 0.0f, appliedRotation));
            }
            
            return movementVector;
        }
    }
}
