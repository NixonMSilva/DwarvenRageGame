using UnityEngine;

public class PickableSpawnManager : SpawnManager
{
    [ContextMenu("Autofill Pickables")]
    private void AutofillPickables ()
    {
        _spawnList.Clear();
        PickableBase[] pickableStatuses = GetComponentsInChildren<PickableBase>();
        foreach (PickableBase pickable in pickableStatuses)
        {
            SpawnData spawnData = new SpawnData(pickable);
            spawnData.id = _spawnList.Count;
            _spawnList.Add(spawnData);
        }
    }
}
