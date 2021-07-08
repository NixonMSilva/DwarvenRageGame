using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossFightController : MonoBehaviour
{
    [SerializeField] private GameObject bossObject;

    [SerializeField] private UnityEvent OnBossDeath;

    private BossAI bossAI;

    private EnemyStatus bossStatus;

    private bool isBossDefeated = false;

    private void Awake ()
    {
        bossAI = bossObject.GetComponent<BossAI>();
        bossStatus = bossObject.GetComponent<EnemyStatus>();
    }

    private void Start ()
    {
        bossStatus.OnDeath += HandleBossDeath;    
    }

    private void OnDestroy ()
    {
        bossStatus.OnDeath -= HandleBossDeath;
    }

    private void HandleBossDeath (EnemyStatus obj)
    {
        isBossDefeated = true; 
        OnBossDeath.Invoke();
    }

    // Triggers the battle when the player enter the boss volume
    public void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartBattle();
        }
    }

    private void StartBattle ()
    {
        // Initializes boss battle
        bossAI.FightStage = 0;
    }
}
