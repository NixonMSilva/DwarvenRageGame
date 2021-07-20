using UnityEngine;

public class Boss2FightController : BossFightController
{
    [SerializeField] private EnemyStatus bossHealth;

    [SerializeField] private CrystalSpawner crystalSpawner;

    private new void Awake ()
    {
        base.Awake();
    }

    public override void HandleStageChange (int stage)
    {
        switch (stage)
        {
            default:
                break;
            case 1:
                DisableCrystalSpawning();
                break;
            case 2:
                // Trigger some audio
                // Do something
                DisableCrystalSpawning();
                break;
            case 3:
                // Trigger some audio
                // Do something
                DisableCrystalSpawning();
                break;
            case 4:
                // Dragon fly
                EnableCrystalSpawning();
                break;
            case 5:
                EnableCrystalSpawning();
                break;
        }
    }

    public void EnableCrystalSpawning ()
    {
        if (crystalSpawner != null)
        {
            //Debug.Log("Crystal spawner active");
            crystalSpawner.CanSpawnCrystal = true;
        }
    }
    
    public void DisableCrystalSpawning ()
    {
        if (crystalSpawner != null)
        {
            //Debug.Log("Crystal spawner deactivated");
            crystalSpawner.CanSpawnCrystal = false;
        }
    }
    
}
