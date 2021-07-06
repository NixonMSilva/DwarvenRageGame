using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Enemy", menuName = "Datasets/Enemy")]
public class Enemy : ScriptableObject
{
    public float maxHealth;

    public float attackDamage;

    public DropSheet[] drops;

    public int goldDrop;
    public int goldDropVariance;

    public DamageType damageType;
    public Effect damageEffect;

    public string soundIdle;
    public string soundAttack;
    public string soundDamage;
    public string soundDeath;
    public string impactType;
}
