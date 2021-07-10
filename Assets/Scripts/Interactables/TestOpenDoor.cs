using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOpenDoor : MonoBehaviour, IInteractable
{
    private GameObject player;
    private CharacterController characterController;

    private GameObject chave;
    private TooltipController chavecollider;
    private bool haschave = false;

    private EnemySpawnManager enemies;
    private bool[] boss;
    
    [SerializeField] private string nomechave;

    [SerializeField] private Transform teleportLocation;

    private void Awake() 
    {
        player = GameObject.Find("Player");
    }

    public void OnInteraction ()
    {
        if (teleportLocation == null)
        {
            Debug.LogWarning("Door destination not set!");
            return;
        }

        //Debug.Log("Here! " + gameObject.name);
        characterController = player.GetComponent<CharacterController>();
        characterController.enabled = false;
        player.transform.position = teleportLocation.position;
        characterController.enabled = true;
        AudioManager.instance.PlaySoundAt(gameObject, "door_open");

        //Debug.Log("Here! " + gameObject.name);
        characterController = player.GetComponent<CharacterController>();
        characterController.enabled = false;
        player.transform.position = teleportLocation.position;
        characterController.enabled = true;
        AudioManager.instance.PlaySoundAt(gameObject, "door_open");
    }

     public void PegarChave ()
    {
       chavecollider.enabled = false;
       chave.SetActive(false);
       haschave = true;
       
       Debug.Log(haschave);
    }

    public void EntrarPortaTrancada ()
    {
        
       if(!haschave)
       {
           Debug.Log("trancado");
       }
       else
       {
            Debug.Log("Pode entrar");
            OnInteraction ();
       }
       
    }

    public void PortaBoss ()
    {
        enemies = GameObject.Find("Enemies").GetComponent<EnemySpawnManager>();
        boss = enemies.GetKilledList();
        for (int i = 0; i < boss.Length; ++i)
        {
            if(boss[2] == false)
            {
                Debug.Log("Boss foi morto");
                haschave = true;
            }
            
        }
        if(!haschave)
        {
           Debug.Log("trancado");
        }
        else
        {
            Debug.Log("Pode entrar");
            OnInteraction ();
        }

    }
}
