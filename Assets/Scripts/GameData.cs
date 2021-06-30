using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    // Player data
    public float[] position;
    public float health;
    public float maxHealth;
    public float armor;

    public int weapon;
    public int shield;
    public int gold;

    // Inventory data
    public int[] itemId;
    public int[] itemStack;

    // Scene data
    public bool[] pickupStatus;

    public GameData (SaveController data)
    {
        position = new float[3];
        position[0] = data.savePosition.x;
        position[1] = data.savePosition.y;
        position[2] = data.savePosition.z;

        health = data.saveHealth;
        maxHealth = data.saveMaxHealth;
        armor = data.saveArmor;

        weapon = data.saveWeapon;
        shield = data.saveShield;
        gold = data.saveGold;

        itemId = data.itemsId.ToArray();
        itemStack = data.itemsStack.ToArray();

        pickupStatus = data.savePickableStatus;
    }
    
}
