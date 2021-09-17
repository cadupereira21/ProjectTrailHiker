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
        private PlayerStateManager playerState;
        
        public GameObject productionCamera;

        // Start is cal led before the first frame update
        void Start()
        {
            playerMovement = GetComponentInParent<PlayerMovementController>();
            colliderManager = FindObjectOfType<PlayerColliderManager>();
            playerState = FindObjectOfType<PlayerStateManager>();
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
            productionCamera.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (playerState.IsMovingDown || playerState.IsMovingUp)
            {
                cameraTarget.localPosition = cameraTargetSlopeOffset;
            }
            else
            {
                virtualCamera.m_Follow = cameraTarget;
                cameraTarget.localPosition = new Vector2(2 + player.velocity.x * speedInfluence * Time.fixedDeltaTime * playerMovement.PlayerDirection, cameraTarget.localPosition.y);
            }

            if (Input.GetKeyDown(KeyCode.F12))
            {
                switch (productionCamera.activeInHierarchy)
                {
                    case true:
                        productionCamera.SetActive(false);
                        break;
                    case false:
                        productionCamera.SetActive(true);
                        break;
                }
            }
        }
    }
}