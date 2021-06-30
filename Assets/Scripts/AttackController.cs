using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    private Animator anim;

    private StatusController status;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;

    [SerializeField] private LayerMask damageableLayer;

    private float attackDamage = 50f;

    private bool hasBerserk;

    private int animationVariation = 0;
    private int animationLimit = 2;

    public float Damage
    {
        get { return attackDamage; }
        set { attackDamage = value; }
    }

    public bool Berserk
    {
        get { return hasBerserk; }
        set { hasBerserk = value; }
    }

    private void Awake ()
    {
        anim = GetComponent<Animator>();
        status = GetComponent<StatusController>();
    }

    private void Start ()
    {
        if (gameObject.CompareTag("Player"))
        {
            InputHandler.instance.OnAttackUnleashed += HandleAttack;
            InputHandler.instance.OnPowerAttackUnleashed += HandlePowerAttack;
        }
    }

    private void HandleAttack (object sender, EventArgs e)
    {
        PerformAttack();
    }

    private void HandlePowerAttack (object sender, EventArgs e)
    {
        PerformPowerAttack();        
    }

    private void PerformAttack ()
    {
        if (!status.IsBlocking)
        {
            anim.SetBool("isAttacking", true);
            animationVariation += 1;
            if (animationVariation > animationLimit)
            {
                animationVariation = 0;
            }
            anim.SetInteger("variation", animationVariation);

        }
    }

    private void PerformPowerAttack ()
    {
        if (!status.IsBlocking)
        {
            anim.SetBool("isPowerAttacking", true);
        }
    }

    public void ConnectAttack ()
    {
        HitscanAttack(false, "");
    }

    public void ConnectPowerAttack ()
    {
        HitscanAttack (true, "");
    }

    public void HitscanAttack (bool isPowerAttack, string type)
    {
        Collider[] hitObjects = Physics.OverlapSphere(attackPoint.position, attackRange, damageableLayer);

        foreach (Collider obj in hitObjects)
        {
            IDamageable damagedObj;
            if (obj.TryGetComponent<IDamageable>(out damagedObj))
            {
                float finalAttackDamage = attackDamage;
                if (hasBerserk)
                    finalAttackDamage *= 2;

                if (isPowerAttack)
                    finalAttackDamage *= 2;

                if (type != "")
                {
                    damagedObj.TakeDamage(finalAttackDamage);
                }
                else
                {
                    damagedObj.TakeElementalDamage(finalAttackDamage, type);
                }
               
            }
        }
    }

    private void OnDestroy ()
    {
        InputHandler.instance.OnAttackUnleashed -= HandleAttack;
        InputHandler.instance.OnPowerAttackUnleashed -= HandlePowerAttack;
    }

    private void OnDrawGizmosSelected ()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
