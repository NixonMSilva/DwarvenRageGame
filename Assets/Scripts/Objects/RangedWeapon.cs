using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New RangedWeapon", menuName = "Datasets/RangedWeapon")]
public class RangedWeapon : Item
{
    // Transform data
    public float posX = 0.001494f;
    public float posY = 0.000778f; 
    public float posZ = 0.000206f;

    // Combat data
    public float damage;
    public float criticalChance = 0.01f;
    public GameObject projectile;
    public AnimatorOverrideController animationSet;
}