using Game.Scripts.Audio;
using UnityEngine;

namespace Game.Scripts.Player
{
    public class PlayerAudio : Player
    {
        private GameManager.GameManager gameManager;
        private AudioManager audioManager;
        //private Rigidbody2D player;
        private PlayerMovementController playerMovement;
        //private PlayerStateManager playerState;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float footstepsVolume;
        
        // Start is called before the first frame update
        void Start()
        {
            audioManager = FindObjectOfType<AudioManager>();
            //player = GetComponent<Rigidbody2D>();
            playerMovement = GetComponent<PlayerMovementController>();
            //playerState = GetComponent<PlayerStateManager>();
            gameManager = FindObjectOfType<GameManager.GameManager>();
            
            if(audioManager.IsPlaying(GameSounds.Pegada))
                audioManager.StopSound(GameSounds.Pegada);
            
            audioManager.Play(GameSounds.Pegada);
            audioManager.SetVolume(GameSounds.Pegada, 0.0f);
        }

        // Update is called once per frame
        void Update()
        {
            if (!gameManager.IsGameRunning)
            {
                audioManager.SetVolume(GameSounds.Pegada, 0.0f);
                return;
            }
            
            if (audioManager == null)
                audioManager = FindObjectOfType<AudioManager>();

            switch (playerMovement.inputAverageTime)
            {
                case 0.5f:
                    audioManager.SetPitch(GameSounds.Pegada,0.8f);
                    break;
                case 0.35f:
                    audioManager.SetPitch(GameSounds.Pegada,0.90f);
                    break;
                case 0.22f:
                    audioManager.SetPitch(GameSounds.Pegada,0.95f);
                    break;
                case 0.12f:
                    audioManager.SetPitch(GameSounds.Pegada,1.0f);
                    break;
                case 0.03f:
                    audioManager.SetPitch(GameSounds.Pegada,1.1f);
                    break;
                default: 
                    audioManager.SetPitch(GameSounds.Pegada,1.2f);
                    break;
            }

            if ((PlayerRb.velocity.x > 0.1f || PlayerRb.velocity.x < -0.1f) && !StateManager.IsFalling)
            {
                audioManager.SetVolume(GameSounds.Pegada, footstepsVolume);
            }
            else
            {
                audioManager.SetVolume(GameSounds.Pegada, 0.0f);
            }
        }
    }
}
