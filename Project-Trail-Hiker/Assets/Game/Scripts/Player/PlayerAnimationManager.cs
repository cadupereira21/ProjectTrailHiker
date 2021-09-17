using UnityEngine;

namespace Game.Scripts.Player
{
    public struct AnimationKeys
    {
        public const string IsWalking = "isWalking";
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
            if ((PlayerRb.velocity.x > 0.1f || PlayerRb.velocity.x < -0.1f) && StateManager.IsGrounded)
            {
                animator.SetBool(AnimationKeys.IsWalking, true);
            } else { animator.SetBool(AnimationKeys.IsWalking, false); }
        }
    }
}
