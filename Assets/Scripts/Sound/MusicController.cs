using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioSource music;

    [SerializeField] private AudioClip[] clipList = {};

    private int currentClip = 0;
    private bool onSitatuon = false;

    public AudioSource Music
    {
        get => music;
        set => music = value;
    }

    private void Awake ()
    {
        if (music.clip == null)
            return;
        
        music.volume = 0.3f * AudioManager.instance.MusicVolume;
        music = GetComponent<AudioSource>();
        AudioManager.instance.onMusicVolumeChange += ChangeMusicVolume;
    }

    private void Start ()
    {
        // If there's no music, then do not process
        if (clipList.Length == 0)
            return;
        
        // Play first song
        music.clip = clipList[0];
        music.Play();

        // Invoke NextShuffle
        Invoke(nameof(NextShuffle), music.clip.length);
    }
    
    public void SwitchToMusic (AudioClip clip)
    {
        music.Stop();
        music.clip = clip;
        music.Play();
        music.loop = true;
        onSitatuon = true;
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
        // Only reset if there's music in the queue
        if (currentClip < clipList.Length)
        {
            music.Stop();
            music.clip = clipList[currentClip];
            music.Play();
            music.loop = false;
            onSitatuon = false;
        }
    }

    public void NextShuffle ()
    {
        if (!onSitatuon)
        {
            // Only shuffles if there's music in the queue
            if (clipList.Length < 1)
                return;
        
            music.Stop();
            currentClip++;
            if (currentClip >= clipList.Length)
                currentClip = 0;
            music.clip = clipList[currentClip];
            music.Play();
        }
        
        
        // Invoke again when the current music is over
        Invoke(nameof(NextShuffle), music.clip.length - music.time);
    }
    
}
