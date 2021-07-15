using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXTriggerVolume : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;

    private Collider[] _colliderList;

    private void Awake()
    {
        _colliderList = GetComponentsInChildren<Collider>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioManager.instance.PlaySound(audioClip.name);
            foreach (Collider collider in _colliderList)
            {
                collider.enabled = false;
            }
            Destroy(gameObject, audioClip.length);
        }
    }
}
