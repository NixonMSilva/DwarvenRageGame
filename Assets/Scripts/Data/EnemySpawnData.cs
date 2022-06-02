using System.Collections;
using UnityEngine;

[System.Serializable]
public class EnemySpawnData : SpawnData
{
    public EnemyStatus status;
    public bool isAlive;

    public EnemySpawnData (EnemyStatus status)
    {
        this.status = status;
        isAlive = true;
    }

    public void SetDead ()
    {
        isAlive = false;
    }
}