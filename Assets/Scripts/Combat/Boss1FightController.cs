using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1FightController : BossFightController
{
    [SerializeField] private float bloodBarProgress;

    [SerializeField] private float percentagePerKill = 15f;
    
    private PigSpawner spawner;

    public float BloodBar
    {
        get => bloodBarProgress;
        set => bloodBarProgress = value;
    }

    private new void Awake ()
    {
        base.Awake();

        spawner = GetComponent<PigSpawner>();
    }

    private new void Start ()
    {
        base.Start();
    }

    private new void OnDestroy ()
    {
        base.OnDestroy();
    }
    
    public override void HandleStageChange(int stage)
    {
        switch (stage)
        {
            case 1:
                // Starting spawning pigs
                spawner.CanSpawn = true;
                break;
            case 3:
                // Disable the spawner
                spawner.enabled = false;
                break;
            default:
                break;
        }
    }
    
    public void FillProgressBar (float delta)
    {
        bloodBarProgress += delta;
        UserInterfaceController.instance.UpdateProgressBar(bloodBarProgress);
    }
}
