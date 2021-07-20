using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GoToCredits : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Fim());
    }

    IEnumerator Fim()
    {
        
        yield return new WaitForSeconds(7); 

        SceneManager.LoadScene(3);
    }  
    
}
