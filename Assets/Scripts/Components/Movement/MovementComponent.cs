// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Stamina;
using UnityEngine;

namespace Assets.Scripts.Components.Movement
{
    [RequireComponent(typeof(Rigidbody2D), typeof(IStaminaInterface))]
    public class MovementComponent 
        : MonoBehaviour
        , IMovementInterface
    {
        public float SprintMultiplier = 1.5f;
        public float ExhaustedMultiplier = 0.5f;
        public float Velocity = 5.0f;
        public int SprintStaminaCostPerUpdate = 1;
        
        private Rigidbody2D ObjectRigidBody { get; set; }
        private IStaminaInterface StaminaInterface { get; set; }

        protected float HorizontalModifier { get; set; }
        protected float ForwardModifier { get; set; }
        
        private bool SprintEnabled { get; set; }
        private float PriorVerticalVelocity { get; set; }

        protected void Awake()
        {
            HorizontalModifier = 0.0f;
            ForwardModifier = 0.0f;

            SprintEnabled = false;

            PriorVerticalVelocity = 0.0f;

            ObjectRigidBody = gameObject.GetComponent<Rigidbody2D>();
            StaminaInterface = gameObject.GetComponent<IStaminaInterface>();
        }

        protected void FixedUpdate ()
        {
            // Normal Vector
            var movementVector = GetMovementVector();

            var currentVelocity = Velocity;

            // Managing sprint status
            if (CanSprint() && (Mathf.Abs(HorizontalModifier) > 0.01f || Mathf.Abs(ForwardModifier) > 0.01f))
            {
                currentVelocity *= SprintMultiplier;

                StaminaInterface.AlterStamina(-SprintStaminaCostPerUpdate);
            }
            else if (StaminaInterface.IsStaminaDepleted())
            {
                currentVelocity *= ExhaustedMultiplier;
            }

            movementVector *= currentVelocity;

            ObjectRigidBody.velocity = new Vector3(movementVector.x, movementVector.y, 0.0f);

            HorizontalModifier = 0.0f;
            ForwardModifier = 0.0f;
        }

        protected virtual Vector3 GetMovementVector()
        {
            var movementVector = new Vector3(HorizontalModifier, ForwardModifier, 0.0f).normalized;
            movementVector = gameObject.transform.rotation * movementVector;

            return movementVector;
        }

        private bool CanSprint()
        {
            return SprintEnabled && StaminaInterface.CanExpendStamina(SprintStaminaCostPerUpdate);
        }

        protected virtual float GetDeltaTime()
        {
            return Time.fixedDeltaTime;
        }

        public void ApplyForwardMotion(float inForwardMagnitude)
        {
            ForwardModifier = Mathf.Clamp(inForwardMagnitude, -1.0f, 1.0f);
        }

        public void ApplySidewaysMotion(float inTurningMotion)
        {
            HorizontalModifier = Mathf.Clamp(inTurningMotion, -1.0f, 1.0f);
        }

        public void SetSprintEnabled(bool isEnabled)
        {
            SprintEnabled = isEnabled;
        }
    }
}
