using UnityEngine;

public class BallistaProjectile : ProjectileController
{
    public override void OnTriggerEnter (Collider other)
    {
        base.OnTriggerEnter(other);
        
        // Forces the dragon boss to Land
        if (other.gameObject.CompareTag("Boss"))
        {
            if (TryGetComponent<EnemyAIUsurper>(out var enemy))
            {
                enemy.FightStage -= 3;
            }
        }
    }
}
