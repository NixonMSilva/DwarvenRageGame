using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    SaveController manager;

    private int aleatorio;
    private void Awake() 
    {
        manager = GameObject.Find("GameManager").GetComponent<SaveController>();
    }

    public void OnInteraction ()
    {
        manager.SaveGame();
        Debug.Log("Game saved!");
    }

    public void Conversar ()
    {
        aleatorio = Random.Range(1, 3);
        switch(aleatorio)
        {
            case 1:
            AudioManager.instance.PlaySound("bode");
            break;
            case 2:
            AudioManager.instance.PlaySound("bode1");
            break;
        }
        
    }
}
