using UnityEngine;

public class EventSpawnManager : SpawnManager
{
    [ContextMenu("Autofill Events")]
    private void AutofillEnemies ()
    {
        _spawnList.Clear();
        EventObject[] eventStatuses = GetComponentsInChildren<EventObject>();
        foreach (EventObject singeEvent in eventStatuses)
        {
            SpawnData spawnData = new SpawnData(singeEvent);
            spawnData.id = _spawnList.Count;
            _spawnList.Add(spawnData);
        }
    }
}
