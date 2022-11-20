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

    [SerializeField] private float musicVolume = 1f;
    [SerializeField] private float sfxVolume = 1f;
    [SerializeField] private float voiceoverVolume = 1f;

    public event Action<float> onMusicVolumeChange;
    public event Action<float> onSFXVolumeChange;
    public event Action<float> onVoiceoverVolumeChange;

    public float MusicVolume
    {
        get => musicVolume;
        set
        {
            musicVolume = value;
            //Debug.Log("Changing music volume to " + value);
            onMusicVolumeChange?.Invoke(musicVolume);
        }
    }
    
    public float SFXVolume
    {
        get => sfxVolume;
        set
        {
            sfxVolume = value;
            onSFXVolumeChange?.Invoke(sfxVolume);
        }
    }

    public float VoiceoverVolume
    {
        get => voiceoverVolume;
        set
        {
            voiceoverVolume = value;
            onVoiceoverVolumeChange?.Invoke(voiceoverVolume);
        }
    }


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
        ConfigureMusic(currentMusic, music);
    }

    public void PlayMusic (String musicName)
    {
        if (musicName == "")
            return;

        StopMusic();

        Sound musicSound = FindSound(musicName);
        currentMusic = gameObject.AddComponent<AudioSource>();
        ConfigureMusic(currentMusic, musicSound);
    }

    private void ConfigureMusic(AudioSource source, Sound sound)
    {
        source.clip = sound.clip;
        source.volume = sound.volume * MusicVolume;
        source.pitch = sound.pitch;
        source.spatialBlend = 0f;
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
        ConfigureAudioSource(soundSource, s, false, AudioType.SFX);
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
        ConfigureAudioSource(soundSource, sound, false, AudioType.SFX);
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
        ConfigureAudioSource(soundSource, s, true, AudioType.SFX);
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
        ConfigureAudioSource(soundSource, sound, true, AudioType.SFX);
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
        ConfigureAudioSource(soundSource, s, true, AudioType.SFX);
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
        ConfigureAudioSource(soundSource, sound, true, AudioType.SFX);
        soundSource.Play();
        Destroy(soundObject, (soundSource.clip.length + 0.1f));
    }

    public void PlayVoiceover (string voiceoverName)
    {
        Sound s = FindSound(voiceoverName);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + voiceoverName + " not found!");
            return;
        }
        AudioSource soundSource = gameObject.AddComponent<AudioSource>();
        ConfigureAudioSource(soundSource, s, false, AudioType.Voiceover);
        soundSource.Play();
        Destroy(soundSource, soundSource.clip.length + 0.1f);
    }
    
    public void DestroyAllSounds ()
    {
        AudioSource[] allSources = GetComponentsInChildren<AudioSource>();
        foreach (AudioSource audio in allSources)
        {
            audio.Stop();
            Destroy(audio);
        }
    }

    public void ConfigureAudioSource (AudioSource source, Sound sound, bool isDirectional, AudioType type)
    {
        source.clip = sound.clip;
        switch (type)
        {
            case AudioType.Music:
                source.volume = sound.volume * MusicVolume;
                break;
            case AudioType.SFX:
                source.volume = sound.volume * SFXVolume;
                break;
            case AudioType.Voiceover:
                source.volume = sound.volume * VoiceoverVolume;
                break;
            default:
                break;
        }
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
        ConfigureAudioSource(source, sound,true, AudioType.SFX);
        source.Play();
    }
    
    public void PlaySoundInVolume (AudioSource source, Sound sound)
    {
        source.Stop();
        ConfigureAudioSource(source, sound,true, AudioType.SFX);
        source.Play();
    }

    public void PlaySoundInVolumeRandom (AudioSource source, string name)
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
        if (allSounds.Count == 0)
            return null;
        int randomIndex = UnityEngine.Random.Range(0, allSounds.Count);
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
