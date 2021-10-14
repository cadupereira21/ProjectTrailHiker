using UnityEngine;

namespace Game.Scripts.Player
{
    public struct AnimationKeys
    {
        public const string IsWalking = "isWalking";
        public const string IsFalling = "isFalling";
        public const string IsJumping = "isJumping";
    }
    public class PlayerAnimationManager : Player
    {
        private Animator animator;

        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        private void Update()
        {
            if ((PlayerRb.velocity.x > 0.1f || PlayerRb.velocity.x < -0.1f))
            {
                animator.SetBool(AnimationKeys.IsWalking, true);
            } else { animator.SetBool(AnimationKeys.IsWalking, false); }

            animator.SetBool(AnimationKeys.IsFalling, StateManager.IsFalling);
            
            if (StateManager.IsJumping)
            {
                animator.Play("jump_anim");
                StateManager.SetState("isJumping", false);
            }
        }
    }
}
