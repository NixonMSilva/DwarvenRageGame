using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : StatusController
{
    private PlayerMovement movement;
    private PlayerEquipment equipment;

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
        Health = maxHealth;
        Armor = 0f;

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
        Armor = 0f;
    }

    public override void Die ()
    {
        isDying = true;
        UserInterfaceController.instance.DeathMenu();
    }

    public override void TakeDamage (float value)
    {
        if (isBlocking)
        {
            // Plays block hit animation
            animator.Play("player_block_hit");

            // If player has shield
            if (!equipment.IsTwoHanded)
                value -= value * equipment.PlayerShield.damageReduction;
            else
                value -= value * equipment.BaseDamageReduction;
        }

        UserInterfaceController.instance.ShowDamagePanel();

        base.TakeDamage(value);
    }

    private void UpdateCharacterUI ()
    {
        UserInterfaceController.instance?.UpdateCharacterFrame(Health, MaxHealth, Armor, MaxArmor);
    }
}
