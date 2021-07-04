using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStatus : StatusController
{
    public event System.Action<EnemyStatus> OnDeath;

    private NavMeshAgent agent;

    private EnemyController enemy;

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
        enemy = GetComponent<EnemyController>();
    }

    public override void Die ()
    {
        // Enemy death
        isDying = true;
        animator.Play("Death");
        GetComponent<EnemyController>().SpawnLoot();
        PlayDeathSound();
        HandleDeath();
        Destroy(gameObject, 10f);
    }

    public void HandleDeath ()
    {
        OnDeath?.Invoke(this);
    }

    public override void TakeDamage (float value)
    {
        if (!isDying)
        {
            base.TakeDamage(value);

            PlayDamageSound();

            if (Random.Range(0f, 1f) >= painThreshold)
            {
                PlayDamageAnimation();
            }
        }
        
    }

    private void PlayDamageAnimation ()
    {
        if (!isDying)
        {
            animator.Play("Hit");
        }
    }

    private new void PlayDamageSound ()
    {
        float verify = UnityEngine.Random.Range(0f, 1f);
        if (verify <= 0.7f)
        {
            AudioManager.instance.PlaySoundRandom(enemy.Type.soundDamage);
        }
    }

    public override void PlayImpactSound ()
    {
        AudioManager.instance.PlaySoundRandom(enemy.Type.impactType);
    }

    private void PlayDeathSound ()
    {
        AudioManager.instance.PlaySoundRandom(enemy.Type.soundDeath);
    }
}
