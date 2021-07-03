using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Datasets/Consumable")]
public class Consumable : Item
{
    public Effect effect;

    public void Use (StatusController target)
    {
        EffectProcessor.ProcessEffect(effect, target);
    }
}
