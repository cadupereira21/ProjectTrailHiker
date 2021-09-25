using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Player
{
    public class PlayerColliderManager : MonoBehaviour
    {
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private LayerMask whatIsSlopeUp;
        [SerializeField] private LayerMask whatIsSlopeDown;
        [SerializeField] private LayerMask whatIsGameOverTrigger;
        [SerializeField] private LayerMask whatIsObstacleQteTrigger;
        private new BoxCollider2D playerCollider;
        
        public bool isGameOver;

        private void Awake()
        {
            playerCollider = GetComponentInParent<BoxCollider2D>();
        }

        private void Update()
        {
            isGameOver = Physics2D.OverlapCircle(gameObject.transform.position, .25f, whatIsGameOverTrigger);

            this.gameObject.transform.position = playerCollider.bounds.min;
        }

        public bool CheckGrounded()
        {
            return Physics2D.OverlapCircle(gameObject.transform.position, .25f, whatIsGround);
        }

        public bool CheckNeedsToJump()
        {
            return Physics2D.OverlapCircle(gameObject.transform.position, .25f, whatIsObstacleQteTrigger);
        }

        public bool CheckMovingUp()
        {
            return Physics2D.OverlapCircle(gameObject.transform.position, .25f, whatIsSlopeUp);
        }

        public bool CheckMovingDown()
        {
            return Physics2D.OverlapCircle(gameObject.transform.position, .25f, whatIsSlopeDown);
        }
    }
}