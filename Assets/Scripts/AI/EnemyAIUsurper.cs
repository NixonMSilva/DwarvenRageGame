using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAIUsurper : BossAI
{
    [SerializeField] private Transform attackPoint;

    private bool isBlocking = false;
    
    private bool isFlying = false;

    private float sqrAttackDistance;

    
    private float[] weights = { 30f, 50f, 70f, 95f };
    private float[] weights2 = { 40f, 60f, 80f, 95f };
    private float[] weights3 = { 60f, 70f, 80f, 90f };

    private void Start ()
    {
        sqrAttackDistance = attackRange * attackRange;
        status.OnHealthChange += CheckHealth;
    }

    private void OnDestroy ()
    {
        status.OnHealthChange -= CheckHealth;
    }

    protected new void Update()
    {
        // Cull if player is too distant
        if (Vector3.SqrMagnitude(player.position - transform.position) > 22000f)
            return;

        // Process normally if the enemy can act or it's not blocking or the fight stage is invalid
        if (CanAct() && !status.IsBlocking && IsFightStageValid())
        {
            // Updates to check if the player is in attack range
            if (IsPlayerInAttackRange())
            {
                MakeNextDecision();
            }
            else
            {
                ChasePlayer();
            }
        }
        else
        {
            StandStill();
        }
    }

    private void MakeNextDecision ()
    {
        // Attack type or block
        
    }
    
    public override void AttackPlayer ()
    {
        if (!alreadyAttacked)
        {
            playerPoint = player.transform.position;
            StopForAttack();

            float diceRoll = Random.Range(0f, 1f);
            anim.Play(diceRoll > 0.2f ? "attack_left" : "attack_right");

            isAttacking = true;
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }        
    }
    
    public override bool IsPlayerInAttackRange ()
    {
        return (sqrAttackDistance <= Vector3.SqrMagnitude(player.position - transform.position));
    }

    private bool CanBlock ()
    {
        if (Random.Range(0f, 1f) >= 98f)
        {
            return true;
        }

        return false;
    }

    private void StandStill ()
    {
        agent.SetDestination(transform.position);
    }

    public void ActivateBlockingStatus ()
    {
        status.IsBlocking = true;
    }

    public void DeactivateBlockingStatus()
    {
        status.IsBlocking = false;
    }

    public void LeapForFatalAttack ()
    {
        // Leap for fatal attack
    }

    public void SpawnFlames ()
    {
        // Spawn flames    
    }
    
    public void Fly ()
    {
        
    }

    public void Land ()
    {
        
    }

    public void CheckHealth (float health, float maxHealth)
    {
        float currentHealthPercentage = health / maxHealth;

        if (currentHealthPercentage < 60f)
        {
            // Flight
            FightStage = 4;
        }
    }

    public bool IsFightStageValid ()
    {
        return (FightStage >= 1);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    public override void HandleStageChange (int stage)
    {
        switch (stage)
        {
            
        }
    }
}
