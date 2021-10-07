using System;
using Game.Scripts.Audio;
using UnityEngine;

namespace Game.Scripts.GameManager
{
    public class GameAudio : MonoBehaviour
    {
        private AudioManager audioManager;
        private GameManager gameManager;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float environmentVolume;
    
        // Start is called before the first frame update
        void Start()
        {
            try
            {
                gameManager = FindObjectOfType<GameManager>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            audioManager = FindObjectOfType<AudioManager>();
            audioManager.Play(GameSounds.Environment1);
            audioManager.SetVolume(GameSounds.Environment1, environmentVolume);
        }

        // Update is called once per frame
        void Update()
        {
            if (audioManager == null)
                audioManager = FindObjectOfType<AudioManager>();

            if (!gameManager.IsGameRunning && !gameManager.IsPaused)
            {
                audioManager.SetVolume(GameSounds.Environment1, 0.0f);
            }
            else
            {
                audioManager.SetVolume(GameSounds.Environment1, environmentVolume);
            }
        }
    }
}
