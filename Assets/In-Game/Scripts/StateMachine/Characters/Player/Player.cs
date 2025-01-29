using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MovementSystem
{
    [RequireComponent(typeof(PlayerInput))]
    public class Player : MonoBehaviour
    {
        [field:Header("References")]
        [field:SerializeField] public PlayerSO Data { get; private set; }


        [field:Header("Collisions")]
        [field: SerializeField] public PlayerCapsuleColliderUtility ColliderUtility { get; private set; }
        [field: SerializeField] public PlayerLayerData LayerData { get; private set; }

        [field:Header("Camera")]
        [field: SerializeField] public PlayerCameraUtility CameraUtility { get; private set; }

        [field:Header("Animations")]
        [field:SerializeField] public PlayerAnimationData AnimationData { get; private set; }

        public Animator Animator { get; private set; }
        public Transform MainCameraTransform{get; private set;}
        public Rigidbody Rigidbody{get; private set;}
        public PlayerInput Input{get; private set;}
        private PlayerMovementStateMachine movementStateMachine;

        private void Awake() {
            Rigidbody=GetComponent<Rigidbody>();
            Input=GetComponent<PlayerInput>();
            Animator = GetComponentInChildren<Animator>();
            
            ColliderUtility.Initialize(gameObject);
            ColliderUtility.CalculateCapsuleColliderDimensions();
            CameraUtility.Initialize();
            AnimationData.Initialize();

            MainCameraTransform=Camera.main.transform;
            movementStateMachine=new PlayerMovementStateMachine(this);
        }

        private void OnValidate()
        {
            ColliderUtility.Initialize(gameObject);
            ColliderUtility.CalculateCapsuleColliderDimensions();
        }

        private void Start() {
            movementStateMachine.ChangeState(movementStateMachine.IdlingState);
        }
        private void OnTriggerEnter(Collider collider)
        {
            movementStateMachine.OnTriggerEnter(collider);
        }
        public void OnTriggerExit(Collider collider)
        {
            movementStateMachine.OnTriggerExit(collider);
        }

        private void Update() {
            movementStateMachine.HandleInput();
            movementStateMachine.Update();
        }

        private void FixedUpdate() {
            movementStateMachine.PhysicsUpdate();
        }

        #region AnimationEvents
        private bool IsInAnimationTransition(int layerIndex = 0)
        {
            return Animator.IsInTransition(layerIndex);
        }

        public void OnMovementStateAnimationEnterEvent()
        {
           if (IsInAnimationTransition())
           {
               return;
           }
            movementStateMachine.OnanimationEnterEvent();
        }
        public void OnMovementStateAnimationExitEvent()
        {
           if(IsInAnimationTransition())
           {
               return;
           }
            movementStateMachine.OnanimationExitEvent();
        }
        public void OnMovementStateAnimationTransitionEvent()
        {
           if(IsInAnimationTransition())
            {
                return;
            }
            movementStateMachine.OnanimationTransitionEvent();
        }
        #endregion
    }
}
