using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SkullProjectile : ProjectileController
{
    public override void OnTriggerEnter (Collider other)
    {
        int layerId = other.gameObject.layer;
        string layerName = LayerMask.LayerToName(layerId);
        
        // Collide if it's on the collisible layer or it isn't the caster;
        if (_canCollideWith.Contains(layerName) && !other.gameObject.Equals(caster))
        {
            IDamageable target;
            if (other.gameObject.TryGetComponent<IDamageable>(out target))
            {
                // Check if the target isn't blocking
                target.CheckForBlock(transform);
                
                // Break the invulnerability of the troll
                if (IsTroll(other.gameObject))
                {
                    var boss = GameObject.Find("BossFight").GetComponent<Boss1FightController>();
                    boss.ResetResistance();
                    boss.ResetColor();
                }
                
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
                
            }
            Destroy(gameObject);
        }
    }

    private bool IsTroll (GameObject obj)
    {
        return obj.gameObject.name.Contains("Troll");
    }
}
