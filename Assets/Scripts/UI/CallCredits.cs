using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CallCredits : MonoBehaviour
{
    // Call the credits scene
    public void PlayCredits()
    {
        SceneManager.LoadScene(3);
    }
}
