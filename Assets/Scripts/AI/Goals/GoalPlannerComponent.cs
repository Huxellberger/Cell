// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.Components.ActionStateMachine.States.Dead;
using Assets.Scripts.Core;
using Assets.Scripts.Messaging;
using UnityEngine;

namespace Assets.Scripts.AI.Goals
{
    public class GoalPlannerComponent 
        : MonoBehaviour
    {
        public List<EGoalID> PossibleGoalIds;
        public GoalParams GoalParameters;

        private IGoalBuilderInterface GoalBuilder { get; set; }
        private readonly TieredLock<EIntelligenceDisableReason> _thoughtLock = new TieredLock<EIntelligenceDisableReason>();

        private UnityMessageEventHandle<EnterDeadActionStateMessage> _enterDeadHandle;
        private UnityMessageEventHandle<LeftDeadActionStateMessage> _leftDeadHandle;

        private IList<Goal> PotentialGoals { get; set; }

        private Goal _activeGoal;

        private Goal ActiveGoal
        {
            get { return _activeGoal; }
            set
            {
                if (_activeGoal != null)
                {
                    _activeGoal.Terminate();
                }
                _activeGoal = value;

                if (_activeGoal != null)
                {
                    _activeGoal.Initialise();
                }
            }
        }

        protected void Start ()
        {
		    PotentialGoals = new List<Goal>();

            GoalBuilder = GetGoalBuilderInterface();

            foreach (var possibleGoalId in PossibleGoalIds)
            {
                var newGoal = GoalBuilder.CreateGoalForId(possibleGoalId);
                newGoal.RegisterGoal();
                PotentialGoals.Add(newGoal);
            }

            RegisterForMessages();
        }

        private void RegisterForMessages()
        {
            _enterDeadHandle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<EnterDeadActionStateMessage>(gameObject,
                    OnEnterDeadActionState);

            _leftDeadHandle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<LeftDeadActionStateMessage>(gameObject,
                    OnLeftDeadActionState);
        }

        private void OnEnterDeadActionState(EnterDeadActionStateMessage inMessage)
        {
            AlterLock(true, EIntelligenceDisableReason.Dead);
        }

        private void OnLeftDeadActionState(LeftDeadActionStateMessage inMessage)
        {
            AlterLock(false, EIntelligenceDisableReason.Dead);
        }

        private void AlterLock(bool locking, EIntelligenceDisableReason inReason)
        {
            if (locking)
            {
                _thoughtLock.Lock(EIntelligenceDisableReason.Dead);

                if (ActiveGoal != null)
                {
                    ActiveGoal.Terminate();
                }
            }
            else
            {
                _thoughtLock.Unlock(EIntelligenceDisableReason.Dead);
            }
        }

        protected void OnDestroy()
        {
            UnregisterForMessages();

            foreach (var potentialGoal in PotentialGoals)
            {
                potentialGoal.UnregisterGoal();
            }
        }

        private void UnregisterForMessages()
        {
            UnityMessageEventFunctions.UnregisterActionWithDispatcher(gameObject, _leftDeadHandle);
            UnityMessageEventFunctions.UnregisterActionWithDispatcher(gameObject, _enterDeadHandle);
        }
	
        protected void Update ()
        {
            if (_thoughtLock.IsLocked())
            {
                return;
            }

            if (ActiveGoal == null)
            {
                ActiveGoal = GetMostDesirableGoal();
                return;
            }

            var deltaTime = GetDeltaTime();

            var updateResult = ActiveGoal.Update(deltaTime);

            switch (updateResult)
            {
                case EGoalStatus.Completed:
                case EGoalStatus.Failed:
                    ActiveGoal.Terminate();
                    ActiveGoal = GetMostDesirableGoal();
                    ActiveGoal.Initialise();
                    break;

                case EGoalStatus.InProgress:
                    var mostDesirableGoal = GetMostDesirableGoal();
                    if (mostDesirableGoal != null && mostDesirableGoal != ActiveGoal)
                    {
                        ActiveGoal = mostDesirableGoal;
                    }
                    break;
                default:
                    break;
            }
        }

        private Goal GetMostDesirableGoal()
        {
            Goal currentMostDesirableGoal = null;
            var currentMostDesirableGoalValue = 0.0f;

            foreach (var potentialGoal in PotentialGoals)
            {
                var potentialGoalDesirability = potentialGoal.GetDesirability();

                if (potentialGoalDesirability > currentMostDesirableGoalValue)
                {
                    currentMostDesirableGoal = potentialGoal;
                    currentMostDesirableGoalValue = potentialGoalDesirability;
                }
            }

            return currentMostDesirableGoal;
        }

        protected virtual IGoalBuilderInterface GetGoalBuilderInterface()
        {
            return new GoalBuilder(gameObject, GoalParameters);
        }

        protected virtual float GetDeltaTime()
        {
            return Time.deltaTime;
        }
    }
}
