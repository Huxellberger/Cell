// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.Character.Attack;
using UnityEngine;

namespace Assets.Scripts.AI.Goals.CustomGoals
{
    public class AttackTargetGoal 
        : AtomicGoal
    {
        private readonly GameObject _target;
        private readonly IAttackInterface _attack;
        private readonly IActionStateMachineInterface _targetState;

        public AttackTargetGoal(GameObject inOwner, GameObject inTarget) 
            : base(inOwner)
        {
            _target = inTarget;
            if (inOwner != null)
            {
                _attack = inOwner.GetComponent<IAttackInterface>();
            }

            if (inTarget != null)
            {
                _targetState = inTarget.GetComponent<IActionStateMachineInterface>();
            }
        }

        public override void Initialise()
        {
        }

        public override EGoalStatus Update(float inDeltaTime)
        {
            if (_target == null || _attack == null || _targetState == null)
            {
                return EGoalStatus.Failed;
            }

            if (_targetState.IsActionStateActiveOnTrack(EActionStateMachineTrack.Locomotion, EActionStateId.Dead))
            {
                return EGoalStatus.Completed;
            }

            if (_attack.CanAttack(_target))
            {
                _attack.Attack(_target);
            }

            return EGoalStatus.InProgress;
        }

        public override void Terminate()
        {
        }
    }
}
