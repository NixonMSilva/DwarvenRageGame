using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageSheet
{
    public DamageType type;
    [Range(-5f, 5f)]
    public float resistance;
}