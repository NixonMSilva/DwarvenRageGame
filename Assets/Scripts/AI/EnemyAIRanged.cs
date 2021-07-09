using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIRanged : EnemyAI
{
    public override void AttackPlayer ()
    {
        //LookAtPlayer();
        StopForAttack();
       
        if (!alreadyAttacked)
        {
            anim.Play("attack_right");
            isAttacking = true;
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    public void SpawnProjectile ()
    {
        attack.CreateProjectile(player.transform);
    }
}
