using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    // Scene
    public int sceneIndex;
    
    // Orientation
    public float posX, posY, posZ;
    public float rotX, rotY, rotZ, rotW;
    
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
    
    // Scene status
    public bool[] pickableStatus;
    public bool[] enemyStatus;
    public bool[] eventStatus;

    public PlayerData ()
    {
        weaponList = new List<Weapon>();
        itemList = new List<Consumable>();
        itemStack = new List<int>();
    }

    public PlayerData (Transform transform, StatusController _status, PlayerEquipment _equipment, Inventory _inventory)
    {
        // Orientation
        this.posX = transform.position.x;
        this.posY = transform.position.y;
        this.posZ = transform.position.z;
        this.rotX = transform.rotation.x;
        this.rotY = transform.rotation.w;
        this.rotZ = transform.rotation.z;
        this.rotW = transform.rotation.w;
        
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
