using System;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioSource music;

    private AudioClip defaultClip;

    public AudioSource Music
    {
        get => music;
        set => music = value;
    }

    private void Awake ()
    {
        if (music.clip == null)
            return;
        
        defaultClip = music.clip;
        music.volume = 0.3f * AudioManager.instance.MusicVolume;
        music = GetComponent<AudioSource>();
        AudioManager.instance.onMusicVolumeChange += ChangeMusicVolume;
    }
    
    public void SwitchToMusic (AudioClip clip)
    {
        music.clip = clip;
    }

    private void OnDestroy ()
    {
        AudioManager.instance.onMusicVolumeChange -= ChangeMusicVolume;
    }

    public void ChangeMusicVolume (float volume)
    {
        music.volume = 0.3f * volume;
    }

    public void ResetMusic ()
    {
        music.Stop();
        music.clip = defaultClip;
        music.Play();
    }
}
