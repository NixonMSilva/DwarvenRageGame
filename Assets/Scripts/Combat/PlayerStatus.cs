using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : StatusController
{
    private PlayerMovement movement;
    private PlayerEquipment equipment;

    private Transform lastAttackPoint;

    [SerializeField] private Transform testPoint;

    [SerializeField] private float goldDropRate = 1f;
    
    public override float Health
    {
        get { return health; }
        set
        {
            health = value;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
            else if (health < 0f)
            {
                health = 0f;
            }

            if (equipment != null && equipment.PlayerWeapon != null)
            {
                equipment.PlayerWeapon.HealthChangeEffect(this);
            }

            UserInterfaceController.instance?.UpdateHealthBar(health / maxHealth);
            UpdateCharacterUI();
            
        }
    }

    public override float MaxHealth
    {
        get { return maxHealth; }
        set
        {
            maxHealth = value;
            if (maxHealth < 0f)
            {
                maxHealth = 0f;
            }
            UserInterfaceController.instance?.UpdateHealthBar(health / maxHealth);
            UpdateCharacterUI();
        }
    }

    public override float Armor
    {
        get { return armor; }
        set 
        { 
            armor = value;
            if (armor > maxArmor)
            {
                armor = maxArmor;
            }
            else if (armor < 0f)
            {
                armor = 0f;
            }
            UserInterfaceController.instance?.UpdateArmor(armor);
            UpdateCharacterUI();
        }
    }

    public override float MaxArmor
    {
        get { return maxArmor; }
        set
        {
            maxArmor = value;
            UpdateCharacterUI();
        }
    }

    public PlayerMovement Movement
    {
        get { return movement; }
    }

    public PlayerEquipment Equipment
    {
        get { return equipment; }
    }

    public float GoldDropRate
    {
        get { return goldDropRate; }
        set 
        {
            goldDropRate = value;
        }
    }

    public override float Speed
    {
        get { return movement.Speed; }
        set { movement.Speed = value; }
    }

    public override float DefaultSpeed
    {
        get { return movement.DefaultSpeed; }
    }

    private void Awake ()
    {
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<AttackController>();
        equipment = GetComponent<PlayerEquipment>();
        animator = GetComponent<Animator>();

        manager = GameObject.Find("GameManager");
    }

    private void Update ()
    {
        if (Health <= 0f)
        {
            if (!isDying)
            {
                Die();
            }
        }
    }

    private new void Start ()
    {
        if (GameManager.instance.Player != null)
        {
            Health = GameManager.instance.Player.health;
            Armor = GameManager.instance.Player.armor;
            _resistances = GameManager.instance.Player.sheet.BuildSheet();
        }
        else
        {
            // Reset situation
            
            Health = maxHealth;
            Armor = 0f;
            _resistances = resistanceSheet.BuildSheet();
        }
    }

    protected override void Die ()
    {
        GameObject music = GameObject.Find("MusicPlayer");
        if (music != null)
        {
            music.GetComponent<MusicController>().Music.Stop();
        }
        AudioManager.instance.PlaySound("belgren_death");
        isDying = true;
        UserInterfaceController.instance.HidePlayerInterface();
        UserInterfaceController.instance.DeathMenu();
    }

    public override void TakeDamage (float value, DamageType type)
    {
        float newValue = value;

        // If resistance type is registered
        if (_resistances.ContainsKey(type))
            newValue *= (1f - _resistances[type]);

        if (IsBlocking())
        {
            // Plays block hit animation
            if (equipment.PlayerShield != null)
                animator.Play("player_block_hit");
            else
                animator.Play("player_block_shieldless_hit");

            // If player is not using two handed
            if (!equipment.IsTwoHanded && equipment.PlayerShield != null)
            {
                // Apply damage reduction
                newValue -= newValue * equipment.PlayerShield.protections[(int) type].resistance;
                AudioManager.instance.PlaySoundRandom("shield_block");
            }
            else
            {
                // Without shield, use weapon protections
                newValue -= newValue * equipment.PlayerWeapon.protections[(int) type].resistance;
            }
        }
        else
        {
            // Play damage sound
            PlayDamageSound();
        }

        // Blink screen
        if (IsBlocking() && type == DamageType.physical)
            BlockBlink();
        else
            DamageBlink(type);

        //Debug.Log("Health reduced:" + newValue);
        DeduceHealth(newValue);
    }

    public override void TakeDamage (float value, DamageType type, Vector3 point)
    {
        float newValue = value;
        bool blockedIncomingHit = false;

        // If resistance type is registered
        if (_resistances.ContainsKey(type))
            newValue *= (1f - _resistances[type]);

        if (IsBlocking() && CanBlockAttackAngle(point))
        {
            blockedIncomingHit = true;
            // Plays block hit animation
            if (equipment.PlayerShield != null)
                animator.Play("player_block_hit");
            else
                animator.Play("player_block_shieldless_hit");

            // If player is not using two handed
            if (!equipment.IsTwoHanded && equipment.PlayerShield != null)
            {
                // Apply damage reduction
                newValue -= newValue * equipment.PlayerShield.protections[(int) type].resistance;
                AudioManager.instance.PlaySoundRandom("shield_block");
            }
            else
            {
                // Without shield, use weapon protections
                newValue -= newValue * equipment.PlayerWeapon.protections[(int) type].resistance;
            }
        }
        else
        {
            // Play damage sound
            PlayDamageSound();
        }

        // Blink screen
        if (blockedIncomingHit && type == DamageType.physical)
            BlockBlink();
        else
            DamageBlink(type);

        //Debug.Log("Health reduced:" + newValue);
        DeduceHealth(newValue);
    }

    public override void TakeDamage (float value, DamageType type, Effect effect, Vector3 point)
    {
        // Apply effect
        for (int i = 0; i < _activeDOTs.Count; ++i)
        {
            if (_activeDOTs[i].Type == effect.dotDamageType)
            {
                _activeDOTs[i].ResetTimer();
                return;
            }
        }

        EffectProcessor.ProcessEffect(effect, this);
        
        // Damage processing
        TakeDamage(value, type, point);
    }

    private bool CanBlockAttackAngle (Vector3 point)
    {
        Vector3 planifiedPoint = new Vector3(point.x, transform.position.y, point.z);

        Vector3 playerForwardVector = transform.forward;
        Vector3 attackVector = (planifiedPoint - transform.position).normalized;

        float angle = Vector3.SignedAngle(transform.forward, attackVector, Vector3.up);
        
        /*
        float angle = Vector3.SignedAngle((transform.forward - transform.position).normalized, 
            (planifiedPoint - transform.position).normalized, Vector3.up);*/
        
        Debug.Log("Vectors: " + playerForwardVector + " | " 
                  + attackVector + "\nAngle: " + angle);
        
        Debug.DrawRay(transform.position, transform.forward, Color.red, 5f);
        Debug.DrawRay(transform.position, attackVector, Color.green, 5f);
        
        if (Mathf.Abs(angle) <= blockAngle)
        {
            return true;
        }
        
        return false;
    }

    public override void TakeDamageIgnoreBlock (float value, DamageType type)
    {
        float newValue = value;
    
        // If resistance type is registered
        if (_resistances.ContainsKey(type))
            newValue *= (1f - _resistances[type]);
        
        // Play damage sound
        PlayDamageSound();

        // Blink screen
        DamageBlink(type);

        //Debug.Log("Health reduced:" + newValue);
        DeduceHealth(newValue);
    }

    private void DamageBlink (DamageType type)
    {
        switch (type)
        {
            case DamageType.poison:
                UserInterfaceController.instance.ShowDamagePanel(new Color(0.3f, 1f, 0f));
                break;
            case DamageType.fire:
                UserInterfaceController.instance.ShowDamagePanel(new Color(1f, 0.35f, 0f));
                break;
            case DamageType.ice:
                UserInterfaceController.instance.ShowDamagePanel(new Color(0.25f, 0.25f, 1f));
                break;
            default:
                UserInterfaceController.instance.ShowDamagePanel(Color.red);
                break;
        }
    }

    private void BlockBlink()
    {
        UserInterfaceController.instance.ShowDamagePanel(Color.white);
    }

    private void UpdateCharacterUI ()
    {
        UserInterfaceController.instance?.UpdateCharacterFrame(Health, MaxHealth, Armor, MaxArmor);
    }

    private new void PlayDamageSound ()
    {
        float verify = UnityEngine.Random.Range(0f, 1f);
        if (verify <= 0.5f)
        {
            AudioManager.instance.PlaySoundRandom("player_damage");
        }
    }

    public override void PlayImpactSound () { }

    public bool IsBlocking()
    {
        return isBlocking;
    }

    public override void CheckForBlock(Transform attackPoint)
    {
        lastAttackPoint = attackPoint;
    }
    
    public override void WearStatus (EffectBase effect, float duration, float originalValue)
    {
        EffectController effectTimeout = gameObject.AddComponent<EffectController>();
        Debug.Log(effect.Type);
        effectTimeout.SetTimer(duration, () =>
        {
            effect.NormalizeValues(this, originalValue);
            Destroy(effectTimeout);
        },
        () => {
            UpdateEffectIcons(effect.Type, effectTimeout.Percentage);
        });
        
    }

    private void UpdateEffectIcons (EffectType effect, float percentage)
    {
        switch (effect)
        {
            case EffectType.berserk:
                UserInterfaceController.instance.UpdateBerserkIcon(percentage);
                break;
            case EffectType.fortune:
                UserInterfaceController.instance.UpdateFortuneIcon(percentage);
                break;
            case EffectType.fireResistance:
                UserInterfaceController.instance.UpdateFireResistanceIcon(percentage);
                break;
            case EffectType.poisonResistance:
                UserInterfaceController.instance.UpdatePoisonResistanceIcon(percentage);
                break;
        }
    }

    private void OnDrawGizmos ()
    {
        /*
        if (testPoint == null)
            return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, transform.forward);
        Gizmos.color = Color.green;
        Vector3 v = new Vector3(testPoint.position.x, transform.position.y, testPoint.position.z);
        Gizmos.DrawRay(v, transform.position - v); */
    }
}
