using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Narrador : MonoBehaviour
{
    [SerializeField] GameObject video1;
    [SerializeField] GameObject video2;
    
    TestPassLevel passLevel;

    [SerializeField] private UnityEvent onVideoEnd;
    

    private void Awake ()
    {
        passLevel = GetComponent<TestPassLevel>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Waiter());
    }

    IEnumerator Waiter()
    {
        AudioManager.instance.PlaySound("fireplace");
        AudioManager.instance.PlaySound("Narrador");

       
        yield return new WaitForSeconds(38); //narrador.lenght
        
        video1.SetActive(false);

        video2.SetActive(true);
        AudioManager.instance.PlaySound("narrador1");

        //Wait for 2 seconds
        yield return new WaitForSeconds(17);

        AudioManager.instance.PlaySound("narrador2");

        yield return new WaitForSeconds(15);   

        AudioManager.instance.PlaySound("narrador3"); 

        yield return new WaitForSeconds(22); 

        AudioManager.instance.DestroyAllSounds();

        onVideoEnd?.Invoke();
    }

}
