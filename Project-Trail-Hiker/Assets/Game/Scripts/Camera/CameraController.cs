using Cinemachine;
using Game.Scripts.Player;
using UnityEngine;

namespace Game.Scripts.Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform cameraTarget;

        [SerializeField] private Rigidbody2D player;
    
        [SerializeField] private float speedInfluence = 0f;
        
        [SerializeField] private Vector3 cameraTargetSlopeOffset = Vector3.zero;

        private CinemachineVirtualCamera virtualCamera;
        private PlayerMovementController playerMovement;
        private PlayerColliderManager colliderManager;

        // Start is cal led before the first frame update
        void Start()
        {
            playerMovement = GetComponentInParent<PlayerMovementController>();
            colliderManager = FindObjectOfType<PlayerColliderManager>();
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        // Update is called once per frame
        void Update()
        {
            if (colliderManager.isMovingDown || colliderManager.isMovingUp)
            {
                cameraTarget.localPosition = cameraTargetSlopeOffset;
            }
            else
            {
                virtualCamera.m_Follow = cameraTarget;
                cameraTarget.localPosition = new Vector2(2 + player.velocity.x * speedInfluence * Time.fixedDeltaTime * playerMovement.PlayerDirection, cameraTarget.localPosition.y);
            }
        }
    }
}