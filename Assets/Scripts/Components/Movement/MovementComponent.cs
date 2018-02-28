// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Stamina;
using UnityEngine;

namespace Assets.Scripts.Components.Movement
{
    [RequireComponent(typeof(Rigidbody), typeof(IStaminaInterface))]
    public class MovementComponent 
        : MonoBehaviour
        , IMovementInterface
    {
        public float SprintMultiplier = 1.5f;
        public float ExhaustedMultiplier = 0.5f;
        public float Velocity = 5.0f;
        public float JumpVelocity = 6.0f;
        public int SprintStaminaCostPerUpdate = 1;
        public int JumpStaminaCost = 10;
        
        private Rigidbody ObjectRigidBody { get; set; }
        private IStaminaInterface StaminaInterface { get; set; }

        private float HorizontalModifier { get; set; }
        private float ForwardModifier { get; set; }
        
        private bool SprintEnabled { get; set; }
        private bool JumpRequested { get; set; }
        private bool JumpInProgress { get; set; }
        private float PriorVerticalVelocity { get; set; }

        protected void Awake()
        {
            HorizontalModifier = 0.0f;
            ForwardModifier = 0.0f;

            SprintEnabled = false;
            JumpRequested = false;
            JumpInProgress = false;

            PriorVerticalVelocity = 0.0f;

            ObjectRigidBody = gameObject.GetComponent<Rigidbody>();
            StaminaInterface = gameObject.GetComponent<IStaminaInterface>();
        }

        protected void FixedUpdate ()
        {
            var beganJumping = UpdateJumpProgress();

            // Disable movement alteration when in the air
            if (!JumpInProgress || beganJumping)
            {
                // Normal Vector
                var movementVector = new Vector3(HorizontalModifier, 0.0f, ForwardModifier).normalized;
                movementVector = gameObject.transform.rotation * movementVector;

                var currentVelocity = Velocity;

                // Managing sprint status
                if (CanSprint(beganJumping) && (Mathf.Abs(HorizontalModifier) > 0.01f || Mathf.Abs(ForwardModifier) > 0.01f))
                {
                    currentVelocity *= SprintMultiplier;

                    StaminaInterface.AlterStamina(-SprintStaminaCostPerUpdate);
                }
                else if (StaminaInterface.IsStaminaDepleted())
                {
                    currentVelocity *= ExhaustedMultiplier;
                }

                movementVector *= currentVelocity;
                
                ObjectRigidBody.velocity = new Vector3(movementVector.x, GetNewVerticalVelocity(), movementVector.z);
            }

            PriorVerticalVelocity = ObjectRigidBody.velocity.y;

            HorizontalModifier = 0.0f;
            ForwardModifier = 0.0f;
        }

        private bool UpdateJumpProgress()
        {
            var beganJumping = false;

            if (JumpRequested && CanJump())
            {
                JumpInProgress = true;
                beganJumping = true;
                StaminaInterface.AlterStamina(-JumpStaminaCost);
            }
            else if (JumpInProgress)
            {
                if (Mathf.Abs(ObjectRigidBody.velocity.y - PriorVerticalVelocity) < 0.05f)
                {
                    JumpInProgress = false;
                }
            }

            JumpRequested = false;
            return beganJumping;
        }

        private float GetNewVerticalVelocity()
        {
            return JumpInProgress ? JumpVelocity : ObjectRigidBody.velocity.y;
        }

        private bool CanJump()
        {
            return !JumpInProgress&& StaminaInterface.CanExpendStamina(JumpStaminaCost);
        }

        private bool CanSprint(bool inBeganJumping)
        {
            return SprintEnabled && (!JumpInProgress || inBeganJumping) && StaminaInterface.CanExpendStamina(SprintStaminaCostPerUpdate);
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

        public void RequestJump()
        {
            if (CanJump())
            {
                JumpRequested = true;
            }
        }
    }
}
