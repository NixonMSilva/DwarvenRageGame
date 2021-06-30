using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    SaveController manager;

    private void Awake() 
    {
        manager = GameObject.Find("GameManager").GetComponent<SaveController>();
    }

    public void OnInteraction ()
    {
        manager.SaveGame();
        Debug.Log("Game saved!");
    }
}
