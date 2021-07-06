using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shop List", menuName = "Datasets/Shop List")]
public class ShopList : ScriptableObject
{
    public ItemSheet[] sheet;
}
