using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Datasets/Weapon")]
public class Weapon : Item
{
    // Transform data
    public float posX = 0.001494f;
    public float posY = 0.000778f; 
    public float posZ = 0.000206f;

    // Combat data
    public float damage;
    public DamageType damageType = DamageType.physical;
    public float criticalChance = 0.01f;
    public bool isTwoHanded;

    public DamageSheet[] protections;

    public WeaponEffectType effectType = WeaponEffectType.defaultEffect;

    public WeaponType weaponType = WeaponType.axe;

    public float AttackEffect (StatusController attacker, IDamageable target)
    {
        return WeaponEffectProcessor.ProcessWeaponEffectOnDamage(effectType, attacker, target);
    }

    public void EquipEffect (StatusController user)
    {
        WeaponEffectProcessor.ProcessWeaponEffectOnEquip(effectType, user);
    }

    public void UnequipEffect (StatusController user)
    {
        WeaponEffectProcessor.ProcessWeaponEffectOnUnequip(effectType, user);
    }

    public void HealthChangeEffect (StatusController user)
    {
        WeaponEffectProcessor.ProcessWeaponEffectOnHealthChange(effectType, user);
    }
}

public enum WeaponType
{
    axe,
    hammer,
    ice,
    fire
}