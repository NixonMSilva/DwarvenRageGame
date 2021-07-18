using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalProjectile : ProjectileController
{
    private void Start ()
    {
        // Destruir o projétil caso este se perca por 10 segundos
        Destroy(gameObject, 15f);
        rigidBody.velocity = new Vector3(0f, 40f, 0f);
    }
    
    public override void OnTriggerEnter (Collider other)
    {
        // Don't do anything if it's the boss
        if (other.gameObject.CompareTag("Boss"))
            return;
        
        int layerId = other.gameObject.layer;
        string layerName = LayerMask.LayerToName(layerId);
        
        // Collide if it's on the collidable layer or it isn't the caster;
        if (_canCollideWith.Contains(layerName) && !other.gameObject.Equals(caster))
        {
            IDamageable target;
            if (other.gameObject.TryGetComponent<IDamageable>(out target))
            {
                // Check if the target isn't blocking
                target.CheckForBlock(transform);
                
                if (effect != null)
                {
                    target.TakeDamage(damageValue, damageType, effect);
                }
                else
                {
                    target.TakeDamage(damageValue, damageType);
                }

                // Make the target bleed if possible
                if (bleedOnImpact)
                {
                    target.SpawnBlood(transform.position);
                }
                
                Destroy(this);
            }
        }
        
        
    }
}
