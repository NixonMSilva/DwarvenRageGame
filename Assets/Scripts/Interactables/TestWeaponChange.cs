using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWeaponChange : MonoBehaviour, IInteractable
{
    [SerializeField] Weapon weapon;

    public void OnInteraction ()
    {
        Debug.Log(weapon.itemName);
        GameObject.Find("Player").GetComponent<Inventory>().AddWeapon(weapon);
    }
}