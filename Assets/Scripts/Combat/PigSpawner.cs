using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _spawnPrefabs;
    [SerializeField] private Transform[] _spawnPoints;

    public void SpawnEnemy ()
    {
        // Dice roll to see which enemy to spawn and where
        int diceRollPoint = (int) Random.Range(0f, _spawnPoints.Length - 1);
        int diceRollEnemy = (int) Random.Range(0f, _spawnPrefabs.Length - 1);

        // Spawn the enemy
        Instantiate(_spawnPrefabs[diceRollEnemy], _spawnPoints[diceRollPoint].position, Quaternion.identity);
    }
}
