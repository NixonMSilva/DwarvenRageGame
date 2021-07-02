using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : StatusController
{
    private Animator anim;

    [SerializeField] private float hurtThreshold = 0.25f;

    private void Awake ()
    {
        Health = maxHealth;
        Armor = 0f;

        attack = GetComponent<AttackController>();
        animator = GetComponent<Animator>();
    }

    public override void Die ()
    {
        // Enemy death
        isDying = true;
        animator.Play("Death");
        GetComponent<EnemyController>().SpawnLoot();
        Destroy(gameObject, 10f);
    }

    public override void TakeDamage (float value)
    {
        base.TakeDamage(value);
        //Debug.Log(MaxHealth * hurtThreshold);
        if (value >= MaxHealth * hurtThreshold)
        {
            PlayDamageAnimation();
        }
    }

    private void PlayDamageAnimation ()
    {
        animator.Play("Hit");
    }
}
