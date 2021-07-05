using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : StatusController
{
    private PlayerMovement movement;
    private PlayerEquipment equipment;

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
        Health = maxHealth;
        Armor = 0f;

        _resistances = resistanceSheet.BuildSheet();
    }

    public override void Die ()
    {
        isDying = true;
        UserInterfaceController.instance.DeathMenu();
    }

    public override void TakeDamage (float value, DamageType type)
    {
        float newValue = value;

        // If resistance type is registered
        if (_resistances.ContainsKey(type))
            newValue *= (1f - _resistances[type]);

        if (isBlocking)
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
                newValue -= newValue * equipment.PlayerShield.protections[(int)type].resistance;
                AudioManager.instance.PlaySoundRandom("shield_block");
            }
            else
            {
                // Without shield, use weapon protections
                newValue -= newValue * equipment.PlayerWeapon.protections[(int)type].resistance;
            }
        }
        else
        {
            // Play damage sound
            PlayDamageSound();
        }

        UserInterfaceController.instance.ShowDamagePanel();

        Debug.Log("Health reduced:" + newValue);

        DeduceHealth(newValue);
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
            AudioManager.instance.PlaySoundRandom("damage");
        }
    }

    public override void PlayImpactSound () { }
}
