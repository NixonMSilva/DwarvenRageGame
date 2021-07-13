using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingCannon : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform target;

    [SerializeField] private GameObject cannonProjectile;
    
    [SerializeField] private Boss1FightController bossFight;

    private Rotater rotationEvent;
    private TooltipController cannonTooltip;

    private int interactionCount = 0;

    private void Awake ()
    {
        cannonTooltip = GetComponent<TooltipController>();
        rotationEvent = GetComponent<Rotater>();
    }

    public void CannonInteraction ()
    {
        // Only interact if the boss battle has started or if he isn't defeated
        if (bossFight.GetStage() > 0 && !bossFight.IsBossDefeated)
        {
            if (interactionCount == 0)
            {
                StartCannonUsage();
            }
            else
            {
                if (bossFight.BloodBar >= 100f)
                {
                    LookAtTarget();
                }
                else
                {
                    BloodNotFull();
                }
            }

            interactionCount++;
        }

    }

    private void StartCannonUsage()
    {
        cannonTooltip.SetTooltipText("Atirar");
        UserInterfaceController.instance.ShowProgressMenu("Blood Collected");
        UserInterfaceController.instance.UpdateProgressBar(bossFight.BloodBar);
    }

    private void BloodNotFull()
    {
        Debug.Log("Blood bar is not full!");
    }

    private void Fire ()
    {
        if (target == null)
            return;
        
        GameObject skull = Instantiate(cannonProjectile, firePoint.position, Quaternion.identity);
        skull.GetComponent<ProjectileController>().SetTarget(target.position - transform.position);
        
        // Play cannon sound
        AudioManager.instance.PlaySound("Canhao");

        // Reset the blood bar
        bossFight.BloodBar = 0f;
        UserInterfaceController.instance.UpdateProgressBar(0f);
    }

    private void LookAtTarget ()
    {
        if (target == null)
            return;

        Quaternion lookRotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
        lookRotation *= Quaternion.Euler(0f, 180f, 0f);
        rotationEvent.MoveTo(lookRotation, true, Fire);
       
        
        //transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 0.2f);
        //transform.rotation = lookRotation;
    }

    public void DisableCannon()
    {
        Destroy(cannonTooltip);
    }
}
