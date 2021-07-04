using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Enemy enemyType;

    private EnemyStatus enemyStatus;
    private AttackController enemyAttack;

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

    public void SpawnLoot ()
    {
        for (int i = 0; i < enemyType.drops.Length; ++i)
        {
            float diceRoll = Random.Range(0f, 1f);
            if (diceRoll <= enemyType.drops[i].dropChance)
            {
                Instantiate(enemyType.drops[i].item, transform.position, Quaternion.identity);
                break;
            }
        }
    }

    
}
