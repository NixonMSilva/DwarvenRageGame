using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effects;

[CreateAssetMenu(fileName = "New Effect", menuName = "Datasets/Effects")]
public class Effect : ScriptableObject
{
    public EffectActionData itemEffect;

    public void Use ()
    {
        switch (itemEffect.type)
        {
            case EffectDataType.heal:
                HealEffect.ApplyEffect(itemEffect);
                Debug.Log("Healed");
            break;

            case EffectDataType.berserk:
                BerserkEffect.ApplyEffect(itemEffect);
                Debug.Log("Berserk on!");
            break;

            case EffectDataType.fireResistance:
                FireResistanceEffect.ApplyEffect(itemEffect);
                Debug.Log("Fire resistance!");
            break;

            case EffectDataType.poisonResistance:
                PoisonResistanceEffect.ApplyEffect(itemEffect);
                Debug.Log("Poison resistance");
                break;

            case EffectDataType.fortune:
                FortuneEffect.ApplyEffect(itemEffect);
                Debug.Log("Fortune");
                break;
        }
    }

}
