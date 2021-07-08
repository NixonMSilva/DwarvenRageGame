using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTriggerVolume : MonoBehaviour
{
    [SerializeField] private string music;

    public void OnTriggerEnter (Collider other)
    {
        if (music == null)
        {
            Debug.LogWarning("Music not set for Music Trigger Volume!");
            return;
        }

        // Play only when the player enters
        if (other.CompareTag("Player"))
        {
            AudioManager.instance?.PlayMusic(music);
        }
    }

}
