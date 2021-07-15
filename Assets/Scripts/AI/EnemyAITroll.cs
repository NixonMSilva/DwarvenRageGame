using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAITroll : BossAI
{
    [SerializeField] private Sound[] tauntSounds;

    [SerializeField] private bool canTaunt = true;

    [SerializeField] private float tauntCooldown = 30f;

    private float originalPainThreshold = 0f;

    [SerializeField] private bool isPlayerOnPlatform = false;

    public bool PlayerOnPlatform
    {
        get => isPlayerOnPlatform;
        set => isPlayerOnPlatform = value;
    }

    private void Start ()
    {
        originalPainThreshold = status.PainThreshold;
        status.PainThreshold = 0f;
    }

    private new void Update ()
    {
        if (!isPlayerOnPlatform && !CanAttackPlatform())
            base.Update();
        
        if (!isAttacking && isPlayerOnPlatform && CanAttackPlatform())
        {
            AttackPlatform();
        }

        if (canTaunt && !status.IsDying)
        {
            Taunt();
        }

        if (FightStage == 1 && status.Health / status.MaxHealth <= 0.5f)
        {
            // Initial invulnerability phase
            FightStage = 2;
        }
        else if (FightStage == 2 && status.Health / status.MaxHealth <= 0.3f)
        {
            // Last round
            FightStage = 3;
        }
        else if (FightStage == 3 && status.Health <= 0f)
        {
            // Deaded
            FightStage = 4;
        }
    }

    private bool CanAttackPlatform ()
    {
        return (Vector3.Distance(transform.position, playerPoint) <= 35f);
    }

    public override void AttackPlayer ()
    {
        if (!alreadyAttacked)
        {
            playerPoint = player.transform.position;
            StopForAttack();

            float diceRoll = Random.Range(0f, 1f);
            anim.Play(diceRoll < 0.2f ? "PowerAttack" : "Attack");

            isAttacking = true;
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }        
    }

    private void AttackPlatform ()
    {
        if (!alreadyAttacked)
        {
            playerPoint = player.transform.position;
            LookAtPlayer();
            StopForAttack();
            
            anim.Play("Attack");

            isAttacking = true;

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void Taunt ()
    {
        // Taunt logic
        canTaunt = false;
        AudioManager.instance.PlaySoundRandomAt(gameObject, "taunt");
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

    public override void HandleStageChange (int stage)
    {
        switch (stage)
        {
            case 0:
                anim.SetBool("hasStarted", true);
                return;
            case 1:
                status.PainThreshold = originalPainThreshold;
                return;
        }
    }
}
