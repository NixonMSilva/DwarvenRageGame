using UnityEngine;

public class Ballista : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform target;
    [SerializeField] private Transform firePoint2;

    [SerializeField] private GameObject ballistaProjectile;
    
    [SerializeField] private Boss2FightController bossFight;

    [SerializeField] private Sprite ballistaIcon;
    
    [SerializeField] private float ballistaLoadingTime;
    
    [SerializeField] private float ballistaProgress = 0f;
    
    [SerializeField] private bool isBallistaLoading = false;
    [SerializeField] private bool isBallistaLoaded = false;

    [SerializeField] private float percentage;
    
    private Rotater rotationEvent;
    private TooltipController ballistaTooltip;

    private int noUses = 0;

    private void Awake ()
    {
        ballistaTooltip = GetComponent<TooltipController>();
        rotationEvent = GetComponent<Rotater>();
    }
    
    private void Update ()
    {
        if (isBallistaLoading)
            UpdateBallistaLoad();
    }

    private void UpdateBallistaLoad ()
    {
        ballistaProgress += Time.deltaTime;
        
        percentage = Mathf.Clamp01(ballistaProgress / ballistaLoadingTime) * 100f;

        UserInterfaceController.instance.UpdateProgressBar(Mathf.Round(percentage));
        
        if (ballistaProgress >= ballistaLoadingTime)
        {
            ballistaProgress = ballistaLoadingTime;
            isBallistaLoading = false;
            isBallistaLoaded = true;
        }
    }

    public void BallistaInteraction ()
    {
        // Only interact if the boss battle has started or if he isn't defeated
        if (bossFight.GetStage() > 0 && !bossFight.IsBossDefeated && (percentage == 0f || percentage == 100f))
        {
            if (noUses++ == 0)
                StartBallistaUsage();
            
            if (percentage >= 100f)
            {
                LookAtTarget();
            }
            else if (!isBallistaLoading)
            {
                isBallistaLoading = true;
            }
        }

    }

    private void StartBallistaUsage ()
    {
        UserInterfaceController.instance.ShowProgressMenu("Ballista Loading");
        UserInterfaceController.instance.SetProgressIcon(ballistaIcon);
        UserInterfaceController.instance.UpdateProgressBar(0f);
    }

    private void Fire ()
    {
        if (target == null || ballistaProgress < 1f)
            return;

        GameObject harpoon = Instantiate(ballistaProjectile, firePoint.position, transform.rotation);
        harpoon.GetComponent<ProjectileController>().SetTarget(firePoint2.position - firePoint.position);
        harpoon.GetComponent<ProjectileController>().FaceTowards(firePoint.position, firePoint2.position);
        
        // Play cannon sound
        AudioManager.instance.PlaySound("Canhao");

        // Reset the loading bar
        ballistaProgress = 0f;
        percentage = 0f;
        isBallistaLoaded = false;
        UserInterfaceController.instance.UpdateProgressBar(0f);
    }

    private void LookAtTarget ()
    {
        if (target == null)
            return;

        Quaternion lookRotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
        lookRotation *= Quaternion.Euler(0f, 180f, 0f);
        rotationEvent.MoveTo (lookRotation, true, Fire);
        
    }

    public void DisableBallista ()
    {
        Destroy (ballistaTooltip);
    }
}
