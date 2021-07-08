using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : EnemyAI
{
    [SerializeField] private float fightStageChangeThreshold;

    private int fightStage = -1;

    private Vector3 playerFixedPoint;

    public int FightStage
    {
        get { return fightStage; }
        set { fightStage = value; }
    }

    protected new void Update ()
    {
        if (CanAct() && fightStage >= 0)
        {
            // Updates to check if the player is in attack range
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

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
        else if (fightStage < 0)
        {
            Patroling();
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

        switch (fightStage)
        {
            default:
                break;
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
        }
    }

    private void LookAtFixedPoint (Vector3 point)
    {
        Debug.DrawRay(point, Vector3.up * 100f, Color.cyan, 10f);
        Vector3 lookPosition = point - transform.position;
        lookPosition.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(lookPosition);
        //Quaternion.Slerp(lookRotation, transform.rotation, 0.25f);
        transform.rotation = lookRotation;
    }

    private new void ChasePlayer ()
    {
        agent.SetDestination(player.position);
    }

    private bool CanAct ()
    {
        return (!status.IsDying && !isAttacking && !isBeingStaggered);
    }
}
