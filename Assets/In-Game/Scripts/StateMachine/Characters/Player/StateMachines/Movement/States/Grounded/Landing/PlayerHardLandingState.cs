using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MovementSystem
{
    public class PlayerHardLandingState : PlayerLandingState
    {
        public PlayerHardLandingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        #region IState Methods
        public override void Enter()
        {
            stateMachine.ReusableData.MovementSpeedModifier = 0f;
            base.Enter();
            StartAnimation(stateMachine.Player.AnimationData.HardLandingParameterHash);
            stateMachine.Player.Input.PlayerActions.Movement.Disable();
            ResetVelocity();
        }
        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine.Player.AnimationData.HardLandingParameterHash);
            stateMachine.Player.Input.PlayerActions.Movement.Enable();
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if (!IsMovingHorizontally())
            {
                return;
            }
            ResetVelocity();
        }
        public override void OnAnimationExitEvent()
        {
            stateMachine.Player.Input.PlayerActions.Movement.Enable();
        }
        public override void OnAnimationTransitionEvent()
        {
            stateMachine.ChangeState(stateMachine.IdlingState);
        }
        #endregion

        #region Reusable Methods
        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();
            stateMachine.Player.Input.PlayerActions.Movement.started += OnMovementStarted;
        }
        protected override void RemoveInputActionCallbacks()
        {
            base.RemoveInputActionCallbacks();
            stateMachine.Player.Input.PlayerActions.Movement.started -= OnMovementStarted;
        } 
        protected override void OnMove()
        {
            if (stateMachine.ReusableData.ShouldWalk)
            {
                return;
            }
            stateMachine.ChangeState(stateMachine.RunningState);
        }
        #endregion

        #region Input Methods
        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {
            
        }
        private void OnMovementStarted(InputAction.CallbackContext context)
        {
            OnMove();
        }
        #endregion
    }



}
