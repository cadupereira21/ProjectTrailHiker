﻿using UnityEngine;

namespace Game.Scripts.Player
{
    public class PlayerStateManager : MonoBehaviour
    {
        //public PlayerStates State { private set; get; }

        //private PlayerMovementController playerMovement;
        private PlayerColliderManager playerCollider;

        [SerializeField] private bool isWalkingSlow = false;
        [SerializeField] private bool isWalkingMedium = false;
        [SerializeField] private bool isWalkingFast = false;
        [SerializeField] private bool isJumping = false;
        [SerializeField] private bool isFalling = false;
        [SerializeField] private bool isCrouched = false;
        [SerializeField] private bool isMovingUp = false;
        [SerializeField] private bool isMovingDown = false;
        [SerializeField] private bool isGrounded = false;
        [SerializeField] private bool needsToJump = false;
        [SerializeField] private bool canJump = false;
        [SerializeField] private bool isFlipping = false;
        private bool isOnQte = false;

        public bool IsWalkingSlow => isWalkingSlow;
        public bool IsWalkingMedium => isWalkingMedium;
        public bool IsWalkingFast => isWalkingFast;
        public bool IsJumping => isJumping;
        public bool IsFalling => isFalling;
        public bool IsCrouched => isCrouched;
        public bool IsMovingUp => isMovingUp;
        public bool IsMovingDown => isMovingDown;
        public bool IsGrounded => isGrounded;
        public bool NeedsToJump => needsToJump;
        public bool CanJump => canJump;
        public bool IsFlipping => isFlipping;
        public bool IsOnQte => isOnQte;

        // Start is called before the first frame update
        void Awake()
        {
            //State = PlayerStates.Idle;
            //playerMovement = GetComponent<PlayerMovementController>();
            playerCollider = FindObjectOfType<PlayerColliderManager>();
        }

        // Update is called once per frame
        void Update()
        {
            isGrounded = playerCollider.CheckGrounded();
            isJumping = !isGrounded;
            canJump = isGrounded && !isOnQte;
            needsToJump = playerCollider.CheckNeedsToJump();
            isMovingDown = playerCollider.CheckMovingDown();
            isMovingUp = playerCollider.CheckMovingUp();

            if (needsToJump || isMovingDown || isMovingUp)
            {
                isOnQte = true;
            }
            else
            {
                isOnQte = false;
            }
        }

        public void SetState(bool state, bool boolean)
        {
            state = boolean;
        }
    }
}