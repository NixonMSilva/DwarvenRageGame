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
        }
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

    private void Start ()
    {
        Armor = 0f;
    }

    public override void Die ()
    {
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

    public override void Berserk (bool status, float magnitude)
    {
        attack.Berserk = status;

        if (status)
        {
            movement.Speed *= magnitude;
            attack.Damage *= magnitude;
        }
        else
        {
            movement.Speed = movement.DefaultSpeed;
            attack.Damage /= magnitude;
        }
    }
}
