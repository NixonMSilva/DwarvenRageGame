using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;

    public static AudioManager instance;

    private void Awake ()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject, 2f);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound sound in sounds)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.clip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.pitch = sound.pitch;
            sound.audioSource.loop = sound.loop;
        }
    }

    public void PlaySound (string name)
    {
        Sound s = FindSound(name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.audioSource.Play();
    }

    public void StopSound (string name)
    {
        Sound s = FindSound(name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.audioSource.Stop();
    }

    public void PlaySoundAt (GameObject source, string name)
    {
        Sound s = FindSound(name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        AudioSource soundSource = source.AddComponent<AudioSource>();
        ConfigureAudioSource(soundSource, s);
        soundSource.Play();
        Destroy(soundSource, (soundSource.clip.length + 0.1f));
    }

    public void PlaySoundAt (Vector3 source, string name)
    {
        Sound s = FindSound(name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        GameObject soundObject = new GameObject("AudioPoint");
        soundObject.transform.position = source;
        AudioSource soundSource = soundObject.AddComponent<AudioSource>();
        ConfigureAudioSource(soundSource, s);
        soundSource.Play();
        Destroy(soundObject, (soundSource.clip.length + 0.1f));
    }

    public void ConfigureAudioSource (AudioSource source, Sound sound)
    {
        source.clip = sound.clip;
        source.volume = sound.volume;
        source.pitch = sound.pitch;
        source.spatialBlend = 1f;
        source.rolloffMode = AudioRolloffMode.Linear;
        source.maxDistance = 100f;
        source.dopplerLevel = 0f;
    }

    private Sound FindSound (string name)
    {
        return Array.Find(sounds, sound => sound.name == name);
    }
}
