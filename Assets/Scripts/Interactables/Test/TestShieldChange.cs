using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShieldChange : MonoBehaviour, IInteractable
{
    [SerializeField] Shield shield;
    Shield aux;

    public void OnInteraction ()
    {
        //Debug.Log(weapon.itemName);
        PlayerEquipment equipment;
        equipment = GameObject.Find("Player").GetComponent<PlayerEquipment>();
        aux = equipment.PlayerShield;
        equipment.PlayerShield = shield;
        shield = aux;
    }
}