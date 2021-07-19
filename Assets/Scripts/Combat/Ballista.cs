using System.Collections;
using System.Collections.Generic;
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
    
    private float ballistaProgress = 0f;
    
    private bool isBallistaLoading = false;
    private bool isBallistaLoaded = false;

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
        
        float percentage = Mathf.Clamp01(ballistaProgress / ballistaLoadingTime) * 100f;
            
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
        if (bossFight.GetStage() > 0 && !bossFight.IsBossDefeated)
        {
            if (noUses++ == 0)
                StartBallistaUsage();
            
            if (isBallistaLoaded)
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
        UserInterfaceController.instance.ShowProgressMenu("Carregamento da Balista");
        UserInterfaceController.instance.SetProgressIcon(ballistaIcon);
        UserInterfaceController.instance.UpdateProgressBar(0f);
    }

    private void Fire ()
    {
        if (target == null || ballistaProgress < 1f)
            return;
        
        // Reset the blood bar
        ballistaProgress = 0f;
        isBallistaLoaded = false;
        
        GameObject harpoon = Instantiate(ballistaProjectile, firePoint.position, Quaternion.identity);
        harpoon.GetComponent<ProjectileController>().SetTarget(firePoint2.position - firePoint.position);
        harpoon.GetComponent<ProjectileController>().FaceTowards(firePoint.position, firePoint2.position);
        
        // Play cannon sound
        AudioManager.instance.PlaySound("Canhao");
        
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
