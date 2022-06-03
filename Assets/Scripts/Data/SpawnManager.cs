using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] protected List<SpawnData> _spawnList = new List<SpawnData>();

    private void Awake ()
    {
        for (int i = 0; i < _spawnList.Count; ++i)
        {
            _spawnList[i].isActive = true;
            _spawnList[i].manager = _spawnList[i].gameObject?.GetComponent<IManageable>();
            _spawnList[i].manager.OnStatusChange += HandleSpawnStatusChange;
            _spawnList[i].manager.UniqueId = i;
        }
    }

    private void OnDisable ()
    {
        for (int i = 0; i < _spawnList.Count; ++i)
        {
            _spawnList[i].manager.OnStatusChange -= HandleSpawnStatusChange;
        }
    }

    /// <summary>
    /// Handles the status change (on death of enemies / pickable picked / event triggered) 
    /// and deactivate the corresponding spawn data on the list.
    /// </summary>
    /// <param name="id">Unique ID attached to the enemy being deactivated.</param>
    public void HandleSpawnStatusChange (int id)
    {
        int senderIndex = _spawnList.FindIndex(t => t.id == id);
        _spawnList[senderIndex].isActive = false;
    }

    /// <summary>
    /// Gets the active list of enemies / pickables / events in order to save data via the SaveManager 
    /// class. </summary>
    /// <returns>A boolean array containin the active status of the objects.</returns>
    public bool[] GetActiveList ()
    {
        List<bool> activeList = new List<bool>();
        foreach (SpawnData pickable in _spawnList)
        {
            activeList.Add(pickable.isActive);
        }
        return activeList.ToArray();
    }

    /// <summary>
    /// Sets the active list of enemies/pickables/events from the game load via the SaveManager class, 
    /// destroying or deactivating objects that are not active.
    /// </summary>
    /// <param name="list">A boolean array containing the active status of the objects as per save game 
    /// status.</param>
    public void SetActiveList (bool[] list)
    {
        if (list == null)
            return;

        for (int i = 0; i < list.Length; ++i)
        {
            if (!list[i])
            {
                _spawnList[i].isActive = false;
                _spawnList[i].manager.DestroyObject();
            }
        }
    }
}
