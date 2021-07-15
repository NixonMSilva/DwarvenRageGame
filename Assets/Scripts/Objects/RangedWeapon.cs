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

    // Fire point data
    public float firePos1X = 0f;
    public float firePos1Y = 0f;
    public float firePos1Z = 0f;

    public float firePos2X = 0f;
    public float firePos2Y = 0f;
    public float firePos2Z = 0f;

    // Combat data
    public float damage;
    public DamageType damageType = DamageType.ranged;
    public float criticalChance = 0.01f;
    public float cooldown = 10f;
    public GameObject projectile;
    public AnimatorOverrideController animationSet;

    public string fireSound;
}