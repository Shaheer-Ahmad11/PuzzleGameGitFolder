using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;



public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {

        public string name;

        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume = .75f;
        [Range(0f, 1f)]
        public float volumeVariance = .1f;

        [Range(.1f, 3f)]
        public float pitch = 1f;
        [Range(0f, 1f)]
        public float pitchVariance = .1f;

        public bool loop = false;

        //	public AudioMixerGroup mixerGroup;

        [HideInInspector]
        public AudioSource source;

    }
    public Sound[] sounds;
    public static SoundManager instance;
    public bool isSound;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.volume = s.volume;
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.playOnAwake = false;
        }
    }
    public void Play(string sound)
    {
        if (isSound)
        {
            Sound s = Array.Find(sounds, item => item.name == sound);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }

            s.source.Play();
        }
    }
    public void Stop(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }
}
