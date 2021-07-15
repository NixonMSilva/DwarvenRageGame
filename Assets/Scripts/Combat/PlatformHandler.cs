using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformHandler : MonoBehaviour
{ 
    [SerializeField] private EnemyAITroll bossAI;

    private void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            bossAI.PlayerOnPlatform = true;
        }
    }
    private void OnTriggerExit (Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            bossAI.PlayerOnPlatform = false;
        }
    }
    
}
