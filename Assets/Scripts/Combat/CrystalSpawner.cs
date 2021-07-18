using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class CrystalSpawner : MonoBehaviour
{
    [SerializeField] private GameObject crystalPrefab;

    [SerializeField] private float spawnMinTime, spawnMaxTime;
    
    [SerializeField] private Transform crystalSpawnLimiterMin;
    [SerializeField] private Transform crystalSpawnLimiterMax;

    private bool canSpawnCrystal = false;
    
    private bool isOnCooldown = false;

    public bool CanSpawnCrystal
    {
        get => canSpawnCrystal;
        set => canSpawnCrystal = value;
    }

    private void Update ()
    {
        if (!canSpawnCrystal)
            return;

        if (isOnCooldown)
            return;
        
        SpawnCrystal();
    }
    
    private void SpawnCrystal ()
    {
        isOnCooldown = true;

        float spawnX = UnityEngine.Random.Range(crystalSpawnLimiterMin.position.x, crystalSpawnLimiterMin.position.z);
        float spawnY = crystalSpawnLimiterMax.position.y;
        float spawnZ = UnityEngine.Random.Range(crystalSpawnLimiterMax.position.x, crystalSpawnLimiterMax.position.z);

        Vector3 spawnPos = new Vector3(spawnX, spawnY, spawnZ);

        Instantiate(crystalPrefab, spawnPos, Quaternion.identity, gameObject.transform);

        float nextSpawn = UnityEngine.Random.Range(spawnMinTime, spawnMaxTime);
        
        Invoke(nameof(ResetCooldown), nextSpawn);
    }

    private void ResetCooldown ()
    {
        isOnCooldown = false;
    }
}
