using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIRanged : EnemyAI
{
    [SerializeField] private GameObject projectile;

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

    public void CreateProjectile ()
    {
        //LookAtPlayer();
        GameObject attackProjectile;
        attackProjectile = Instantiate(projectile, attack.AttackPoint.position, Quaternion.identity);
        attackProjectile.GetComponent<ProjectileController>().SetDirection(player.transform.position - attack.AttackPoint.position);
    }
}
