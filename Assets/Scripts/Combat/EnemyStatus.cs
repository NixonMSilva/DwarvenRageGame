using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStatus : StatusController
{
    public event System.Action<EnemyStatus> OnDeath;

    public event System.Action OnDeathEffect;

    private NavMeshAgent agent;

    private EnemyController enemy;

    private PlayerStatus player;

    [SerializeField] private float painThreshold = 0.25f;    

    public override float Speed
    {
        get { return agent.speed; }
        set { agent.speed = value; }
    }

    public EnemyController Enemy
    {
        get { return enemy; }
    }

    public PlayerStatus Player
    {
        get { return player; }
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

        player = GameObject.Find("Player").GetComponent<PlayerStatus>();
    }

    public override void Die ()
    {
        // Enemy death
        isDying = true;
        animator.Play("Death");
        PlayDeathSound();
        HandleDeath();
        Destroy(gameObject, 10f);
    }

    public void HandleDeath ()
    {
        OnDeath?.Invoke(this);
        // Handles the death for different types of enemies
        OnDeathEffect?.Invoke();
    }

    public override void TakeDamage (float value, DamageType type)
    {
        if (!isDying)
        {
            float newValue = value;

            // If resistance type is registered
            if (_resistances.ContainsKey(type))
                newValue *= (1f - _resistances[type]);

            PlayDamageSound();

            if (Random.Range(0f, 1f) >= painThreshold)
            {
                PlayDamageAnimation();
            }

            DeduceHealth(newValue);
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
