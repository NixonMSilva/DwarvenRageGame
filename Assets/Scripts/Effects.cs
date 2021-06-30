using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectDataType
{
    heal,
    berserk,
    fortune,
    fireResistance,
    poisonResistance,
}

namespace Effects
{
    [System.Serializable]
    public struct EffectActionData
    {
        public EffectDataType type;
        public float magnitude;
        public float timeout;
        public string name;
    }

    public static class HealEffect
    {
        public static void ApplyEffect (EffectActionData effect)
        {
            GameObject.Find("Player").GetComponent<StatusController>().Health += effect.magnitude;
        }
    }

    public static class BerserkEffect
    {
        public static  void ApplyEffect (EffectActionData effect)
        {
            GameObject.Find("Player").GetComponent<StatusController>().AddStatus(effect.type, effect.magnitude, effect.timeout);
        }
    }

    public static class FireResistanceEffect
    {
        public static void ApplyEffect (EffectActionData effect)
        {
            Debug.Log("logic");
        }
    }

    public static class PoisonResistanceEffect
    {
        public static void ApplyEffect (EffectActionData effect)
        {
            Debug.Log("logic");
        }
    }

    public static class FortuneEffect
    {
        public static void ApplyEffect (EffectActionData effect)
        {
            Debug.Log("Logic");
        }
    }
}
