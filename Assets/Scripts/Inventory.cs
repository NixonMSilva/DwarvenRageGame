using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<Weapon> _weaponSlots = new List<Weapon>();
    [SerializeField] private List<Item> _itemSlots = new List<Item>();

    [SerializeField]
    private int weaponLimit, itemLimit;

    private PlayerEquipment equipment;

    private void Awake ()
    {
        equipment = GetComponent<PlayerEquipment>();

        //_weaponSlots = new List<Weapon>(weaponLimit);
        //_itemSlots = new List<Item>(itemLimit);
    }

    private void Start ()
    {       
        DrawWeaponSlots();
        InputHandler.instance.OnWeaponKeyPressed += WeaponChangeHandler;

    }

    private void OnDestroy ()
    {
        InputHandler.instance.OnWeaponKeyPressed -= WeaponChangeHandler;
    }

    private void WeaponChangeHandler (int index)
    {
        Weapon equippedWeapon = equipment.PlayerWeapon;
        if (_weaponSlots.Count <= index)
            Debug.Log("Weapon slot out of range!");
        else
        {
            if (_weaponSlots[index] == null)
                Debug.Log("Weapon slot is empty!");
            else
                equipment.PlayerWeapon = EquipWeaponAtSlot(index, equippedWeapon);
        }
    }

    private bool AddItem (Item item)
    {
        if (_itemSlots.Count >= itemLimit)
        {
            return false;
        }
        _itemSlots.Add(item);
        return true;
    }

    public bool AddWeapon (Weapon weapon)
    {
        if (_weaponSlots.Count >= weaponLimit)
        {
            return false;
        }
        _weaponSlots.Add(weapon);
        UpdateWeaponSlots();
        return true;
    }

    public Item RemoveItemAtSlot (int slot)
    {
        Item itemBeingRemoved = _itemSlots[slot];
        _itemSlots.RemoveAt(slot);
        UpdateWeaponSlots();
        // RemoveAt already reorganizes the list
        return itemBeingRemoved;
    }

    private void UseItemAtSlot (int slot)
    {
        
    }

    private Weapon RemoveWeaponAtSlot (int slot)
    {
        Weapon weaponBeingRemoved = _weaponSlots[slot];
        _weaponSlots.RemoveAt(slot);
        UpdateWeaponSlots();
        // RemoveAt already reorganizes the list
        return weaponBeingRemoved;
    }

    private Weapon EquipWeaponAtSlot (int slot, Weapon swappedWeapon)
    {
        Weapon weaponBeingEquipped = _weaponSlots[slot];
        _weaponSlots[slot] = swappedWeapon;
        UpdateWeaponSlots();
        return weaponBeingEquipped;
    }

    private void DrawWeaponSlots ()
    {
        UserInterfaceController.instance.CreateWeaponSlots(weaponLimit);
        UpdateWeaponSlots();
    }

    private void UpdateWeaponSlots ()
    {
        List<Sprite> _weaponSprites = new List<Sprite>();
        foreach (Weapon wpn in _weaponSlots)
        {
            if (wpn != null)
            {
                _weaponSprites.Add(wpn.icon);
            }
        }
        UserInterfaceController.instance.UpdateWeaponSlot(_weaponSprites);
    }

    
}
