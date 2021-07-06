using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Enemy enemyType;

    private EnemyStatus enemyStatus;
    private AttackController enemyAttack;

    [SerializeField] private UnityEvent<GameObject> OnDeathExtraEffect;

    [SerializeField] private bool useFromDatabase = true;
	
    public Enemy Type
    {
        get { return enemyType; }
    }

    private void Awake ()
    {
        enemyStatus             = GetComponent<EnemyStatus>();
        enemyAttack             = GetComponent<AttackController>();

        enemyStatus.Health      = enemyType.maxHealth;
        enemyStatus.MaxHealth   = enemyType.maxHealth;

        if (useFromDatabase)
        {
            enemyAttack.Damage = enemyType.attackDamage;
        }
    }

    private void Start ()
    {
        enemyStatus.OnDeathEffect += DeathEffect;
    }

    private void DeathEffect ()
    {
        // Spawn loot
        SpawnLoot();

        // Add gold
        GoldDrop();

        // Extra effect
        OnDeathExtraEffect.Invoke(gameObject);
    }

    public void SpawnLoot ()
    {
        for (int i = 0; i < enemyType.drops.Length; ++i)
        {
            float diceRoll = UnityEngine.Random.Range(0f, 1f);
            if (diceRoll <= enemyType.drops[i].dropChance)
            {
                Instantiate(enemyType.drops[i].item, transform.position, Quaternion.identity);
                break;
            }
        }
    }

    public void GoldDrop ()
    {
        int delta = enemyType.goldDrop + UnityEngine.Random.Range(-enemyType.goldDropVariance, enemyType.goldDropVariance + 1);
        delta = (int)(delta * enemyStatus.Player.GoldDropRate);
        enemyStatus.Player.Equipment.Gold += delta;
        UserInterfaceController.instance.UpdateGoldCount(enemyStatus.Player.Equipment.Gold, delta);
    }

    private void OnDestroy ()
    {
        enemyStatus.OnDeathEffect -= DeathEffect;
    }
}
