using Game.Scripts.Audio;
using UnityEngine;

namespace Game.Scripts.Player
{
    public class PlayerAudio : MonoBehaviour
    {
        private AudioManager audioManager;
        private Rigidbody2D player;
        private PlayerMovementController playerMovement;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float footstepsVolume;
        
        // Start is called before the first frame update
        void Start()
        {
            audioManager = FindObjectOfType<AudioManager>();
            player = GetComponent<Rigidbody2D>();
            playerMovement = GetComponent<PlayerMovementController>();
            
            audioManager.Play(GameSounds.Pegada2);
            audioManager.SetVolume(GameSounds.Pegada2, 0.0f);
        }

        // Update is called once per frame
        void Update()
        {
            if (audioManager == null)
                audioManager = FindObjectOfType<AudioManager>();

            switch (playerMovement.inputAverageTime)
            {
                case 0.5f:
                    audioManager.SetPitch(GameSounds.Pegada2,0.8f);
                    break;
                case 0.35f:
                    audioManager.SetPitch(GameSounds.Pegada2,0.90f);
                    break;
                case 0.22f:
                    audioManager.SetPitch(GameSounds.Pegada2,0.95f);
                    break;
                case 0.12f:
                    audioManager.SetPitch(GameSounds.Pegada2,1.0f);
                    break;
                case 0.03f:
                    audioManager.SetPitch(GameSounds.Pegada2,1.1f);
                    break;
                default: 
                    audioManager.SetPitch(GameSounds.Pegada2,1.2f);
                    break;
            }

            if ((player.velocity.x > 0.1f || player.velocity.x < -0.1f) && !playerMovement.isFalling)
            {
                audioManager.SetVolume(GameSounds.Pegada2, footstepsVolume);
            }
            else
            {
                audioManager.SetVolume(GameSounds.Pegada2, 0.0f);
            }
        }
    }
}
