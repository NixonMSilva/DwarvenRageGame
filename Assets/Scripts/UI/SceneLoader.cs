using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    private void LoadScene (int index)
    {
        // Stop player from inputting into the character
        InputHandler.instance.LockCursor(false);

        // Show the loading screen
        UserInterfaceController.instance.ShowLoadingMenu();
        
        // Destroy all running sounds
        AudioManager.instance.DestroyAllSounds();

        // Start the loading operation
        StartCoroutine((LoadAsynchronously(index)));
    }

    public void LoadSceneWithData(int index)
    {
        // Saves all the status on the current scene for the next one
        GameManager.instance.SaveCurrentSceneStatus();
        
        LoadScene(index);
    }

    public void LoadSceneWithoutData (int index)
    {
        LoadScene(index);
    }

    public void LoadSceneWithPurge (int index)
    {
        GameManager.instance.PurgeData();
        
        LoadSceneWithoutData(index);
    }

    public void LoadSceneOnMenu (int index)
    {
        // Stop player from inputting into the character
        InputHandler.instance.LockCursor(false);

        // Destroy all running sounds
        AudioManager.instance.DestroyAllSounds();
        
        // Start the loading operation
        StartCoroutine((LoadAsynchronouslyOnMenu(index)));
    }

    public void ReloadScene ()
    {
        LoadSceneWithoutData(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator LoadAsynchronously (int index)
    {
        // Load the scene asynchronously
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(index);

        // Operates while the loading isn't done
        while (!loadingOperation.isDone)
        {
            // Calculate the loading progress
            float loadingProgress = Mathf.Clamp01(loadingOperation.progress / .9f);
            
            // Updates the slider
            UserInterfaceController.instance.UpdateLoadingSlider(loadingProgress);
            
            // Go to the next frame
            yield return null;
        }
        
        // Wipe user interface
        UserInterfaceController.instance.WipeInterface();
        
        // Updates scene number in manager
        GameManager.instance.SetCurrentScene(index);
    }
    
    IEnumerator LoadAsynchronouslyOnMenu (int index)
    {
        Slider loadingSlider = GameObject.Find("LoadingSliderMenu").GetComponent<Slider>();
        
        // Load the scene asynchronously
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(index);

        // Operates while the loading isn't done
        while (!loadingOperation.isDone)
        {
            // Calculate the loading progress
            float loadingProgress = Mathf.Clamp01(loadingOperation.progress / .9f);
            
            // Updates the slider
            loadingSlider.value = loadingProgress;
            
            // Go to the next frame
            yield return null;
        }
        
        // Wipe user interface
        UserInterfaceController.instance.WipeInterface();
        
        // Updates scene number in manager
        GameManager.instance.SetCurrentScene(index);
    }
}
