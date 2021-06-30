using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    public int id;

    public string itemName;

    public Mesh worldMesh;
    public Material[] materialList;

    public float scaleX, scaleY, scaleZ;

    public Sprite icon;

}
