using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void Awake ()
    {
        InputHandler.instance.LockCursor(false);
        Time.timeScale = 1f;
    }

    //carrega a proxima cena
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
	
    public void QuitGame()
    {
        //Debug.Log("quit!");
        Application.Quit();
    }
}
