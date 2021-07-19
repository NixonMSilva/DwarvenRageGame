using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : EnemyAI
{
    [SerializeField] private float fightStageChangeThreshold;

    public Action<int> onFightStageChange;

    // -1 Pre-Start | 0 - Intro | 1 - Post-Intro
    [SerializeField] private int fightStage = -1;

    protected Vector3 playerFixedPoint;

    public int FightStage
    {
        get => fightStage;
        set 
        {
            fightStage = value;
            onFightStageChange?.Invoke(fightStage);
            HandleStageChange(fightStage);
        }
    }

    protected new void Update ()
    {
        // Only proceed to normal routines
        if (CanAct() && fightStage >= 1)
        {
            // Updates to check if the player is in attack range
            playerInAttackRange = IsPlayerInAttackRange();

            UpdateSpeed();

            // If the player is in attack range, attacks,
            // else, chase him
            if (playerInAttackRange)
            {
                AttackPlayer();
                playerFixedPoint = player.position;
            }
            else
            {
                ChasePlayer();
            }
        }
        else if (isAttacking)
        {
            LookAtFixedPoint(playerFixedPoint);
        }
        else if (status.IsDying)
        {
            LookAtFixedPoint(playerFixedPoint);
            agent.SetDestination(transform.position);
        }
        else
        {
            
        }
    }

    public virtual bool IsPlayerInAttackRange ()
    {
        return Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
    }

    protected void LookAtFixedPoint (Vector3 point)
    {
        Debug.DrawRay(point, Vector3.up * 100f, Color.cyan, 10f);
        Vector3 lookPosition = point - transform.position;
        lookPosition.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(lookPosition);
        //Quaternion.Slerp(lookRotation, transform.rotation, 0.25f);
        transform.rotation = lookRotation;
    }

    protected new void ChasePlayer ()
    {
        agent.SetDestination(player.position);
    }

    protected bool CanAct ()
    {
        return (!status.IsDying && !isAttacking && !isBeingStaggered);
    }
    
    public virtual void HandleStageChange (int stage)
    {
    }
}
