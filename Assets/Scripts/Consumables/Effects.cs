using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EffectProcessor
{
    private static Dictionary<EffectType, EffectBase> _effects = new Dictionary<EffectType, EffectBase>();


    private static bool isInitialized;

    private static void Initialize ()
    {
        _effects.Clear();
       
        // Reflection: Get all types that are derivate from EffectBase
        var assembly = Assembly.GetAssembly(typeof(EffectBase));
        var allEffectTypes = assembly.GetTypes()
            .Where(t => typeof(EffectBase).IsAssignableFrom(t) && t.IsAbstract == false);

        foreach (var effectType in allEffectTypes)
        {
            EffectBase effect = Activator.CreateInstance(effectType) as EffectBase;
            // Assign the effect type on the enum to its equivalent effect children class
            _effects.Add(effect.Type, effect);
        }

        isInitialized = true;

    }

    public static void ProcessEffect (Effect effect, StatusController target)
    {
        if (isInitialized == false)
            Initialize();

        foreach (KeyValuePair<EffectType, EffectBase> kvp in _effects)
        {
            //Debug.Log("Key = {" + kvp.Key + "}, Value = {" + kvp.Value + "}");
        }

        var currentEffect = _effects[effect.type];
        currentEffect.ApplyEffect(target, effect);
    }
}