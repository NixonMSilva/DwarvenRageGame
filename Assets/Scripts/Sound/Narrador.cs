using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Narrador : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        StartCoroutine(waiter());

        
    }

    IEnumerator waiter()
{

    AudioManager.instance.PlaySound("Narrador");

   
    yield return new WaitForSeconds(38);

   
     AudioManager.instance.PlaySound("narrador1");

    //Wait for 2 seconds
    yield return new WaitForSeconds(17);

    AudioManager.instance.PlaySound("narrador2");

    yield return new WaitForSeconds(15);   

    AudioManager.instance.PlaySound("narrador3"); 
}

    // Update is called once per frame
}
