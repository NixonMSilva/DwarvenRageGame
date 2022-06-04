using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTriggerVolume : SFXTriggerVolume
{
    [SerializeField] private MusicController controller;

    private void Awake ()
    {
        controller = GameObject.Find("MusicPlayer").GetComponent<MusicController>();
    }

    // Triggers the volume when the player enters it
    protected override void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayMusic();
        }
    }

    private void PlayMusic ()
    {
        // Switches the music and disabble all volume colliders
        controller.SwitchToMusic(audioClip);
        foreach (Collider collider in _colliderList)
        {
            collider.enabled = false;
        }
    }
}
