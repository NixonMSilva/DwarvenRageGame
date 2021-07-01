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
    [SerializeField] private float attackSpeed = 1f;

    [SerializeField] private LayerMask damageableLayer;

    [SerializeField] private float attackDamage = 50f;

    private bool hasBerserk;

    [SerializeField] private bool canAttack = true;

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
    
    public bool CanAttack
    {
        get { return canAttack; }
        set { canAttack = value; }
    }

    private void Awake ()
    {
        anim = GetComponent<Animator>();
        status = GetComponent<StatusController>();

        anim.SetFloat("attackSpeed", attackSpeed);
    }

    private void Start ()
    {
        if (gameObject.CompareTag("Player"))
        {
            InputHandler.instance.OnAttackUnleashed += HandleAttack;
            InputHandler.instance.OnPowerAttackUnleashed += HandlePowerAttack;
        }
    }

    private void Update ()
    {
        anim.SetFloat("attackSpeed", attackSpeed);
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
        if (!status.IsBlocking && canAttack)
        {
            anim.Play("attack");
            canAttack = false;
        }
    }

    private void PerformPowerAttack ()
    {
        if (!status.IsBlocking && canAttack)
        {
            anim.Play("power_attack");
            canAttack = false;
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