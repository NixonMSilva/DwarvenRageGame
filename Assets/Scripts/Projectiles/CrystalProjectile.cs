using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalProjectile : ProjectileController
{
    public override void OnTriggerEnter (Collider other)
    {
        // Don't do anything if it's the boss
        if (other.gameObject.CompareTag("Boss"))
            return;
        
        base.OnTriggerEnter(other);
    }
}
