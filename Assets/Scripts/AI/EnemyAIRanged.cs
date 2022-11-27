using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIRanged : EnemyAI
{
    private bool hasMovedToNewPoint = true;
    private bool hasAcquiredNewPoint = false;

    [SerializeField] private string projectileCastName;

    private Vector3 newPoint;

    public override void AttackPlayer ()
    {
        if (!hasMovedToNewPoint)
        {
            MoveToNewPoint();
        }
       
        if (!alreadyAttacked)
        {
            StopForAttack();
            PerformAttack();
        }
    }

    // Makes the ranged enemies move to a new point
    // so they don't stand still when attack
    // (That means no camping basically)
    private void MoveToNewPoint ()
    {
        if (Vector3.Distance(transform.position, newPoint) <= 1f)
        {
            hasMovedToNewPoint = true;
        }
        else
        {
            if (!hasAcquiredNewPoint)
            {
                newPoint = SearchWalkPoint();
                agent.SetDestination(newPoint);
                hasAcquiredNewPoint = true;
            }
        }
    }

    public void SpawnProjectile ()
    {
        attack.CreateProjectile(player.transform, projectileCastName);
    }

    private void PerformAttack ()
    {
        anim.Play("attack_right");
        isAttacking = true;
        alreadyAttacked = true;
        hasMovedToNewPoint = false;
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }

    // Move to a new point before attacking again

}
