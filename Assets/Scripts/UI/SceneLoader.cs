using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene (int index)
    {
        // Stop player from inputting into the character
        InputHandler.instance.LockCursor(false);

        // Saves all the status on the current scene for the next one
        GameManager.instance.SaveCurrentSceneStatus();
        
        // Show the loading screen
        UserInterfaceController.instance.ShowLoadingMenu();

        // Start the loading operation
        StartCoroutine((LoadAsynchronously(index)));
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
}
