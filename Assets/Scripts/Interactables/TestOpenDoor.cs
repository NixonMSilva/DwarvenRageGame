using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOpenDoor : MonoBehaviour, IInteractable
{
    private GameObject player;
    [SerializeField] private Transform teleportLocation;

    [SerializeField] private bool canTeleport = true;

    private void Awake() 
    {
        player = GameObject.Find("Player");
    }

    public void OnInteraction ()
    {
        if (canTeleport)
        {
            Debug.Log("Interacted!");
            canTeleport = false;
            ActionOnTimer teleportEvent = gameObject.AddComponent<ActionOnTimer>();
            UserInterfaceController.instance.FadeInToBlack(0.5f);
            teleportEvent.SetTimer(0.5f, () =>
            {
                UserInterfaceController.instance.FadeOutFromBlack(0.5f);
                player.transform.position = teleportLocation.position;
                canTeleport = true;
                Destroy(teleportEvent);
            });
        }
        
        
    }
}
