using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGame : MonoBehaviour, IInteractable
{
    [SerializeField] private GameManager managerRef;

    private void Awake ()
    {
        managerRef = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void OnInteraction ()
    {
        GameManager.instance.LoadGame();
    }
}
