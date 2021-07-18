using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyAIUsurper : BossAI
{
    [SerializeField] private Transform attackPoint;

    private bool isBlocking = false;
    
    [SerializeField] private bool isFlying = false;

    // 0 - Basic | Breath | Claw | Block
    private float[] weights = { 30f, 50f, 70f, 95f };
    private float[] weights2 = { 40f, 60f, 80f, 95f };
    private float[] weights3 = { 60f, 70f, 80f, 90f };

    private void Start ()
    {
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
            // Update speed for animation purposes
            UpdateSpeed();
            
            // Updates to check if the player is in attack range
            if (IsPlayerInAttackRange())
            {
                StandStill();
                playerFixedPoint = player.position;
                MakeNextDecision();
            }
            else
            {
                if (!isAttacking)
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

        // Pick decision
        int decision = ProcessDecision(FightStage, diceRoll);

        // If is flying
        if (isFlying)
        {
            CastFireballs();
            return;
        }

        DecisionPicker(decision);
    }

    private int ProcessDecision (int stage, float roll, Action callback = null)
    {
        float[] weightList = new float[4];

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
            if (weightList[i] > roll)
            {
                return i;
            }
        }

        return 0;
    }

    private void DecisionPicker (int value)
    {
        switch (value)
        {
            case 0:
                AttackNormal();
                break;
            case 1:
                AttackBreath();
                break;
            case 2:
                AttackClaw();
                break;
            case 3:
                Block();
                break;
            default:
                AttackPlayer();
                break;
        }
    }

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
        
        anim.Play("Flame Attack");
        
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
        Debug.Log("Fly!");
        StandStill();
        anim.Play("Take Off");
        anim.SetBool("isFlying", true);
    }

    public void Land ()
    {
        StandStill();
        isFlying = false;
        anim.SetBool("isFlying", false);
    }
    
    private void CastFireballs ()
    {
        StopForAttack();
        
        anim.Play("Fly Flame Attack");

        isAttacking = true;
        alreadyAttacked = true;
        
        ResetAttackTimer();
    }

    private void ResetAttackTimer ()
    {
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }
    
    public override bool IsPlayerInAttackRange ()
    {
        return (attackRange >= (Vector3.Distance(transform.position, player.position)));
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

        // Don't reprocess if the fight stage is 4 or above (flight)
        if (FightStage >= 4)
            return;
        
        // Flight triggers
        if (currentHealthPercentage < 0.95f && FightStage == 1)
        {
            FightStage += 3;
        }
        else if (currentHealthPercentage < 0.45f && FightStage == 2)
        {
            FightStage += 3;
        }
        else if (currentHealthPercentage < 0.25f && FightStage == 3)
        {
            FightStage += 3;
        }
    }

    public bool IsFightStageValid ()
    {
        if (FightStage >= 1 && FightStage < 4)
            return true;
        return false;
    }
    
    private new void ResetAttack ()
    {
        alreadyAttacked = false;
        // In case he gets stuck into attack mode
        isAttacking = false;
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
                if (!isFlying)
                    Fly();
                break;
            case 5:
                if (!isFlying)
                    Fly();
                break;
            case 6:
                if (!isFlying)
                    Fly();
                break;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
    
}
