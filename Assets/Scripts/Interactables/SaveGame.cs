using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SaveGame : MonoBehaviour, IInteractable
{ 
    [SerializeField] private GameManager managerRef;

    [SerializeField] private UnityEvent onGameStart;
    private void Start() 
    {
        onGameStart?.Invoke();
    }
    private void Awake ()
    {
        managerRef = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void OnInteraction ()
    {
        managerRef.SaveGame();
        
    }
}
