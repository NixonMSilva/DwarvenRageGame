using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Enemy enemyType;

    private StatusController damageStatus;

    private MeshFilter enemyMesh;

    private List<string> _dropName;

    private List<float> _dropChance;

    [SerializeField] private List<GameObject> _spawnItem;

    private void Awake ()
    {
        damageStatus            = GetComponent<StatusController>();
        enemyMesh               = GetComponent<MeshFilter>();
        _spawnItem              = GameObject.Find("GameManager").GetComponent<GameManager>().GetDropTypes();

        damageStatus.Health     = enemyType.maxHealth;
        damageStatus.MaxHealth  = enemyType.maxHealth;
        _dropName               = enemyType.dropList;
        _dropChance             = enemyType.dropChance;
    }

    public void SpawnLoot ()
    {
        for (int i = 0; i < _dropName.Count; ++i)
        {
            float diceRoll = Random.Range(0f, 1f);
            if (diceRoll <= _dropChance[i])
            {
                GameObject item = _spawnItem.Where(item => item.name.Equals(_dropName[i])).ToList().First<GameObject>();
                Instantiate(item, transform.position, Quaternion.identity);
                break;
            }
        }
        //Destroy(gameObject);
    }

    
}
