using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : StatusController
{
    private Animator anim;

    [SerializeField] private float hurtThreshold = 0.25f;

    private new void Awake ()
    {
        base.Awake();

        anim = GetComponentInChildren<Animator>();
    }

    public override void Die ()
    {
        // Enemy death
        isDying = true;
        anim.Play("Death");
        GetComponent<EnemyController>().SpawnLoot();
        Destroy(gameObject, 10f);
    }

    public new void TakeDamage (float value)
    {
        base.TakeDamage(value);

        if (value >= MaxHealth * hurtThreshold)
        {
            PlayDamageAnimation();
        }
    }

    private void PlayDamageAnimation ()
    {
        anim.Play("Hit");
    }
}
