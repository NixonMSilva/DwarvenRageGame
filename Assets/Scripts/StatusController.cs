using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusController : MonoBehaviour, IDamageable
{
    protected AttackController attack;

    protected Animator animator;

    protected GameObject manager;

    [SerializeField] protected float maxHealth = 100f;

    [SerializeField] protected float health;

    [SerializeField] protected float maxArmor = 100f;

    [SerializeField] protected float armor;

    protected float fireResistance = 0f, poisonResistance = 0f;

    [SerializeField] protected bool isBlocking;

    protected bool isDying = false;

    public virtual float Health
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
        }
    }

    public float MaxHealth
    {
        get { return maxHealth; }
        set
        {
            maxHealth = value;
            if (maxHealth < 0f)
            {
                maxHealth = 0f;
            }
        }
    }

    public virtual float Armor
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
        }
    }

    public bool IsBlocking
    {
        get { return isBlocking; }
        set { isBlocking = value; }
    }

    public bool IsDying
    {
        get { return isDying; }
        set { isDying = value; }
    }

    protected void Start ()
    {
        Armor = 0f;
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

    public virtual void Die ()
    {
        // Generic death
        isDying = true;
    }

    public void AddArmor (float delta)
    {
        Armor += delta;
    }

    public virtual void TakeDamage (float value)
    {
        if (Armor > 0)
        {
            Armor -= value;
            float remainder = value - armor;
            if (remainder > 0f)
            {
                Health -= remainder;
            }
        }
        else
        {
            Health -= value;
        }
    }

    public void TakeElementalDamage (float value, string type)
    {
        if (type.Equals("poison"))
        {
            value *= 1f - poisonResistance;
        }
        else if (type.Equals("fire"))
        {
            value *= 1f - fireResistance;
        }
        TakeDamage(value);
    }

    public void AddStatus (EffectDataType statusType, float magnitude, float timeout)
    {
        ActionOnTimer statusTimeout = manager.AddComponent<ActionOnTimer>();

        HandleStatus(statusType, magnitude, true);

        statusTimeout.SetTimer(timeout, () =>
        {
            // Deactivate status upon timeout completion
            HandleStatus(statusType, magnitude, false);

            // Destroy timer
            Destroy(statusTimeout);
        });
    }

    private void HandleStatus (EffectDataType statusType, float magnitude, bool isActivation)
    {
        if (!isActivation)
            magnitude *= -1;

        switch (statusType)
        {
            case EffectDataType.fireResistance:
                AddFireResistance(magnitude);
                break;
            case EffectDataType.poisonResistance:
                AddPoisonResistance(magnitude);
                break;
            case EffectDataType.berserk:
                Berserk(isActivation, magnitude);
                break;
            case EffectDataType.fortune:
                Fortune(isActivation, magnitude);
                break;
        }
    }

    private void AddFireResistance (float value)
    {
        fireResistance += value;
    }

    private void AddPoisonResistance (float value)
    {
        poisonResistance += value;
    }

    private void Fortune (bool status, float magnitude)
    {

    }

    public virtual void Berserk (bool status, float magnitude)
    {
        attack.Berserk = status;

        if (status)
        {
            attack.Damage *= magnitude;
        }
        else
        {
            attack.Damage *= magnitude;
        }
    }
}
