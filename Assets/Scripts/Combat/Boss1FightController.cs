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
            default:
                break;
            case 1:
                // Starting spawning pigs
                spawner.CanSpawn = true;
                AudioManager.instance.PlaySoundAt(bossObject, "troll_line_1");
                break;
            case 2:
                AudioManager.instance.PlaySoundAt(bossObject, "troll_line_2");
                break;
            case 3:
                AudioManager.instance.PlaySoundAt(bossObject, "troll_line_3");
                break;
        }
    }
    
    public void FillProgressBar ()
    {
        bloodBarProgress += percentagePerKill;
        UserInterfaceController.instance.UpdateProgressBar(bloodBarProgress);
    }

    public void DisableSpawner()
    {
        spawner.enabled = false;
    }
}
