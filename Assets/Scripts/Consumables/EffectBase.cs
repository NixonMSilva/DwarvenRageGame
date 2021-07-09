using System.Collections;
using UnityEngine;

public enum EffectType
{
    heal,
    healArmor,
    berserk,
    fortune,
    fireResistance,
    poisonResistance,
    addMaxHealth,
    addMaxArmor,
    poison,
    burning,
    none
}

public enum WeaponEffectType
{
    defaultEffect,
    belgrenEffect,
    brokrrEffect,
    jotunnEffect,
    vengeanceEffect,
    dwalingarEffect,
    kingEffect,
    draconicEffect,
}

public abstract class EffectBase
{
    public EffectBase () { }

    public abstract EffectType Type { get; }

    public abstract void ApplyEffect (StatusController target, Effect data);

    public abstract void StatusEnd (StatusController target);

    public abstract void NormalizeValues (StatusController target, float value);
}

public class Heal : EffectBase
{
    public Heal () { }
    public override EffectType Type => EffectType.heal;

    public override void ApplyEffect (StatusController target, Effect data)
    {
        target.Health += data.magnitude;
        
    }

    public override void StatusEnd (StatusController target) { }

    public override void NormalizeValues (StatusController target, float value) { }
}

public class HealArmor : EffectBase
{
    public HealArmor () { }
    public override EffectType Type => EffectType.healArmor;

    public override void ApplyEffect (StatusController target, Effect data)
    {
        target.Armor += data.magnitude;
    }

    public override void StatusEnd (StatusController target)
    {

    }

    public override void NormalizeValues (StatusController target, float value) { }
}


public class Berserker : EffectBase
{
    public Berserker () { }

    public override EffectType Type => EffectType.berserk;

    public override void ApplyEffect (StatusController target, Effect data)
    {
        target.Attack.Berserk = true;
        target.Attack.TemporaryDamage = target.Attack.Damage * data.magnitude;
        target.WearStatus(this, data.duration);

        // If it's the player, double the speed as well
        if (target.gameObject.CompareTag("Player"))
        {
            target.Speed = target.DefaultSpeed * data.magnitude;
        }
    }

    public override void StatusEnd (StatusController target)
    {
        target.Attack.Berserk = false;
        target.Attack.TemporaryDamage = target.Attack.Damage;
        target.Speed = target.DefaultSpeed;
    }

    public override void NormalizeValues (StatusController target, float value) { }
}

public class FireResistance : EffectBase
{
    public FireResistance () { }

    public override EffectType Type => EffectType.fireResistance;

    public override void ApplyEffect (StatusController target, Effect data)
    {
        float originalValue = target.Resistances[DamageType.fire]; 
        target.Resistances[DamageType.fire] = data.magnitude;
        target.WearStatus(this, data.duration, originalValue);
    }

    public override void StatusEnd (StatusController target) { }

    public override void NormalizeValues (StatusController target, float value)
    {
        target.Resistances[DamageType.fire] = value;
    }
}

public class PoisonResistance : EffectBase
{
    public PoisonResistance () { }

    public override EffectType Type => EffectType.poisonResistance;

    public override void ApplyEffect (StatusController target, Effect data)
    {
        float originalValue = target.Resistances[DamageType.poison];
        target.Resistances[DamageType.poison] = data.magnitude;
        target.WearStatus(this, data.duration, originalValue);
    }

    public override void StatusEnd (StatusController target) { }

    public override void NormalizeValues (StatusController target, float value)
    {
        target.Resistances[DamageType.fire] = value;
    }
}

public class Fortune : EffectBase
{
    public Fortune () { }

    public override EffectType Type => EffectType.fortune;

    public override void ApplyEffect (StatusController target, Effect data)
    {
        PlayerStatus player = target as PlayerStatus;

        player.GoldDropRate *= data.magnitude;
        player.WearStatus(this, data.duration, data.magnitude);
    }

    public override void StatusEnd (StatusController target) { }

    public override void NormalizeValues (StatusController target, float value) 
    {
        PlayerStatus player = target as PlayerStatus;
        player.GoldDropRate /= value;
    }
}

public class AddMaxHealth : EffectBase
{
    public AddMaxHealth () { }

    public override EffectType Type => EffectType.addMaxHealth;

    public override void ApplyEffect (StatusController target, Effect data)
    {
        target.MaxHealth += data.magnitude;
    }

    public override void StatusEnd (StatusController target)
    {

    }

    public override void NormalizeValues (StatusController target, float value) { }
}

public class AddMaxArmor : EffectBase
{
    public AddMaxArmor () { }

    public override EffectType Type => EffectType.addMaxArmor;

    public override void ApplyEffect (StatusController target, Effect data)
    {
        target.MaxArmor += data.magnitude;
    }

    public override void StatusEnd (StatusController target)
    {

    }

    public override void NormalizeValues (StatusController target, float value) { }
}

public class Poison : EffectBase
{
    public Poison () { }

    public override EffectType Type => EffectType.poison;

    public override void ApplyEffect (StatusController target, Effect data)
    {
        target.DamageOverTime(data.magnitude, DamageType.poison, data.duration, data.tick);
    }

    public override void StatusEnd (StatusController target) { }

    public override void NormalizeValues (StatusController target, float value) { }
}

public class Burning : EffectBase
{
    public Burning () { }

    public override EffectType Type => EffectType.burning;

    public override void ApplyEffect (StatusController target, Effect data)
    {
        target.DamageOverTime(data.magnitude, DamageType.fire, data.duration, data.tick);
    }

    public override void StatusEnd (StatusController target) { }

    public override void NormalizeValues (StatusController target, float value) { }
}

public class None : EffectBase
{
    public None () { }

    public override EffectType Type => EffectType.none;
    public override void ApplyEffect (StatusController target, Effect data) { }

    public override void StatusEnd (StatusController target) { }

    public override void NormalizeValues (StatusController target, float value) { }
}

//------------------------------------------------
// Weapon Effects
//------------------------------------------------

public abstract class WeaponEffect
{
    public WeaponEffect () { }

    public abstract WeaponEffectType WeaponType { get; }

    public abstract float ApplyEffectOnDamage (StatusController attacker, IDamageable target);

    public abstract void ApplyEffectOnEquip (StatusController user);

    public abstract void ApplyEffectOnUnequip (StatusController user);

    public abstract void ApplyEffectOnHealthChange (StatusController user);
}

public class DefaultEffect : WeaponEffect
{
    public DefaultEffect () { }

    public override WeaponEffectType WeaponType => WeaponEffectType.defaultEffect;

    public override float ApplyEffectOnDamage (StatusController attacker, IDamageable target)
    {
        return 1f;
    }

    public override void ApplyEffectOnEquip (StatusController user)
    {

    }

    public override void ApplyEffectOnUnequip (StatusController user)
    {

    }

    public override void ApplyEffectOnHealthChange (StatusController user)
    {

    }
}


public class BelgrenEffect : WeaponEffect
{
    public float goldDropRateIncrease = 1.1f;

    public BelgrenEffect () { }

    public override WeaponEffectType WeaponType => WeaponEffectType.belgrenEffect;

    public override float ApplyEffectOnDamage (StatusController attacker, IDamageable target)
    {
        return 1f;
    }

    public override void ApplyEffectOnEquip (StatusController user)
    {
        PlayerStatus player = user as PlayerStatus;
        player.GoldDropRate *= goldDropRateIncrease; 
    }

    public override void ApplyEffectOnUnequip (StatusController user)
    {
        PlayerStatus player = user as PlayerStatus;
        player.GoldDropRate /= goldDropRateIncrease;
    }

    public override void ApplyEffectOnHealthChange (StatusController user)
    {

    }
}

public class BrokrrEffect : WeaponEffect
{
    public BrokrrEffect () { }

    public override WeaponEffectType WeaponType => WeaponEffectType.brokrrEffect;

    public override float ApplyEffectOnDamage (StatusController attacker, IDamageable target)
    {
        // Recover player health during attack, chance based
        float diceRoll = Random.Range(0f, 1f);
        if (diceRoll <= 0.15f)
        {
            attacker.Health += attacker.MaxHealth * 0.3f;
        }
        // Return 1 so it doesn't change the original damage throughput 
        return 1f;
    }

    public override void ApplyEffectOnEquip (StatusController user)
    {
        
    }

    public override void ApplyEffectOnUnequip (StatusController user)
    {
        
    }

    public override void ApplyEffectOnHealthChange (StatusController user)
    {

    }
}

public class JotunnEffect : WeaponEffect
{
    float slowdown = 0.2f;
    float attackSpeed = 0.1f;

    public JotunnEffect () { }

    public override WeaponEffectType WeaponType => WeaponEffectType.jotunnEffect;

    public override float ApplyEffectOnDamage (StatusController attacker, IDamageable target)
    {
        return 1f;
    }

    public override void ApplyEffectOnEquip (StatusController user)
    {
        PlayerStatus player = user as PlayerStatus;
        // Change attack speed
        player.Attack.AttackSpeed -= attackSpeed;
        // Change player speed
        player.Movement.Speed = player.Movement.Speed * (1f - slowdown);
    }

    public override void ApplyEffectOnUnequip (StatusController user)
    {
        PlayerStatus player = user as PlayerStatus;
        // Change attack speed
        player.Attack.AttackSpeed += attackSpeed;
        // Change player speed
        player.Movement.Speed = player.Movement.DefaultSpeed;
    }

    public override void ApplyEffectOnHealthChange (StatusController user)
    {

    }
}

public class VengeanceEffect : WeaponEffect
{
    public float vengeanceCap = 2f;

    public VengeanceEffect () { }

    public override WeaponEffectType WeaponType => WeaponEffectType.vengeanceEffect;

    public override float ApplyEffectOnDamage (StatusController attacker, IDamageable target)
    {
        float damageIncrease = vengeanceCap - (attacker.Health / attacker.MaxHealth);
        return damageIncrease;
    }

    public override void ApplyEffectOnEquip (StatusController user)
    {

    }

    public override void ApplyEffectOnUnequip (StatusController user)
    {
        // Normalize attack speed
        user.Attack.AttackSpeed = 1f;
    }

    public override void ApplyEffectOnHealthChange (StatusController user)
    {
        // Increase attack speed
        PlayerStatus player = user as PlayerStatus;
        float attackSpeed = vengeanceCap - (player.Health / player.MaxHealth);
        player.Attack.AttackSpeed = attackSpeed;
    }
}

public class DwalingarEffect : WeaponEffect
{
    float value = 25f;

    public DwalingarEffect () { }

    public override WeaponEffectType WeaponType => WeaponEffectType.dwalingarEffect;

    public override float ApplyEffectOnDamage (StatusController attacker, IDamageable target)
    {
        return 1f;
    }

    public override void ApplyEffectOnEquip (StatusController user)
    {
        user.MaxHealth += value;
        user.Health += value;
    }

    public override void ApplyEffectOnUnequip (StatusController user)
    {
        user.MaxHealth -= value;
        if (user.Health > user.MaxHealth)
            user.Health = user.MaxHealth;
    }

    public override void ApplyEffectOnHealthChange (StatusController user)
    {

    }
}

public class KingEffect : WeaponEffect
{
    public KingEffect () { }

    public override WeaponEffectType WeaponType => WeaponEffectType.kingEffect;

    public override float ApplyEffectOnDamage (StatusController attacker, IDamageable target)
    {
        return 1f;
    }

    public override void ApplyEffectOnEquip (StatusController user)
    {

    }

    public override void ApplyEffectOnUnequip (StatusController user)
    {

    }

    public override void ApplyEffectOnHealthChange (StatusController user)
    {

    }
}

public class DraconicEffect : WeaponEffect
{
    public Consumable draconicItem;

    public DraconicEffect () 
    {
        draconicItem = Resources.Load<Consumable>("Consumables/FireResistance");
    }

    public override WeaponEffectType WeaponType => WeaponEffectType.draconicEffect;

    public override float ApplyEffectOnDamage (StatusController attacker, IDamageable target)
    {
        float diceRoll = Random.Range(0f, 1f);
        if (diceRoll <= 0.2f)
        {
            draconicItem.Use(attacker);
        }
        return 1f;
    }

    public override void ApplyEffectOnEquip (StatusController user)
    {
        PlayerStatus player = user as PlayerStatus;
        // Draconic shield has ID 13
        if (player.Equipment.PlayerShield != null && player.Equipment.PlayerShield.id == 13)
        {
            player.Equipment.PlayerWeapon.damageType = DamageType.dragonKiller;
        }
        else
        {
            player.Equipment.PlayerWeapon.damageType = DamageType.physical;
        }
    }

    public override void ApplyEffectOnUnequip (StatusController user)
    {
        PlayerStatus player = user as PlayerStatus;
        player.Equipment.PlayerWeapon.damageType = DamageType.physical;
    }

    public override void ApplyEffectOnHealthChange (StatusController user)
    {

    }
}


