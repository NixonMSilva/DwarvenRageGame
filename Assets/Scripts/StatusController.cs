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

    protected float speed = 0f;

    public AttackController Attack 
    { 
        get { return attack; } 
    }

    public virtual float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    public virtual float DefaultSpeed
    {
        get { return speed; }
    }

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

    public virtual float MaxHealth
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

    public float MaxArmor
    {
        get { return maxArmor; }
        set
        {
            maxArmor = value;
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

    public void WearStatus (EffectBase effect, float duration)
    {
        ActionOnTimer timeout = gameObject.AddComponent<ActionOnTimer>();
        timeout.SetTimer(duration, () =>
        {
            effect.StatusEnd(this);
            Destroy(timeout);
        });
    }
}
