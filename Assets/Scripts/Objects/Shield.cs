using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shield", menuName = "Datasets/Shield")]
public class Shield : Item
{
    // Transform data
    public float rotX = 26.867f;
    public float rotY = 52.819f;
    public float rotZ = -4.477f;

    // Combat data
    public DamageSheet[] protections;
}
