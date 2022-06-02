using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class EnemySpawnManager : MonoBehaviour
{
    // [SerializeField] private List<EnemyStatus> _enemyList;

    [SerializeField] private List<bool> _wasNotKilled;

    [SerializeField] private List<EnemySpawnData> _spawnList;

    [SerializeField] private PlayerStatus player;

    private char[] limits = { '(', ')' };

    private PlayerStatus Player
    {
        get { return player; }
    }

    private void Awake ()
    {
        player = GameObject.Find("Player").GetComponent<PlayerStatus>();

        for (int i = 0; i < _spawnList.Count; ++i)
        {
            _spawnList[i].isAlive = true;
            _spawnList[i].status.OnDeath += HandleEnemyDeath;
            _spawnList[i].status.UniqueId = i;
        }
    }

    private void HandleEnemyDeath (int sender)
    {
        // Updates the enemy list
        int senderIndex = _spawnList.FindIndex(t => t.id == sender);
        _spawnList[senderIndex].isAlive = false;
    }

    public void SetKilledList (bool[] arr)
    {
        // If the array is null then do nothing
        if (arr == null)
            return;

        for (int i = 0; i < arr.Length && i < _spawnList.Count; ++i)
        {
            if (!arr[i])
            {
                _spawnList[i].isAlive = false;       
                Destroy(_spawnList[i].status.gameObject);
            }
        }
    }

    public bool[] GetKilledList ()
    {
        List<bool> killedList = new List<bool>();
        foreach (EnemySpawnData enemy in _spawnList)
        {
            killedList.Add(enemy.isAlive);
        }
        return killedList.ToArray();
    }

    [ContextMenu("Autofill Enemies")]
    void AutofillEnemies ()
    {
        _spawnList.Clear();
        EnemyStatus[] enemyStatuses = GetComponentsInChildren<EnemyStatus>();
        foreach (EnemyStatus enemy in enemyStatuses)
        {
            EnemySpawnData spawnData = new EnemySpawnData(enemy);
            
            spawnData.id = _spawnList.Count;
            _spawnList.Add(spawnData);
        }
    }
}
