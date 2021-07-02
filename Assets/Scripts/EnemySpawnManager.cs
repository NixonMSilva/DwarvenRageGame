using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] private List<EnemyStatus> _enemyList;

    [SerializeField] private List<bool> _wasNotKilled;

    private char[] limits = { '(', ')' };

    private void Awake ()
    {
        for (int i = 0; i < _enemyList.Count; ++i)
        {
            _wasNotKilled.Add(true);
        }

        foreach (EnemyStatus enemy in _enemyList)
        {
            enemy.OnDeath += HandleEnemyDeath;
        }
    }

    private void HandleEnemyDeath (EnemyStatus sender)
    {
        string name = sender.gameObject.name;
        string[] subs = name.Split(limits);
        int enemyId = Int32.Parse(subs[1]);
        _wasNotKilled[enemyId] = false;
    }

    public void SetKilledList (bool[] arr)
    {
        // If the array is null then do nothing
        if (arr == null)
            return;

        _wasNotKilled = arr.ToList();
        for (int i = 0; i < _wasNotKilled.Count; ++i)
        {
            if (!_wasNotKilled[i])
            {
                Destroy(_enemyList[i].gameObject);
            }
        }
    }

    public bool[] GetKilledList ()
    {
        return _wasNotKilled.ToArray();
    }

    [ContextMenu("Autofill Enemies")]
    void AutofillEnemies ()
    {
        _enemyList = GetComponentsInChildren<EnemyStatus>().ToList();
    }
}
