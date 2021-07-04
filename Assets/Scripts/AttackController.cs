using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AttackController : MonoBehaviour
{
    private Animator anim;

    private StatusController status;

    [SerializeField] private Transform attackPoint;

    [SerializeField] private Transform firePointA;
    [SerializeField] private Transform firePointB;

    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private float attackSpeed = 1f;

    [SerializeField] private LayerMask damageableLayer;

    [SerializeField] private float attackDamage = 50f;
    [SerializeField] private float currentAttackDamage;

    [SerializeField] private PlayerEquipment equipment;

    [SerializeField] private GameObject projectile;

    private bool hasBerserk;

    [SerializeField] private bool canAttack = true;

    private int animationVariation = 0;
    private int animationLimit = 2;

    public float Damage
    {
        get { return attackDamage; }
        set 
        { 
            attackDamage = value;
            currentAttackDamage = value;
        }
    }

    public float TemporaryDamage
    {
        get { return currentAttackDamage; }
        set
        {
            currentAttackDamage = value;
        }
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

    public Transform AttackPoint
    {
        get { return attackPoint; }
    }

    public Transform FirePointA
    {
        get { return firePointA; }
        set { firePointA = value; }
    }

    public Transform FirePointB
    {
        get { return firePointB; }
        set { firePointB = value; }
    }

    private void Awake ()
    {
        anim = GetComponent<Animator>();
        status = GetComponent<StatusController>();
        TryGetComponent(out equipment);

        anim.SetFloat("attackSpeed", attackSpeed);
    }

    private void Start ()
    {
        if (gameObject.CompareTag("Player"))
        {
            InputHandler.instance.OnAttackUnleashed += HandleAttack;
            InputHandler.instance.OnPowerAttackUnleashed += HandlePowerAttack;
            InputHandler.instance.OnRangedAttackUnleashed += HandleRangedAttack;
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

    private void HandleRangedAttack (object sender, EventArgs e)
    {
        PerformRangedAttack();        
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

    private void PerformRangedAttack ()
    {
        if (!status.IsBlocking && canAttack && equipment.RangedWeapon != null)
        {
            anim.Play("weapon_switch_to_ranged");
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

    public void FireRangedAttack ()
    {
        // Criar projétil
        CreateProjectile();
    }

    // Usually used by the player
    public void CreateProjectile ()
    {
        GameObject attackProjectile;
        if (equipment != null)
        {
            Vector3 point = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.farClipPlane));
            attackProjectile = Instantiate(equipment.RangedWeapon.projectile, firePointB.position, Quaternion.identity);
            attackProjectile.GetComponent<ProjectileController>().SetTarget(point);
            //Debug.DrawRay(firePointB.position, point, Color.magenta, 10f);
        }
        else
        {
            Debug.LogWarning("Ranged weapon not equipped!");
        }
    }
    
    // Usually used by enemies
    public void CreateProjectile (Transform target)
    {
        GameObject attackProjectile;
        Vector3 point = target.position - attackPoint.position;
        attackProjectile = Instantiate(projectile, attackPoint.position, Quaternion.identity);
        attackProjectile.GetComponent<ProjectileController>().SetTarget(point);
        attackProjectile.GetComponent<ProjectileController>().FaceTowards(AttackPoint.position, point);
    }


    public void HitscanAttack (bool isPowerAttack, string type)
    {
        Collider[] hitObjects = Physics.OverlapSphere(attackPoint.position, attackRange, damageableLayer);

        // Add all the game objects that were hit and
        // remove duplicates - in case some object has
        // more than one collider, it will count as
        // n hits where n is the number of colliders
        // from this object the histcan has touched
        List<GameObject> hitEntities = new List<GameObject>();

        foreach (Collider hit in hitObjects)
        {
            hitEntities.Add(hit.gameObject);
        }

        hitEntities = hitEntities.Distinct().ToList();

        // Apply damage to the entities hit
        foreach (GameObject hitEntity in hitEntities)
        {
            IDamageable damagedObj;

            //Debug.Log(hitEntity.gameObject.name);

            if (hitEntity.TryGetComponent<IDamageable>(out damagedObj))
            {
                float finalAttackDamage = attackDamage;

                if (isPowerAttack)
                    finalAttackDamage *= 2f;

                if (type != "")
                {
                    damagedObj.TakeDamage(finalAttackDamage);
                }
                else
                {
                    damagedObj.TakeElementalDamage(finalAttackDamage, type);
                }

                damagedObj.PlayImpactSound();
            }
        }
    }

    private void OnDestroy ()
    {
        InputHandler.instance.OnAttackUnleashed -= HandleAttack;
        InputHandler.instance.OnPowerAttackUnleashed -= HandlePowerAttack;
        InputHandler.instance.OnRangedAttackUnleashed -= HandleRangedAttack;
    }

    private void OnDrawGizmosSelected ()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
