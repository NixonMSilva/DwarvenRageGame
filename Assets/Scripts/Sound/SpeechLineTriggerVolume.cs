using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class SpeechLineTriggerVolume : SFXTriggerVolume
{
    // Used for the captions controller
    //[SerializeField] [TextArea] private string[] voiceLineCaptions;
    [SerializeField] private Subtitle[] voiceLineCaptions;
    
    // Check if it's triggered through interaction or automatically
    [SerializeField] private bool playAutomatically = true;
    
    // Base delay between voiceline splits
    [SerializeField] private float baseDelay = 0.07f;

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
        AudioManager.instance.PlayVoiceover(audioClip.name);
        
        // Show subtitles if they're enabled
        if (GameManager.instance.showSubtitles)
        {
            StartCoroutine(ShowSubtitles());
        }

        // Disables the trigger colliders
        foreach (Collider collider in _colliderList)
        {
            collider.enabled = false;
        }
    }

    // Show the subtitles each part right next after the other
    private IEnumerator ShowSubtitles ()
    {
        foreach (Subtitle sub in voiceLineCaptions)
        {
            UserInterfaceController.instance.ShowSubtitle(sub.duration, sub.caption);
            yield return new WaitForSeconds(sub.duration + baseDelay);
        }
    }
}