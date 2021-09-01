using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Player
{
    public struct CollidersTag{
        public const string Ground = "Ground";
    }

    public class PlayerColliderManager : MonoBehaviour
    {
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private LayerMask whatIsSlopeUp;
        [SerializeField] private LayerMask whatIsSlopeDown;
        [SerializeField] private LayerMask whatIsGameOverTrigger;

        public bool isGrounded;
        public bool isMovingDown;
        public bool isMovingUp;
        public bool isGameOver;

        private void Update()
        {
            isGrounded = Physics2D.OverlapCircle(gameObject.transform.position, .25f, whatIsGround);
            isMovingDown = Physics2D.OverlapCircle(gameObject.transform.position, .25f, whatIsSlopeDown);
            isMovingUp = Physics2D.OverlapCircle(gameObject.transform.position, .25f, whatIsSlopeUp);
            isGameOver = Physics2D.OverlapCircle(gameObject.transform.position, .25f, whatIsGameOverTrigger);
        }
    }
}