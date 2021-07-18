using UnityEngine;

public class Boss2FightController : BossFightController
{
    [SerializeField] private float ballistaLoadingTime;
    
    private float ballistaProgress = 0f;
    
    private bool isBallistaLoading = false;
    private bool isBallistaLoaded = false;
    
    private Resistance originalResistance;
    [SerializeField] private Resistance invulnerabiltyResistance;
    
    [SerializeField] private SkinnedMeshRenderer dragonBodyMesh;
    
    [SerializeField] private EnemyStatus bossHealth;

    public bool BallistaLoaded
    {
        get => isBallistaLoaded;
        set => isBallistaLoaded = value;
    }
    
    private new void Awake ()
    {
        base.Awake();
        
        originalResistance = bossStatus.Sheet;
    }

    private void Update ()
    {
        if (isBallistaLoading)
            UpdateBallistaLoad();
    }

    private void UpdateBallistaLoad ()
    {
        ballistaProgress += Time.deltaTime;
        if (ballistaProgress >= ballistaLoadingTime)
        {
            ballistaProgress = ballistaLoadingTime;
            isBallistaLoading = false;
            isBallistaLoaded = true;
        }
    }
    public override void HandleStageChange (int stage)
    {
        switch (stage)
        {
            default:
                break;
            case 1:
                // Trigger some audio
                // Do something
                break;
            case 2:
                // Trigger some audio
                // Do something
                break;
            case 3:
                // Trigger some audio
                // Do something
                break;
            case 4:
                // Dragon fly
                // Crystal 
                break;
        }
    }
    
}
