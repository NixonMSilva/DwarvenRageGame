using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyStatus : StatusController
{
    public event System.Action<EnemyStatus> OnDeath;

    public event System.Action OnDeathEffect;
    
    public event System.Action<float, float> OnHealthChange;

    private NavMeshAgent agent;

    private EnemyController enemy;
    private EnemyAI intelligence;

    private PlayerStatus player;

    [SerializeField] private float painThreshold = 0.25f;
    [SerializeField] private LayerMask thisLayer;

    public override float Speed
    {
        get { return agent.speed; }
        set { agent.speed = value; }
    }
    
    public override float Health
    {
        get => health;
        set 
        { 
            health = value; 
            if (health > maxHealth)
            {
                health = maxHealth;
            }
            else if (health < 0f)
            {
                health = 0f;
            }
            OnHealthChange?.Invoke(health, maxHealth);
        }
    }

    public EnemyController Enemy
    {
        get { return enemy; }
    }

    public PlayerStatus Player
    {
        get { return player; }
    }

    public float PainThreshold
    {
        get { return painThreshold; }
        set { painThreshold = value; }
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
        intelligence = GetComponent<EnemyAI>();

        player = GameObject.Find("Player").GetComponent<PlayerStatus>();
    }

    protected override void Die ()
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
            
            float diceRoll = Random.Range(0f, 1f);
            if (diceRoll <= painThreshold)
            {
                PlayDamageSound();
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
            intelligence.StopForStagger();
        }
    }

    private new void PlayDamageSound ()
    {
        AudioManager.instance.PlaySoundRandomAt(gameObject, enemy.Type.soundDamage);
    }

    public override void PlayImpactSound ()
    {
        //AudioManager.instance.PlaySoundRandomAt(gameObject, enemy.Type.impactType);
    }

    private void PlayDeathSound ()
    {
        AudioManager.instance.PlaySoundRandomAt(gameObject, enemy.Type.soundDeath);
    }

    public override void SpawnBlood (Vector3 position) 
    {
        ParticleSystem bloodSystem = Instantiate(enemy.Type.bloodParticle, position, Quaternion.identity, gameObject.transform).GetComponent <ParticleSystem>();
        Destroy(bloodSystem.gameObject, bloodSystem.main.duration + 0.1f); ;
        Debug.DrawLine(position, Vector3.up * 100, Color.red, 10f);
    }

    public override void SpawnBlood (Transform position)
    {
        RaycastHit hit;
        ParticleSystem bloodSystem;

        bool hasHit = Physics.Raycast(position.position, position.forward, out hit, 1f, thisLayer);

        Debug.DrawRay(position.position, position.forward, Color.magenta, 10f);

        if (hasHit) 
        {
            bloodSystem = Instantiate(enemy.Type.bloodParticle, hit.point, Quaternion.identity, gameObject.transform).GetComponent<ParticleSystem>();
            //Debug.Log(hit.collider.gameObject);
            Debug.DrawRay(hit.point, Vector3.up * 100, Color.red, 10f);
        }
        else
        {
            bloodSystem = Instantiate(enemy.Type.bloodParticle, position.position + position.forward, Quaternion.identity, gameObject.transform).GetComponent<ParticleSystem>();
        }
        
        Destroy(bloodSystem.gameObject, bloodSystem.main.duration + 0.1f);
    }
}
