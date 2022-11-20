using UnityEngine;
using System.Collections.Generic;

public class SpeechLineTriggerVolume : SFXTriggerVolume
{
    // Used for the captions controller
    [SerializeField] [TextArea] private string[] voiceLineCaptions;
    
    // Check if it's triggered through interaction or automatically
    [SerializeField] private bool playAutomatically = true;

    // Triggers the volume when the player enters it
    protected override void OnTriggerEnter (Collider other)
    {
        if (playAutomatically && other.gameObject.CompareTag("Player"))
        {
            PlayVoiceLine();
        }
    }

    // Reproduce the voice line audio attached to the script
    public void PlayVoiceLine ()
    {
        if (audioClip == null)
        {
            Debug.LogWarning("AudioClip for SpeechLineTriggerVolume on " + gameObject.name + " not set!");
            return;
        }

        // Play voice line sounds
        AudioManager.instance.DestroyAllSounds();
        AudioManager.instance.PlaySound(audioName);
        
        // Show subtitles if they're enabled
        if (GameManager.instance.showSubtitles)
            UserInterfaceController.instance.ShowSubtitle(audioClip.length, voiceLineCaptions[0]);
        
        // Disables the trigger colliders
        foreach (Collider collider in _colliderList)
        {
            collider.enabled = false;
        }
    }
}