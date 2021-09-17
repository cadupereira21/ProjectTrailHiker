using System;
using UnityEngine;

namespace Game.Scripts.Player
{
    public class Player : MonoBehaviour
    {
        protected Rigidbody2D PlayerRb { private set; get; }
        protected PlayerStateManager StateManager { private set; get; }
        protected PlayerColliderManager ColliderManager { private set; get; }
        protected PlayerInputManager InputManager { private set; get; }

        protected virtual void Awake()
        {
            PlayerRb = GetComponent<Rigidbody2D>();
            StateManager = GetComponent<PlayerStateManager>();
            ColliderManager = FindObjectOfType<PlayerColliderManager>();
            InputManager = GetComponent<PlayerInputManager>();
        }
    }
}