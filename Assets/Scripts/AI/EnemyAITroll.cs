using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAITroll : BossAI
{
    [SerializeField] private Sound[] tauntSounds;

    private bool canTaunt = true;
    [SerializeField] private float tauntCooldown = 2f;

    private new void Update ()
    {
        base.Update();

        if (playerInSightRange && canTaunt && !status.IsDying)
        {
            Taunt();
        }

        if (FightStage == 0 && status.Health / status.MaxHealth <= 0.5f)
        {
            // Second stage of battle
            FightStage = 1;
        }
    }

    public override void AttackPlayer ()
    {
        if (!alreadyAttacked)
        {
            playerPoint = player.transform.position;
            StopForAttack();

            float diceRoll = Random.Range(0f, 1f);
            if (diceRoll < 0.2f)
            {
                // Power attack
                anim.Play("PowerAttack");
            }
            else
            {
                // Normal attack
                anim.Play("Attack");
            }

            isAttacking = true;
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }        
    }

    private void Taunt ()
    {
        // Taunt logic
        canTaunt = false;
        ActionOnTimer onTimeAction = gameObject.AddComponent<ActionOnTimer>();
        onTimeAction.SetTimer(tauntCooldown, () =>
        {
            canTaunt = true;
            Destroy(onTimeAction);
        });
    }

    private new void ResetAttack ()
    {
        alreadyAttacked = false;
        // In case he gets stuck into attack mode
        isAttacking = false;
    }
}
