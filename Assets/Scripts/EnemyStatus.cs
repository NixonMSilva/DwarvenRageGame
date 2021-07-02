using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStatus : StatusController
{
    public event System.Action<EnemyStatus> OnDeath;

    private NavMeshAgent agent;

    [SerializeField] private float painThreshold = 0.25f;

    public override float Speed
    {
        get { return agent.speed; }
        set { agent.speed = value; }
    }

    private void Awake ()
    {
        Health = maxHealth;
        Armor = 0f;

        agent = GetComponent<NavMeshAgent>();
        speed = agent.speed;
        attack = GetComponent<AttackController>();
        animator = GetComponent<Animator>();
    }

    public override void Die ()
    {
        // Enemy death
        isDying = true;
        animator.Play("Death");
        GetComponent<EnemyController>().SpawnLoot();
        HandleDeath();
        Destroy(gameObject, 10f);
    }

    public void HandleDeath ()
    {
        OnDeath?.Invoke(this);
    }

    public override void TakeDamage (float value)
    {
        base.TakeDamage(value);
        //Debug.Log(MaxHealth * hurtThreshold);
        
        if (Random.Range(0f, 1f) >= painThreshold)
        {
            PlayDamageAnimation();
        }
    }

    private void PlayDamageAnimation ()
    {
        Debug.Log("Pain!");
        animator.Play("Hit");
    }
}
