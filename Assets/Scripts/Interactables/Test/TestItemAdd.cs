using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItemAdd : MonoBehaviour
{
    [SerializeField] private Consumable item;

    [SerializeField] private int count;

    public void OnInteraction ()
    {
        if (count == 1)
            GameObject.Find("Player").GetComponent<Inventory>().AddItem(item);
        else
            GameObject.Find("Player").GetComponent<Inventory>().AddItem(item, count);
    }
}
