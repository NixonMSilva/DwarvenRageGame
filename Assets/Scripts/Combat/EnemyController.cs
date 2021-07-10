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

    [SerializeField] private GameObject deathExtraEffectPoint;
    [SerializeField] private GameObject deathExtraEffectFX;

    [SerializeField] private bool useFromDatabase = true;

    [SerializeField] private Transform lootSpawnPoint;
	
    public Enemy Type
    {
        get { return enemyType; }
    }

    private void Awake ()
    {
        enemyStatus             = GetComponent<EnemyStatus>();
        enemyAttack             = GetComponent<AttackController>();

        enemyStatus.MaxHealth   = enemyType.maxHealth;
        enemyStatus.Health      = enemyType.maxHealth;

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
    }

    private void SpawnLoot ()
    {
        foreach (var t in enemyType.drops)
        {
            float diceRoll = UnityEngine.Random.Range(0f, 1f);
            if (diceRoll <= t.dropChance)
            {
                Instantiate(t.item, lootSpawnPoint.position, Quaternion.LookRotation(Vector3.up));
                break;
            }
        }
    }

    private void GoldDrop ()
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

    public void DeathExtraEffect ()
    {
        // I give up, switch-case statements here are my way of doing this shit
        switch (enemyType.enemyName)
        {
            case "Fat Pig":
                GameObject blood = Instantiate(deathExtraEffectFX, deathExtraEffectPoint.transform.position, Quaternion.identity);
                Collider[] hit = Physics.OverlapSphere(deathExtraEffectPoint.transform.position, 5f, enemyAttack.Damageables);
                foreach  (Collider obj in hit)
                {
                    StatusController controller;
                    if (obj.TryGetComponent<StatusController>(out controller))
                    {
                        controller.TakeDamage(100f, DamageType.physical);
                    }
                }
                Destroy(blood, 5f);
                break;
            default:
                break;
        }
    }
}
