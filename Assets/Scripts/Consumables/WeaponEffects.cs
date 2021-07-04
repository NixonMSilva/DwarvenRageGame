using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WeaponEffectProcessor
{
    private static Dictionary<WeaponEffectType, WeaponEffect> _weaponEffects = new Dictionary<WeaponEffectType, WeaponEffect>();

    private static bool isInitialized;

    private static void Initialize ()
    {
        _weaponEffects.Clear();
       
        // Reflection: Get all types that are derivate from EffectBase
        var assembly = Assembly.GetAssembly(typeof(WeaponEffect));
        var allWeaponEffectType = assembly.GetTypes()
            .Where(t => typeof(WeaponEffect).IsAssignableFrom(t) && t.IsAbstract == false);

        foreach (var weaponEffectType in allWeaponEffectType)
        {
            WeaponEffect effect = Activator.CreateInstance(weaponEffectType) as WeaponEffect;
            _weaponEffects.Add(effect.WeaponType, effect);
        }

        isInitialized = true;

    }

    public static float ProcessWeaponEffectOnDamage (WeaponEffectType effect, StatusController attacker, IDamageable target)
    {
        if (isInitialized == false)
            Initialize();

        foreach (KeyValuePair<WeaponEffectType, WeaponEffect> kvp in _weaponEffects)
        {
            //Debug.Log("Key = {" + kvp.Key + "}, Value = {" + kvp.Value + "}");
        }
        var currentEffect = _weaponEffects[effect];
        return currentEffect.ApplyEffectOnDamage(attacker, target);
    }

    public static void ProcessWeaponEffectOnEquip (WeaponEffectType effect, StatusController user)
    {
        if (isInitialized == false)
            Initialize();

        foreach (KeyValuePair<WeaponEffectType, WeaponEffect> kvp in _weaponEffects)
        {
            //Debug.Log("Key = {" + kvp.Key + "}, Value = {" + kvp.Value + "}");
        }
        var currentEffect = _weaponEffects[effect];
        currentEffect.ApplyEffectOnEquip(user);
    }

    public static void ProcessWeaponEffectOnUnequip (WeaponEffectType effect, StatusController user)
    {
        if (isInitialized == false)
            Initialize();

        foreach (KeyValuePair<WeaponEffectType, WeaponEffect> kvp in _weaponEffects)
        {
            //Debug.Log("Key = {" + kvp.Key + "}, Value = {" + kvp.Value + "}");
        }
        var currentEffect = _weaponEffects[effect];
        currentEffect.ApplyEffectOnUnequip(user);
    }

    public static void ProcessWeaponEffectOnHealthChange (WeaponEffectType effect, StatusController user)
    {
        if (isInitialized == false)
            Initialize();

        foreach (KeyValuePair<WeaponEffectType, WeaponEffect> kvp in _weaponEffects)
        {
            //Debug.Log("Key = {" + kvp.Key + "}, Value = {" + kvp.Value + "}");
        }

        var currentEffect = _weaponEffects[effect];
        currentEffect.ApplyEffectOnHealthChange(user);
    }
}