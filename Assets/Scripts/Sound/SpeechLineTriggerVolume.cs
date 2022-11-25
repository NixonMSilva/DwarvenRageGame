using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class SpeechLineTriggerVolume : SFXTriggerVolume
{
    // Used for the captions controller
    [SerializeField] private Subtitle[] voiceLineCaptions;
    
    // Check if it's triggered through interaction or automatically
    [SerializeField] private bool playAutomatically = true;
    
    // Base delay between voiceline splits
    [SerializeField] private float baseDelay = 0.07f;
    
    // Event to call when the subtitles finish playing (and the audio)
    [SerializeField] private UnityEvent unityCallback;

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
        AudioManager.instance.DestroyAllVoicelines();
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
        CallEventOnComplete();
    }
    
    // Performs something on complete
    private void CallEventOnComplete ()
    {
        unityCallback?.Invoke();
    }
}