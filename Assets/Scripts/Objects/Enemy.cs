using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Datasets/Enemy")]
public class Enemy : ScriptableObject
{
    public string enemyName;

    public float maxHealth;

    public float attackDamage;

    public Mesh enemyMesh;

    public List<string> dropList;

    public List<float> dropChance;
}
