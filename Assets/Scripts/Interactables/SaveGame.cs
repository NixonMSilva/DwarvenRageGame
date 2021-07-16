using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGame : MonoBehaviour, IInteractable
{ 
    [SerializeField] private GameManager managerRef;

    private void Awake ()
    {
        managerRef = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void OnInteraction ()
    {
        managerRef.SaveGame();
    }
}
