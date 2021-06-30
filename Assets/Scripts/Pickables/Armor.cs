using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Armor : PickableBase
{
    [SerializeField] private float armorValue = 100f;

    public override void ApplyEffect (StatusController player)
    {
        Debug.Log("Here!");
        player.AddArmor(armorValue);
    }
}
