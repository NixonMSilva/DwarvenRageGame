using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyAIUsurper : BossAI
{
    [SerializeField] private Transform attackPoint;

    private bool isBlocking = false;

    private bool isLocked = false;
    
    [SerializeField] private bool isFlying = false;

    // 0 - Basic | Breath | Claw | Block
    private float[] weights = { 30f, 50f, 70f, 95f };
    private float[] weights2 = { 40f, 60f, 80f, 95f };
    private float[] weights3 = { 60f, 70f, 80f, 90f };

    private bool[] stageTriggers = { false, false, false };

    [SerializeField] private Transform rootBoneTransform;

    public bool Flying
    {
        get => isFlying;
        set => isFlying = value;
    }

    public bool Locked
    {
        get => isLocked;
        set => isLocked = value;
    }
    
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
            if (!isFlying)
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
                if (!isAttacking)
                {
                    LookAtFixedPoint(playerFixedPoint);
                    CastFireballs();
                }
                    
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
                AttackBreath();
                break;
            case 1:
                AttackBreath();
                break;
            case 2:
                AttackBreath();
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
        
        anim.Play("Basic Attack");
        //anim.Play("Claw Attack");
        
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
        FightStage = 4;
        anim.SetBool("isFlying", true);
        
    }

    public void Land ()
    {
        Debug.Log("Land!");
        StandStill();
        FightStage = 1;
        anim.SetBool("isFlying", false);
        isFlying = false;
    }
    
    private void CastFireballs ()
    {
        StopForAttack();
        
        anim.Play("Fly Flame Attack");

        isAttacking = true;
        alreadyAttacked = true;
        
        ResetAttackTimer();
    }
    
    private void SpawnFireballs ()
    {
        attack.CreateProjectile(rootBoneTransform.forward);
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

        // Flight triggers
        if (currentHealthPercentage < 0.95f && stageTriggers[0] == false && !isFlying)
        {
            stageTriggers[0] = true;
            Fly();
            
        }

        if (currentHealthPercentage < 0.5f && stageTriggers[1] == false && !isFlying)
        {
            stageTriggers[1] = true;
            Fly();
        }

        if (currentHealthPercentage < 0.3f && stageTriggers[2] == false && !isFlying)
        {
            stageTriggers[2] = true;
            Fly();
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
        }
    }
    
    private new bool CanAct ()
    {
        return (!status.IsDying && !isAttacking && !isBeingStaggered && !isLocked);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(rootBoneTransform.position, rootBoneTransform.forward * 30f);
    }
    
}
