using UnityEngine;

[System.Serializable]
public class SpawnData
{
    public int id;

    public bool isActive;

    public IManageable manager;

    // Redundant given the IManageable interface
    // Used only for Debugging purposes
    public GameObject gameObject;

    public SpawnData (IManageable manager)
    {
        this.gameObject = manager.AttachedObject;
        isActive = true;
    }

    public void SetInactive ()
    {
        isActive = false;
    }

    public void DestroyObject ()
    {
        manager.DestroyObject();
    }

}