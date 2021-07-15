using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIUsurper : BossAI
{
    [SerializeField] private Transform attackPoint;

    private bool isBlocking = false;

    protected new void Update()
    {
        if (status.IsBlocking)
            return;
        
        if (CanBlock())
        {
            anim.Play("defend");
        }
        else
        {
            base.Update();
        }
        
    }

    public override bool IsPlayerInAttackRange ()
    {
        return Physics.CheckSphere(attackPoint.position, attackRange, whatIsPlayer);
    }
    
    public override void AttackPlayer ()
    {
        if (!alreadyAttacked)
        {
            playerPoint = player.transform.position;
            StopForAttack();

            float diceRoll = Random.Range(0f, 1f);
            anim.Play(diceRoll < 0.2f ? "attack_left" : "attack_right");

            isAttacking = true;
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }        
    }

    private bool CanBlock ()
    {
        if (Random.Range(0f, 1f) >= 98f)
        {
            return true;
        }

        return false;
    }

    public void ActivateBlockingStatus ()
    {
        status.IsBlocking = true;
    }

    public void DeactivateBlockingStatus()
    {
        status.IsBlocking = false;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
