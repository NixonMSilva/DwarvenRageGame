using UnityEngine;

public class EnemySpawnManager : SpawnManager
{
    [ContextMenu("Autofill Enemies")]
    private void AutofillEnemies ()
    {
        _spawnList.Clear();
        EnemyStatus[] enemyStatuses = GetComponentsInChildren<EnemyStatus>();
        foreach (EnemyStatus enemy in enemyStatuses)
        {
            SpawnData spawnData = new SpawnData(enemy);
            spawnData.id = _spawnList.Count;
            _spawnList.Add(spawnData);
        }
    }
}
