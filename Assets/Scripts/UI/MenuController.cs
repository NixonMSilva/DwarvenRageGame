using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	private void Awake ()
	{
		Time.timeScale = 1f;
	}
	
    private void Start ()
    {
        InputHandler.instance.LockCursor(false);
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
