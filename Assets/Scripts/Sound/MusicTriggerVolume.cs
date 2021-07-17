using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTriggerVolume : MonoBehaviour
{
    [SerializeField] private AudioClip clip;

    [SerializeField] private MusicController controller;

    private bool wasActivated = false;

    private void Awake ()
    {
        controller = GameObject.Find("MusicPlayer").GetComponent<MusicController>();
    }

    public void OnTriggerEnter (Collider other)
    {
        if (wasActivated)
            return;
        
        if (clip == null)
        {
            Debug.LogWarning("Music clip not set for Music Trigger Volume!");
            return;
        }

        // Play only when the player enters
        if (other.CompareTag("Player"))
        {
            controller.Music.Stop();
            controller.Music.clip = clip;
            controller.Music.Play();
            
            // Set activated status so it doesn't play again
            wasActivated = true;
        }
    }

}
