using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Events;

public class PigSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _spawnPrefabs;
    [SerializeField] private Transform[] _spawnPoints;

    [SerializeField] private BossFightController bossFightController;
    
    [SerializeField] private float spawnCooldown = 5f;

    [SerializeField] private float spawnLimit = 8;

    [SerializeField] private List<GameObject> _spawns;

    [SerializeField] private UnityEvent onSpawnedDeath;

    public bool CanSpawn { get; set; }

    private void Awake ()
    {
        CanSpawn = false;
    }

    private void Update()
    {
        if (!CanSpawn || _spawns.Count >= spawnLimit)
            return;
        
        SpawnEnemy();
    }

    private void SpawnEnemy ()
    {
        ActionOnTimer spawnCooldownComponent = gameObject.AddComponent<ActionOnTimer>();
        
        // Dice roll to see which enemy to spawn and where
        int diceRollPoint = (int) Random.Range(0f, _spawnPoints.Length - 1);
        int diceRollEnemy = (int) Random.Range(0f, _spawnPrefabs.Length - 1);

        // Spawn the enemy
        GameObject enemy = Instantiate(_spawnPrefabs[diceRollEnemy], _spawnPoints[diceRollPoint].position, Quaternion.identity);
        enemy.GetComponent<EnemyStatus>().OnDeath += HandleSpawnedDeath;
        _spawns.Add(enemy);

        // Spawn cooldown handling
        CanSpawn = false;
        spawnCooldownComponent.SetTimer(spawnCooldown, () =>
        {
            CanSpawn = true;
            Destroy(spawnCooldownComponent);
        });
    }

    private void HandleSpawnedDeath (EnemyStatus obj)
    {
        onSpawnedDeath.Invoke();
    }
}
