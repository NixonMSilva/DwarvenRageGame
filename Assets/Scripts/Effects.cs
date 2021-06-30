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

    public abstract class EffectAction
    {
        public abstract void ApplyEffect (EffectActionData effect);
    }

    public class HealEffect : EffectAction
    {
        public override void ApplyEffect (EffectActionData effect)
        {
            GameObject.Find("Player").GetComponent<StatusController>().Health += effect.magnitude;
        }
    }

    public class BerserkEffect : EffectAction
    {
        public override void ApplyEffect (EffectActionData effect)
        {
            GameObject.Find("Player").GetComponent<StatusController>().AddStatus(effect.type, effect.magnitude, effect.timeout);
        }
    }

    public class FireResistanceEffect : EffectAction
    {
        public override void ApplyEffect (EffectActionData effect)
        {
            Debug.Log("logic");
        }
    }

    public class PoisonResistanceEffect : EffectAction
    {
        public override void ApplyEffect (EffectActionData effect)
        {
            Debug.Log("logic");
        }
    }
}
