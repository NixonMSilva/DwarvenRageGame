using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Datasets/Effects")]
public class Effect : ScriptableObject
{
    public EffectType type;
    public float magnitude;
    public float duration;
}
