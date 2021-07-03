using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIBerserk : EnemyAI
{
    public override void AttackPlayer()
    {
        playerPoint = player.transform.position;
        StopForAttack();

        if (!alreadyAttacked)
        {
            anim.Play("attack_right");
            isAttacking = true;
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

}
