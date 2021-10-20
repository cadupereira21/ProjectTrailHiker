using System;
using System.Collections;
using System.ComponentModel.Design;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

namespace Game.Scripts.Audio
{
    public struct GameSounds
    {
        public const string Theme = "Theme";
        public const string Ambiente = "Ambiente";
        public const string Pegada = "Pegada";
        public const string CaminhadaLeve = "CaminhadaLeve";
        public const string CaminhadaMedia = "CaminhadaMediaFull";
        public const string Correndo = "Correndo";
    }

    public class AudioManager : MonoBehaviour
    {
        public Sound[] sounds;

        public static AudioManager instance;

        // Start is called before the first frame update
        void Awake()
        {
            if (instance == null)
                instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            foreach(Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
        }
        
        private bool CheckSoundReference(Sound sound)
        {
            if (sound == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return false;
            }

            return true;
        }

        public void Play(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);

            if (!CheckSoundReference(s))
                return;
            
            s.source.Play();
        }

        public void StopSound(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);

            if (!CheckSoundReference(s))
                return;

            s.source.Stop();
        }

        public void SetVolume(string name, float volume)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);

            if (!CheckSoundReference(s))
                return;

            s.source.volume = volume;
        }

        public void SetPitch(string name, float pitch)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);

            if (!CheckSoundReference(s))
                return;

            s.source.pitch = pitch;
        }

        public void Fade(string name, float endVolume, float duration)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            
            if (!CheckSoundReference(s))
                return;

            DOTween.To(()=> s.source.volume, x=> s.source.volume = x, endVolume, duration);
        }

        public float CheckVolume(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);

            if (!CheckSoundReference(s))
                return -1;

            return s.source.volume;
        }

        public bool IsPlaying(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);

            if (!CheckSoundReference(s))
                return false;

            return s.source.isPlaying;
        }

    }
}