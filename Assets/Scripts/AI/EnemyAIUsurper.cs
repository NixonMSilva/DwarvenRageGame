using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyAIUsurper : BossAI
{
    [SerializeField] private Transform attackPoint;

    private bool isBlocking = false;
    
    private bool isFlying = false;

    private float sqrAttackDistance;
    
    // 0 - Basic | Breath | Claw | Block
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
                playerFixedPoint = player.position;
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

        if (isAttacking || status.IsDying)
        {
            LookAtFixedPoint(playerFixedPoint);
            
            if (status.IsDying)
                StandStill();
        }
    }

    private void MakeNextDecision ()
    {
        // Do nothing if on decision cooldown
        if (alreadyAttacked)
            return;
        
        // Dice roll for the decision
        float diceRoll = UnityEngine.Random.Range(0f, 100f);
        
        Debug.Log("Dice roll: " + diceRoll);
        
        // Pick decision
        int decision = ProcessDecision(FightStage, diceRoll);
        
        Debug.Log ("Decision taken: " + decision);

        // If is flying
        if (isFlying)
        {
            CastFireballs();
            return;
        }

        switch (decision)
        {
            case 0:
                AttackPlayer();
                break;
            case 1:
                AttackPlayer();
                break;
            case 2:
                AttackPlayer();
                break;
            case 3:
                Block();
                break;
            default:
                AttackPlayer();
                break;
        }
    }

    private int ProcessDecision (int stage, float roll, Action callback = null)
    {
        float[] weightList = new float[1];

        switch (stage)
        {
            case 1:
                weightList = weights;
                break;
            case 2:
                weightList = weights2;
                break;
            case 3:
                weightList = weights3;
                break;
            default:
                weightList = weights;
                break;
        }

        for (int i = 0; i < weightList.Length; ++i)
        {
            Debug.Log(i);
            if (weightList[i] <= roll)
                return i;
        }

        return 0;
    }
    
    /*
    public override void AttackPlayer ()
    {
        if (!alreadyAttacked)
        {
            playerPoint = player.transform.position;
            StopForAttack();

            float diceRoll = UnityEngine.Random.Range(0f, 1f);
            anim.Play(diceRoll > 0.2f ? "attack_left" : "attack_right");

            isAttacking = true;
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }        
    } */

    private void AttackNormal ()
    {
        StopForAttack();

        anim.Play("Basic Attack");
        
        isAttacking = true;
        alreadyAttacked = true;
        
        ResetAttackTimer();
    }

    private void AttackBreath ()
    {
        StopForAttack();
        
        anim.Play("Fly Flame Attack");
        
        isAttacking = true;
        alreadyAttacked = true;
        
        ResetAttackTimer();
    }

    private void AttackClaw ()
    {
        StopForAttack();
        
        anim.Play("Claw Attack");
        
        isAttacking = true;
        alreadyAttacked = true;

        ResetAttackTimer();
    }
    
    
    private void Block ()
    {
        StopForAttack();
        
        anim.Play("Defend");
        
        isAttacking = true;
        alreadyAttacked = true;

        ResetAttackTimer();
    }
    
    public void Fly ()
    {
        isFlying = true;
        anim.SetBool("isFlying", true);
    }

    public void Land ()
    {
        isFlying = false;
        anim.SetBool("isFlying", false);
    }
    
    private void CastFireballs ()
    {
        anim.Play("Fly Flame Attack");
    }
    
    private void ResetAttackTimer () { Invoke(nameof(ResetAttack), timeBetweenAttacks); }
    
    public override bool IsPlayerInAttackRange ()
    {
        return (sqrAttackDistance <= Vector3.SqrMagnitude(player.position - transform.position));
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

    public void CheckHealth (float health, float maxHealth)
    {
        float currentHealthPercentage = health / maxHealth;

        // Don't reprocess if the fight stage is 4 (flight)
        if (FightStage == 4)
            return;
        
        // Flight triggers
        if (currentHealthPercentage < 65f && FightStage == 1)
        {
            FightStage = 4;
        }
        else if (currentHealthPercentage < 45f && FightStage == 2)
        {
            FightStage = 4;
        }
        else if (currentHealthPercentage < 25f && FightStage == 3)
        {
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
            case 0:
                anim.SetBool("hasStarted", true);
                break;
            case 1:
                if (isFlying)
                    Land();
                break;
            case 2:
                if (isFlying)
                    Land();
                break;
            case 3:
                if (isFlying)
                    Land();
                break;
            case 4:
                if (isFlying)
                    Fly();
                break;
        }
    }
}
