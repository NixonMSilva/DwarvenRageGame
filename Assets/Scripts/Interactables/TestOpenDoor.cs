using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOpenDoor : MonoBehaviour, IInteractable
{
    private GameObject player;
    
    [SerializeField] private float positionX;
    [SerializeField] private float positionY;
    [SerializeField] private float positionZ;
    private void Awake() 
    {
        player = GameObject.Find("Player");
    }

    public void OnInteraction ()
    {
       Debug.Log(player.transform.position);
       Debug.Log("Entrou na dungeon!");

       Vector3 position;
        position.x = positionX;
        position.y = positionY;
        position.z = positionZ;

        player.transform.position = position;

        Debug.Log(player.transform.position);
    }
}
