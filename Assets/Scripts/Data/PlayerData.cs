using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    // Status
    public float health, armor;
    public Resistance sheet;
    
    // Equipment
    public Weapon playerWeapon;
    public Shield playerShield;
    public RangedWeapon playerRanged;
    public int gold;
    
    // Inventory
    public List<Weapon> weaponList;
    public List<Consumable> itemList;
    public List<int> itemStack;

    public PlayerData (StatusController _status, PlayerEquipment _equipment, Inventory _inventory)
    {
        // Status
        this.health = _status.Health;
        this.armor = _status.Armor;
        this.sheet = _status.Sheet;

        // Equipment
        this.playerWeapon = _equipment.PlayerWeapon;
        this.playerShield = _equipment.PlayerShield;
        this.playerRanged = _equipment.PlayerRanged;
        this.gold = _equipment.Gold;

        // Inventory
        this.weaponList = _inventory.Weapons;
        this.itemList = _inventory.Items;
        this.itemStack = _inventory.Stacks;
    }
}
