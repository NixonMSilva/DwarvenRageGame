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

    private bool isPlayer = false;

    private bool hasBerserk;

    [SerializeField] private bool canAttack = true;

    [SerializeField] private bool canAttackRanged = true;

    private float rangedCooldown;

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

    public float RangedCooldown
    {
        get { return rangedCooldown; }
        set { rangedCooldown = value; }
    }

    private void Awake ()
    {
        if (gameObject.CompareTag("Player"))
            isPlayer = true;

        anim = GetComponent<Animator>();
        status = GetComponent<StatusController>();

        if (isPlayer)
            equipment = GetComponent<PlayerEquipment>();

        anim.SetFloat("attackSpeed", attackSpeed);
    }

    private void Start ()
    {
        if (isPlayer)
        {
            InputHandler.instance.OnAttackUnleashed += HandleAttack;
            InputHandler.instance.OnPowerAttackUnleashed += HandlePowerAttack;
            InputHandler.instance.OnRangedAttackUnleashed += HandleRangedAttack;
        }
    }

    private void Update ()
    {
        //anim.SetFloat("attackSpeed", attackSpeed);
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
        if (!status.IsBlocking && canAttack && canAttackRanged && equipment.PlayerRanged != null)
        {
            anim.Play("weapon_switch_to_ranged");
            canAttack = false;
            canAttackRanged = false;
            CooldownController cooldown = gameObject.AddComponent<CooldownController>();
            Debug.Log("Teste: " + rangedCooldown);
            cooldown.SetTimer(rangedCooldown,
            () =>
            {
                // On Finish
                canAttackRanged = true;
                Destroy(cooldown);
            },
            () =>
            {
                // On Update
                UserInterfaceController.instance.UpdateRangedCooldownSlider(cooldown.TimeElapsed());   
            });
        }
    }

    public void ConnectAttack ()
    {
        if (isPlayer)
            RegisterAttack(equipment.PlayerWeapon.damage);
        else
            RegisterAttack(Damage);
    }

    public void ConnectPowerAttack ()
    {
        if (isPlayer)
            RegisterAttack(equipment.PlayerWeapon.damage * 2f);
        else
            RegisterAttack(Damage * 2f);
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
        if (isPlayer)
        {
            Vector3 point = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.farClipPlane));
            attackProjectile = Instantiate(equipment.PlayerRanged.projectile, firePointB.position, Quaternion.identity);
            attackProjectile.GetComponent<ProjectileController>().SetTarget(point - firePointB.position);
            Debug.DrawLine(firePointB.position, point, Color.green, 10f);
            //Debug.DrawRay(firePointB.position, point, Color.magenta, 10f);
        }
        else
        {
            Debug.LogWarning("Not the player!");
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

    public void RegisterAttack (float damage)
    {
        Collider[] hitObjects = Physics.OverlapSphere(attackPoint.position, attackRange, damageableLayer);

        // Add all the game objects that were hit and
        // remove duplicates - in case some object has
        // more than one collider, it will count as
        // n hits where n is the number of colliders
        // from this object the histcan has touched
        List<GameObject> hitEntities = new List<GameObject>();

        foreach (Collider hit in hitObjects)
            hitEntities.Add(hit.gameObject);

        hitEntities = hitEntities.Distinct().ToList();

        // Apply damage to the entities hit
        foreach (GameObject hitEntity in hitEntities)
        {
            if (hitEntity.TryGetComponent(out IDamageable damagedObj))
            {
                if (isPlayer)
                {
                    float damageModifier = equipment.PlayerWeapon.AttackEffect(status, damagedObj);
                    damagedObj.TakeDamage(damage * damageModifier, equipment.PlayerWeapon.damageType);
                }
                else
                    damagedObj.TakeDamage(damage, GetComponent<EnemyController>().Type.damageType);

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
