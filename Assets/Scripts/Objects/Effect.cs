using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effects;

[CreateAssetMenu(fileName = "New Effect", menuName = "Datasets/Effects")]
public class Effect : ScriptableObject
{
    public EffectActionData itemEffect;

    public void CheckConditions ()
    {
        switch (itemEffect.type)
        {
            case EffectDataType.heal:
                Debug.Log("Healed");
            break;

            case EffectDataType.berserk:
                Debug.Log("Berserk on!");
            break;

            case EffectDataType.fireResistance:
                Debug.Log("Fire resistance!");
            break;

            case EffectDataType.poisonResistance:
                Debug.Log("Poison resistance");
                break;

            case EffectDataType.fortune:
                Debug.Log("Fortune");
                break;
        }
    }

}
