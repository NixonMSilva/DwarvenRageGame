using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOpenDoor : MonoBehaviour, IInteractable
{
    private GameObject player;
    private CharacterController characterController;

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

        Debug.Log("Here! " + gameObject.name);
        characterController = player.GetComponent<CharacterController>();
        characterController.enabled = false;
        player.transform.position = teleportLocation.position;
        characterController.enabled = true;
        AudioManager.instance.PlaySoundAt(gameObject, "door_open");
    }
}
