using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTriggerVolume : SFXTriggerVolume
{
    [SerializeField] private MusicController controller;

    [SerializeField] private bool isTriggeredOnce = true;

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
        if (audioClip == null)
        {
            Debug.LogWarning("AudioClip for MusicTriggerVolume on " + gameObject.name + " not set!");
            return;
        }

        controller.SwitchToMusic(audioClip);
        if (isTriggeredOnce)
        {
            Collider[] colliderList = GetComponents<Collider>();
            foreach (Collider col in colliderList)
            {
                col.enabled = false;
            }
        }
    }
}
