using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Datasets/Weapon")]
public class Weapon : Item
{
    // Transform data
    public float posX = 0.001494f;
    public float posY = 0.000778f; 
    public float posZ = 0.000206f;

    public float rotX = 2.216f;
    public float rotY = -91.993f;
    public float rotZ = -161.967f;

    // Combat data
    public float damage;
    public bool isTwoHanded;


}