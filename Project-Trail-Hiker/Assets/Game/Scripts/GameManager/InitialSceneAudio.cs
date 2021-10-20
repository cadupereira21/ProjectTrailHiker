using System;
using Game.Scripts.Audio;
using UnityEngine;

namespace Game.Scripts.GameManager
{
    public class InitialSceneAudio : MonoBehaviour
    {
        [SerializeField] private AudioManager audioManager;

        [Range(0.01f, 1.0f)]
        [SerializeField] private float themeVolume;

        [Range(0.01f, 10.0f)] [SerializeField] private float fadeDuration;

        private void Start()
        {
            if(audioManager.IsPlaying(GameSounds.Theme))
                audioManager.StopSound(GameSounds.Theme);
            
            audioManager.Play(GameSounds.Theme);
            audioManager.Fade(GameSounds.Theme, themeVolume, fadeDuration);
        }
    }
}