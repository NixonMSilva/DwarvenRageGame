using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Datasets/Weapon")]
public class Weapon : Item
{
    // Combat data
    public float damage;
    public bool isTwoHanded;
}