using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingCannon : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform target;

    [SerializeField] private GameObject cannonProjectile;

    private TooltipController cannonTooltip;

    private int interactionCount = 0;

    private void Awake ()
    {
        cannonTooltip = GetComponent<TooltipController>();
    }

    private void Update ()
    {
        LookAtTarget();
    }

    public void Fire ()
    {
        GameObject skull = Instantiate(cannonProjectile, firePoint.position, Quaternion.identity);
        skull.GetComponent<ProjectileController>().SetTarget(target.position - transform.position);
    }

    private void LookAtTarget ()
    {
        Debug.Log("Looking!");
        Quaternion lookRotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
        lookRotation *= Quaternion.Euler(0f, 180f, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 0.2f);
        //transform.rotation = lookRotation;
    }

    public void Interaction ()
    {
        switch (interactionCount)
        {
            case 0:
                break;
            case 1:
                break;
        }
        interactionCount++;
    }
}
