using UnityEngine;

public class SpeechLineTriggerVolume : SFXTriggerVolume
{
    // Used for the captions controller
    [SerializeField][TextArea] private string voiceLineCaptions;
    
    // Triggers the volume when the player enters it
    protected override void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
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

        AudioManager.instance.DestroyAllSounds();
        AudioManager.instance.PlaySound(audioName);
        foreach (Collider collider in _colliderList)
        {
            collider.enabled = false;
        }
    }
}