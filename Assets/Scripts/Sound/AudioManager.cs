using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<Sound> _sounds;

    public static AudioManager instance;

    private AudioSource currentMusic = null;

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

    }

    public void PlayMusic (Sound music)
    {
        if (music == null)
            return;

        // Stop a music if it's playing
        StopMusic();

        currentMusic = gameObject.AddComponent<AudioSource>();
        ConfigureAudioSource(currentMusic, music, false);
    }

    public void PlayMusic (String musicName)
    {
        if (musicName == "")
            return;

        StopMusic();

        Sound musicSound = FindSound(musicName);
        currentMusic = gameObject.AddComponent<AudioSource>();
        ConfigureAudioSource(currentMusic, musicSound, false);
    }

    public void StopMusic ()
    {
        if (currentMusic == null)
            return;

        currentMusic.Stop();
        Destroy(currentMusic);
    }

    public void PlaySound (string name)
    {
        Sound s = FindSound(name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        AudioSource soundSource = gameObject.AddComponent<AudioSource>();
        ConfigureAudioSource(soundSource, s, false);
        soundSource.Play();
        Destroy(soundSource, soundSource.clip.length + 0.1f);
        
    }

    public void PlaySound (Sound sound)
    {
        if (sound == null || !_sounds.Contains(sound))
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        AudioSource soundSource = gameObject.AddComponent<AudioSource>();
        ConfigureAudioSource(soundSource, sound, false);
        soundSource.Play();
        if (!sound.loop)
            Destroy(soundSource, soundSource.clip.length + 0.1f);
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
        ConfigureAudioSource(soundSource, s, true);
        soundSource.Play();
        Destroy(soundSource, (soundSource.clip.length + 0.1f));
    }

    public void PlaySoundAt (GameObject source, Sound sound)
    {
        if (sound == null || !_sounds.Contains(sound))
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        AudioSource soundSource = source.AddComponent<AudioSource>();
        ConfigureAudioSource(soundSource, sound, true);
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
        ConfigureAudioSource(soundSource, s, true);
        soundSource.Play();
        Destroy(soundObject, (soundSource.clip.length + 0.1f));
    }


    public void PlaySoundAt (Vector3 source, Sound sound)
    {
        if (sound == null || !_sounds.Contains(sound))
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        GameObject soundObject = new GameObject("AudioPoint");
        soundObject.transform.position = source;
        AudioSource soundSource = soundObject.AddComponent<AudioSource>();
        ConfigureAudioSource(soundSource, sound, true);
        soundSource.Play();
        Destroy(soundObject, (soundSource.clip.length + 0.1f));
    }

    public void ConfigureAudioSource (AudioSource source, Sound sound, bool isDirectional)
    {
        source.clip = sound.clip;
        source.volume = sound.volume;
        source.pitch = sound.pitch;
        if (isDirectional)
        {
            source.spatialBlend = 1f;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.maxDistance = 100f;
            source.dopplerLevel = 0f;
        }
    }

    private Sound FindSound (string name)
    {
        return _sounds.Find(sound => sound.name == name);
    }

    public void PlaySoundInVolume (AudioSource source, string name)
    {
        source.Stop();
        Sound sound = FindSound(name);
        ConfigureAudioSource(source, sound,true);
        source.Play();
    }
    
    public void PlaySoundInVolume (AudioSource source, Sound sound)
    {
        source.Stop();
        ConfigureAudioSource(source, sound,true);
        source.Play();
    }

    public void PlaySoundInVolumeRandom(AudioSource source, string name)
    {
        Sound pickedName = PickRandomSound(name);
        PlaySoundInVolume(source, pickedName);
    }

    public void PlaySoundRandom (string name)
    {
        PlaySound(PickRandomSound(name));
    }

    public void PlaySoundRandomAt (GameObject source, string name)
    {
        PlaySoundAt(source, PickRandomSound(name));
    }

    public void PlaySoundRandomAt (Vector3 source, string name)
    {
        PlaySoundAt(source, PickRandomSound(name));
    }

    private Sound PickRandomSound (string name)
    {
        List<Sound> allSounds = _sounds.FindAll(sound => sound.name.Contains(name));
        int randomIndex = UnityEngine.Random.Range(0, allSounds.Count - 1);
        return allSounds[randomIndex];
    }

    [ContextMenu("Autofill Sounds")]
    void AutofillSounds ()
    {
        _sounds.Clear();
        AudioClip[] audioFiles = Resources.LoadAll<AudioClip>("Sounds");
        foreach (AudioClip clip in audioFiles)
        {
            Sound sound = new Sound();
            sound.name = clip.name;
            sound.clip = clip;
            sound.volume = 0.5f;
            sound.pitch = 1f;
            _sounds.Add(sound);
        }
    }
}
