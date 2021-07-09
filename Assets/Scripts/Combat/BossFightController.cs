using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossFightController : MonoBehaviour
{
    [SerializeField] protected GameObject bossObject;

    [SerializeField] protected UnityEvent OnBossDeath;

    protected BossAI bossAI;

    protected EnemyStatus bossStatus;

    protected bool isBossDefeated = false;

    public bool IsBossDefeated
    {
        get => isBossDefeated;
    }

    protected void Awake ()
    {
        bossAI = bossObject.GetComponent<BossAI>();
        bossStatus = bossObject.GetComponent<EnemyStatus>();
    }

    protected void Start ()
    {
        bossAI.onFightStageChange += HandleStageChange;
        bossStatus.OnDeath += HandleBossDeath;    
    }

    protected void OnDestroy ()
    {
        bossAI.onFightStageChange -= HandleStageChange;
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
        // Only run if the fight stage is not activated (-1)
        if ((other.CompareTag("Player") || other.CompareTag("Projectile")) && bossAI.FightStage == -1)
        {
            StartBattle();
        }
    }

    private void StartBattle ()
    {
        // Initializes boss battle
        bossAI.FightStage = 0;
    }
    
    public virtual void HandleStageChange(int stage)
    {
        Debug.Log("Parent stage change");
    }

    public int GetStage() => bossAI.FightStage;
}
