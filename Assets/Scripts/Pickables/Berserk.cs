using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Berserk : PickableBase
{
    public override void ApplyEffect (StatusController player)
    {
        Debug.Log("Here!");
        player.AddStatus(EffectDataType.berserk, 1f, 5f);
    }
}
