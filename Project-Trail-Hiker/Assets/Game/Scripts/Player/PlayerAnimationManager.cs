using UnityEngine;

namespace Game.Scripts.Player
{
    public struct AnimationKeys
    {
        public const string IsWalking = "isWalking";
    }
    public class PlayerAnimationManager : MonoBehaviour
    {
        private PlayerColliderManager colliderManager;
        private Rigidbody2D playerRb;
        private Animator animator;
        
        // Start is called before the first frame update
        void Awake()
        {
            playerRb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            colliderManager = FindObjectOfType<PlayerColliderManager>();
        }

        // Update is called once per frame
        void Update()
        {
            if ((playerRb.velocity.x > 0.1f || playerRb.velocity.x < -0.1f ) && colliderManager.isGrounded)
            {
                animator.SetBool(AnimationKeys.IsWalking, true);
            } else { animator.SetBool(AnimationKeys.IsWalking, false); }
        }
    }
}
