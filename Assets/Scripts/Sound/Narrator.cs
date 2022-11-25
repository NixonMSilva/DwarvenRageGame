using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Narrator : MonoBehaviour
{
    [SerializeField] GameObject video1;
    [SerializeField] GameObject video2;
    [SerializeField] private SpeechLineTriggerVolume initialTrigger;

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(Waiter());
    }

    IEnumerator Waiter ()
    {
        AudioManager.instance.PlaySound("fireplace");
        
        yield return new WaitForSeconds(1f);
        
        initialTrigger.PlayVoiceLine();
        
        yield return new WaitForSeconds(38f); //narrador.lenght
        
        video1.SetActive(false);
        video2.SetActive(true);
    }

}
