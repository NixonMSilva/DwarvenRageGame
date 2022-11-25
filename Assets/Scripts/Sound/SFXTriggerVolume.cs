using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXTriggerVolume : MonoBehaviour
{
    [SerializeField] protected string audioName;
    [SerializeField] protected AudioClip audioClip;

    protected Collider[] _colliderList;

    protected virtual void Awake()
    {
        _colliderList = GetComponentsInChildren<Collider>();
    }

    // Triggers the volume when the player enters it
    protected virtual void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlaySFX();
        }
    }

    // Reproduce the SFX audio attached to the script
    private void PlaySFX ()
    {
        if (audioClip == null)
        {
            Debug.LogWarning("AudioClip for SFXTriggerVolume on " + gameObject.name + " not set!");
            return;
        }

        AudioManager.instance.PlaySound(audioClip.name);
        foreach (Collider collider in _colliderList)
        {
            collider.enabled = false;
        }
        Destroy(gameObject, audioClip.length);
    }
}
