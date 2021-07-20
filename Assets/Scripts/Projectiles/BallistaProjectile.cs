using UnityEngine;

public class BallistaProjectile : ProjectileController
{
    
    public override void OnTriggerEnter (Collider other)
    {
        // Forces the dragon boss to Land
        if (other.gameObject.CompareTag("Boss"))
        {
            EnemyAIUsurper enemy;
            if ((enemy = other.gameObject.GetComponentInParent<EnemyAIUsurper>()) != null)
            {
                if (enemy.Flying)
                {
                    enemy.Land();
                }
            }
            
            int layerId = other.gameObject.layer;
            string layerName = LayerMask.LayerToName(layerId);
        
            // Collide if it's on the collidable layer or it isn't the caster;
            if (_canCollideWith.Contains(layerName) && !other.gameObject.Equals(caster))
            {
                IDamageable target;
                if ((target = other.gameObject.GetComponentInParent<IDamageable>()) != null)
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
                }
                Destroy(gameObject);
            }
        }
    }
}
