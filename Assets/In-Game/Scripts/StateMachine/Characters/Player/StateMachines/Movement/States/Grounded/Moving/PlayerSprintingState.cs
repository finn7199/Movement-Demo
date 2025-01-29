using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MovementSystem
{
    public class PlayerSprintingState : PlayerMovingState
    {
        private PlayerSprintData sprintData;
        private float startTime;
        private bool keepSprinting;
        private bool shouldResetSprintState;

        public PlayerSprintingState(PlayerMovementStateMachine playerMovementStateMachine):base(playerMovementStateMachine)
        {
            sprintData = movementData.SprintData;
        }

        #region IState Methods
        public override void Enter()
        {
            stateMachine.ReusableData.MovementSpeedModifier = sprintData.SpeedModifier;
            base.Enter();
            StartAnimation(stateMachine.Player.AnimationData.SprintParameterHash);
            stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.HardForce;
            shouldResetSprintState = true;
            startTime = Time.time;
            if (!stateMachine.ReusableData.ShouldSprint)
            {
                keepSprinting = false;
            }
        }
        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine.Player.AnimationData.SprintParameterHash);
            if (shouldResetSprintState)
            {
                keepSprinting = false;
                stateMachine.ReusableData.ShouldSprint = false;
            }
            
        }
        public override void Update()
        {
            base.Update();
            if (keepSprinting)
            {
                return;
            }
            if (Time.time < startTime + sprintData.SprintToRunTime)
            {
                return;
            }
            StopSprinting();
        }
        #endregion

        #region Main Methods
        private void StopSprinting()
        {
            if (stateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                stateMachine.ChangeState(stateMachine.IdlingState);
                return;
            }
            stateMachine.ChangeState(stateMachine.RunningState);
        }
        #endregion

        #region Reusable Methods
        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();
            stateMachine.Player.Input.PlayerActions.Sprint.performed += OnSprintPerformed;
        }

        protected override void RemoveInputActionCallbacks()
        {
            base.RemoveInputActionCallbacks();
            stateMachine.Player.Input.PlayerActions.Sprint.performed += OnSprintPerformed;
        }

        protected override void OnFall()
        {
            shouldResetSprintState = false;
            base.OnFall();
        }
        #endregion

        #region Input Methods
        private void OnSprintPerformed(InputAction.CallbackContext context)
        {
            keepSprinting = true;
            stateMachine.ReusableData.ShouldSprint = true;
        }
        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {
            shouldResetSprintState = false;
            base.OnJumpStarted(context);
        }
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.HardStoppingState);
            base.OnMovementCanceled(context);
        }
        #endregion
    }
}
