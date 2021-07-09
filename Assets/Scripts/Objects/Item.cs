using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    public int id;

    public string itemName;

    [TextArea]
    public string itemDescription;

    public Mesh worldMesh;
    public Material[] materialList;

    public float scaleX, scaleY, scaleZ;

    public Sprite icon;

    // Shop system
    public int price = 0;

}
