using UnityEngine;

public class BallistaProjectile : ProjectileController
{
    
    public override void OnTriggerEnter (Collider other)
    {
        // Forces the dragon boss to Land
        if (other.gameObject.CompareTag("Boss"))
        {
            if (other.TryGetComponent<EnemyAIUsurper>(out var enemy))
            {
                if (enemy.Flying)
                {
                    enemy.Land();
                }
            }
        }
        
        base.OnTriggerEnter(other);
    }
}
