using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    [SerializeField] private GameObject loadingScreen;
        
	private void Awake ()
	{
		Time.timeScale = 1f;
	}
	
    private void Start ()
    {
        InputHandler.instance.LockCursor(false);
    }

    public void ShowLoadingScreen()
    {
        loadingScreen.SetActive(true);
    }
    
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
	
    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetQuality (int qualityIndex)
    {
        Debug.Log(qualityIndex);
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    
}
