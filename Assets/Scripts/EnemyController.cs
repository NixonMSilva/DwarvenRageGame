using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Enemy enemyType;

    private EnemyStatus enemyStatus;
    private AttackController enemyAttack;

    [SerializeField] private List<GameObject> _dropItem;
    [SerializeField] private List<float> _dropChance;

    [SerializeField] private bool useFromDatabase = false;

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
        for (int i = 0; i < _dropItem.Count; ++i)
        {
            float diceRoll = Random.Range(0f, 1f);
            if (diceRoll <= _dropChance[i])
            {
                Instantiate(_dropItem[i], transform.position, Quaternion.identity);
                break;
            }
        }
    }

    
}
