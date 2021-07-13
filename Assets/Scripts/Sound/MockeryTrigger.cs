using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockeryTrigger : MonoBehaviour
{
    private Collider[] colliderList;

    private void Start ()
    {
        colliderList = GetComponentsInChildren<Collider>();
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioManager.instance.PlaySound("mockery");
            foreach (Collider collider in colliderList)
            {
                collider.enabled = false;
            }
        }
    }
}
