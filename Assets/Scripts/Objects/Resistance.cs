using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Resistance Sheet", menuName = "Datasets/Resistances")]
public class Resistance : ScriptableObject
{
    public DamageSheet[] sheet;

    public Dictionary<DamageType, float> BuildSheet ()
    {
        Dictionary<DamageType, float> newSheet = new Dictionary<DamageType, float>();

        foreach (DamageSheet s in sheet)
        {
            newSheet.Add(s.type, s.resistance);
        }

        return newSheet;
    }
}