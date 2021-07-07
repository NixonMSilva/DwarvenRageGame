using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] protected bool isBlocking;

    protected bool isDying = false;

    protected float speed = 0f;

    [SerializeField] protected Resistance resistanceSheet;

    protected Dictionary<DamageType, float> _resistances;

    [SerializeField] protected List<DOTController> _activeDOTs;

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

    public virtual float MaxArmor
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

    public Dictionary<DamageType, float> Resistances
    {
        get { return _resistances; }
        set { _resistances = value; }
    }    

    public Resistance Sheet
    {
        get { return resistanceSheet; }
    }


    protected void Start ()
    {
        Armor = 0f;
        _resistances = resistanceSheet.BuildSheet();
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

    protected void DeduceHealth (float value)
    {
        if (Armor > 0)
        {
            float remainder = value - armor;
            Armor -= value;
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

    public virtual void TakeDamage (float value, DamageType type)
    {
        float newValue = value;

        // If resistance type is registered
        if (_resistances.ContainsKey(type))
            newValue *= (1f - _resistances[type]);

        DeduceHealth(newValue);
    }

    public void TakeDamage (float value, DamageType type, Effect effect)
    {
        TakeDamage(value, type);

        for (int i = 0; i < _activeDOTs.Count; ++i)
        {
            if (_activeDOTs[i].Type == effect.dotDamageType)
            {
                _activeDOTs[i].ResetTimer();
                return;
            }
        }

        EffectProcessor.ProcessEffect(effect, this);
    }

    public void WearStatus (EffectBase effect, float duration, Action onEnd)
    {
        ActionOnTimer timeout = gameObject.AddComponent<ActionOnTimer>();
        timeout.SetTimer(duration, onEnd);
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

    public void WearStatus (EffectBase effect, float duration, float originalValue)
    {
        ActionOnTimer timeout = gameObject.AddComponent<ActionOnTimer>();
        timeout.SetTimer(duration, () =>
        {
            effect.NormalizeValues(this, originalValue);
            Destroy(timeout);
        });
    }

    public void DamageOverTime (float magnitude, DamageType type, float duration, float tickTime)
    {
        DOTController dot = gameObject.AddComponent<DOTController>();
        _activeDOTs.Add(dot);
        dot.SetTimer(type, duration, tickTime, () =>
        {
            // On end
            //_activeDOTs.Remove(type);
            _activeDOTs.Remove(dot);
            Destroy(dot);
        },
        () =>
        {
            // On tick
            TakeDamage(magnitude, type);
        });
    }

    protected void PlayDamageSound () {  }

    public virtual void PlayImpactSound () 
    {
        Debug.Log("Here!");
    }
}
