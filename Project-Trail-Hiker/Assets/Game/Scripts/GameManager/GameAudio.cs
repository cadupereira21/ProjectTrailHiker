using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Audio;
using Game.Scripts.Player;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Random = System.Random;

namespace Game.Scripts.GameManager
{
    public class GameAudio : MonoBehaviour
    {
        private AudioManager audioManager;
        private GameManager gameManager;
        [SerializeField] private Rigidbody2D playerMovement;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float environmentVolume;

        [Range(0.0f, 1.0f)] [SerializeField] private float musicVolume;

        [Range(0.01f, 5.0f)] [SerializeField] private float environmentFadeTime;
        [Range(0.01f, 5.0f)] [SerializeField] private float musicFadeTime;
        
        private string playerVelocity = "";
        private IEnumerator musicPlayer;
    
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
            
            if(audioManager.IsPlaying(GameSounds.Theme))
                audioManager.Fade(GameSounds.Theme, 0.0f, 2.5f);
            
            audioManager.Play(GameSounds.Ambiente);
            audioManager.Play(GameSounds.CaminhadaLeve);
            audioManager.Play(GameSounds.CaminhadaMedia);
            audioManager.Play(GameSounds.Correndo);

            musicPlayer = MusicPlayer();
            StartCoroutine(musicPlayer);
        }

        // Update is called once per frame
        void Update()
        {

            if (playerMovement.velocity.x > 8.5f)
                playerVelocity = "Correndo";
            else if (playerMovement.velocity.x > 5.5f)
                playerVelocity = "Trotando";
            else if (playerMovement.velocity.x > 0.5f)
                playerVelocity = "Caminhando";
            else
                playerVelocity = "Parado";
                
            
            
            if (audioManager == null)
                audioManager = FindObjectOfType<AudioManager>();

            if (Mathf.Approximately(audioManager.CheckVolume(GameSounds.Theme), 0.0f))
            {
                audioManager.Fade(GameSounds.Ambiente, environmentVolume, environmentFadeTime);
            }

            if (!gameManager.IsGameRunning && !gameManager.IsPaused)
            {
                audioManager.Fade(GameSounds.Ambiente, 0.0f, environmentFadeTime);
            }
            else
            {
                audioManager.Fade(GameSounds.Ambiente, environmentVolume, environmentFadeTime);
            }
        }

        private IEnumerator MusicPlayer()
        {
            yield return new WaitForSeconds(3.0f);
            
            YieldInstruction corroutineTimer;
            var oldVelocity = playerVelocity;
            var timePlaying = 0;
            int timeLimit;

            while (true)
            {
                if (oldVelocity.Equals("Parado"))
                {
                    corroutineTimer = new WaitForSeconds(1.5f);
                    timeLimit = 7;
                }
                else
                {
                    corroutineTimer = new WaitForSeconds(1f);
                    timeLimit = 3;
                }
                
                
                var musicPlaying = WhichMusicIsPlaying();
                if(musicPlaying != null)
                    if (timePlaying++ > timeLimit)
                        if (oldVelocity.Equals(playerVelocity))
                        {
                            if (WillPlayNext(35))
                            {
                                audioManager.Fade(musicPlaying, 0.0f, musicFadeTime);
                            }
                            yield return corroutineTimer;
                        }
                        else
                        {
                            audioManager.Fade(musicPlaying, 0.0f, musicFadeTime);
                            yield return corroutineTimer;
                        }
                    else
                        yield return corroutineTimer;
                else
                {
                    var willPlayNext = WillPlayNext(playerVelocity.Equals("Parado") ? 10 : 35);

                    if (willPlayNext)
                    {
                        oldVelocity = playerVelocity;
                        timePlaying = 0;
                        PlayNext();
                        yield return corroutineTimer;
                    }
                }

                yield return corroutineTimer;
            }
        }

        // private bool WillPlayNext()
        // {
        //     var random = new Random();
        //     var index = random.Next(0, 100);
        //
        //     return index < 35;
        // }

        private void PlayNext()
        {
            switch (playerVelocity)
            {
                case "Parado":
                    var random = new Random();
                    switch (random.Next(1,8))
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            audioManager.Fade(GameSounds.Theme, musicVolume, musicFadeTime);
                            break;
                        case 5:
                            audioManager.Fade(GameSounds.CaminhadaLeve, musicVolume, musicFadeTime);
                            break;
                        case 6:
                            audioManager.Fade(GameSounds.CaminhadaMedia, musicVolume, musicFadeTime);
                            break;
                        case 7:
                            audioManager.Fade(GameSounds.Correndo, musicVolume, musicFadeTime);
                            break;
                    }
                    break;
                case "Caminhando":
                    audioManager.Fade(GameSounds.CaminhadaLeve, musicVolume, musicFadeTime);
                    break;
                case "Trotando":
                    audioManager.Fade(GameSounds.CaminhadaMedia, musicVolume, musicFadeTime);
                    break;
                case "Correndo":
                    audioManager.Fade(GameSounds.Correndo, musicVolume, musicFadeTime);
                    break;
            }
        }
        
        private bool WillPlayNext(int probability)
        {
            var random = new Random();
            var index = random.Next(0, 100);

            return index < probability;
        }

        private string WhichMusicIsPlaying()
        {
            var numberOfPlayingSongs = 0;
            int playingMusicIndex = 0;
            
            var isCaminhadaLevePlaying =
                !Mathf.Approximately(audioManager.CheckVolume(GameSounds.CaminhadaLeve), 0.0f);
            var isCaminhadaMediaPlaying =
                !Mathf.Approximately(audioManager.CheckVolume(GameSounds.CaminhadaMedia), 0.0f);
            var isCorrendoPlaying = !Mathf.Approximately(audioManager.CheckVolume(GameSounds.Correndo), 0.0f);
            var isThemePlaying = !Mathf.Approximately(audioManager.CheckVolume(GameSounds.Theme), 0.0f);

            var list = new[] {isCaminhadaLevePlaying, isCaminhadaMediaPlaying, isCorrendoPlaying, isThemePlaying};

            for (int i = 0; i < list.Length; i++)
            {
                if (list[i])
                {
                    playingMusicIndex = i;
                    ++numberOfPlayingSongs;
                }
            }

            if (numberOfPlayingSongs < 1)
                return null;

            if (numberOfPlayingSongs > 1)
            {
                audioManager.SetVolume(GameSounds.Correndo, 0.0f);
                audioManager.SetVolume(GameSounds.CaminhadaLeve, 0.0f);
                audioManager.SetVolume(GameSounds.CaminhadaMedia, 0.0f);
                audioManager.SetVolume(GameSounds.Theme, 0.0f);
                return null;
            }

            switch (playingMusicIndex)
            {
                case 0:
                    return GameSounds.CaminhadaLeve;
                case 1:
                    return GameSounds.CaminhadaMedia;
                case 2:
                    return GameSounds.Correndo;
                case 3:
                    return GameSounds.Theme;
            }

            return null;
        }
    }
}
