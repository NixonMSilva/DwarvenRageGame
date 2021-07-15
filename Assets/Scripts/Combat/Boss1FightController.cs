using System;
using UnityEngine;

public class Boss1FightController : BossFightController
{
    [SerializeField] private float bloodBarProgress;

    [SerializeField] private float percentagePerKill = 15f;

    private Resistance originalResistance;
    [SerializeField] private Resistance invulnerabiltyResistance;
    
    [SerializeField] private Material[] bossMaterials;

    [SerializeField] private SkinnedMeshRenderer trollBodyMesh;

    [SerializeField] private EnemyStatus bossHealth;
    
    [SerializeField] private GameObject platform;

    private PigSpawner spawner;

    public float BloodBar
    {
        get => bloodBarProgress;
        set
        {
            bloodBarProgress = value;

            if (bloodBarProgress >= 100f)
                bloodBarProgress = 100f;
        }
    }

    private new void Awake ()
    {
        base.Awake();

        spawner = GetComponent<PigSpawner>();

        originalResistance = bossStatus.Sheet;
    }

    private void Update ()
    {
        UserInterfaceController.instance.UpdateBossBar(bossHealth.Health / bossHealth.MaxHealth);
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
                break;
            case 2:
                // Second phase (red)
                trollBodyMesh.material = bossMaterials[1];
                ChangeResistance(invulnerabiltyResistance);
                break;
            case 3:
                // Final phase (purple)
                trollBodyMesh.material = bossMaterials[2];
                ChangeResistance(invulnerabiltyResistance);
                AudioManager.instance.PlaySoundAt(bossObject, "troll_final_phase");
                break;
        }
    }
    
    public void FillProgressBar ()
    {
        BloodBar += percentagePerKill;
        UserInterfaceController.instance.UpdateProgressBar(bloodBarProgress);
    }

    public void DisableSpawner()
    {
        spawner.enabled = false;
    }
    
    private void ChangeResistance (Resistance newResistance)
    {
        bossStatus.Resistances = newResistance.BuildSheet();
    }

    public void ResetResistance()
    {
        bossStatus.Resistances = originalResistance.BuildSheet();
    }

    public void ResetColor()
    {
        trollBodyMesh.material = bossMaterials[0];
    }
}
