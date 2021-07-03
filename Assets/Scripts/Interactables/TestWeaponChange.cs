using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWeaponChange : MonoBehaviour, IInteractable
{
    [SerializeField] Weapon weapon;
    Weapon aux;

    public void OnInteraction ()
    {
        //Debug.Log(weapon.itemName);
        PlayerEquipment equipment;
        equipment = GameObject.Find("Player").GetComponent<PlayerEquipment>();
        aux = equipment.PlayerWeapon;
        equipment.PlayerWeapon = weapon;
        weapon = aux;
    }
}