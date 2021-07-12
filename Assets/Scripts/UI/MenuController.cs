using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    private void Start ()
    {
        InputHandler.instance.LockCursor(false);
        Time.timeScale = 1f;
    }
    
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
	
    public void QuitGame()
    {
        Application.Quit();
    }

    
}
