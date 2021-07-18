using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballista : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform target;

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
            
        UserInterfaceController.instance.UpdateProgressBar(percentage);
        
        if (ballistaProgress >= ballistaLoadingTime)
        {
            ballistaProgress = ballistaLoadingTime;
            isBallistaLoading = false;
            isBallistaLoaded = true;
            ballistaTooltip.SetTooltipText("Atirar");
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
        ballistaTooltip.SetTooltipText("Carregar");
        UserInterfaceController.instance.ShowProgressMenu("Carregamento da Ballista");
        UserInterfaceController.instance.SetProgressIcon(ballistaIcon);
        UserInterfaceController.instance.UpdateProgressBar(0f);
    }

    private void Fire ()
    {
        if (target == null)
            return;
        
        GameObject harpoon = Instantiate(ballistaProjectile, firePoint.position, Quaternion.identity);
        harpoon.GetComponent<ProjectileController>().SetTarget(target.position - transform.position);
        
        // Play cannon sound
        AudioManager.instance.PlaySound("Canhao");

        // Reset the blood bar
        ballistaProgress = 0f;
        isBallistaLoaded = false;
        
        UserInterfaceController.instance.UpdateProgressBar(0f);
        ballistaTooltip.SetTooltipText("Carregar");
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
