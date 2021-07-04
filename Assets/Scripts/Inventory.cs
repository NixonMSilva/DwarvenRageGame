using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<Weapon> _weaponSlots = new List<Weapon>();
    [SerializeField] private List<Consumable> _itemSlots = new List<Consumable>();
    [SerializeField] private List<int> _itemSlotsStack = new List<int>();

    private int maxStack = 99;

    [SerializeField]
    private int weaponLimit, itemLimit;

    private PlayerEquipment equipment;
    private PlayerStatus playerStatus;

    private void Awake ()
    {
        equipment = GetComponent<PlayerEquipment>();
        playerStatus = GetComponent<PlayerStatus>();

        //_weaponSlots = new List<Weapon>(weaponLimit);
        //_itemSlots = new List<Item>(itemLimit);
    }

    private void Start ()
    {       
        DrawWeaponSlots();
        DrawItemSlots();

        InputHandler.instance.OnItemKeyPressed += ItemUseHandler;
        InputHandler.instance.OnWeaponScroll += WeaponChangeHandler;
    }

    private void OnDestroy ()
    {
        InputHandler.instance.OnItemKeyPressed -= ItemUseHandler;
        InputHandler.instance.OnWeaponScroll -= WeaponChangeHandler;
    }

    private void WeaponChangeHandler (bool isUp)
    {
        if (_weaponSlots.Count > 0)
        {
            Weapon equippedWeapon = equipment.PlayerWeapon;
            Weapon newWeapon;
            if (isUp)
            {
                newWeapon = EquipNextWeapon(equippedWeapon);
            }
            else
            {
                newWeapon = EquipPreviousWeapon(equippedWeapon);
            }
            equipment.PlayerWeapon = newWeapon;
            UpdateWeaponSlots();
        }
    }

    private void ItemUseHandler (int index)
    {
        if (index < _itemSlots.Count && _itemSlots[index] != null)
        {
            UseItemAtSlot(index);
        }
    }

    public bool AddItem (Consumable item)
    {
        if (_itemSlots.Contains(item))
        {
            // If the item exists then check if
            // itcan be added to the stack
            // int index = _itemSlots.LastIndexOf(item);

            int index = FirstWhereStackIsNotFull(item.id);

            if (_itemSlotsStack[index] >= maxStack)
            {
                Debug.Log(_itemSlots.Count + 1 + " | " + itemLimit);
                if (_itemSlots.Count +  1 <= itemLimit)
                {
                    // Add as a new stack if you can
                    _itemSlots.Add(item);
                    _itemSlotsStack.Add(1);
                }
            }
            else
            {
                // Add item
                _itemSlotsStack[index] += 1;
            }
        }
        else
        {
            // If the item doesn't exist, check if the inventory
            // isn't full
            if (_itemSlots.Count < itemLimit)
            {
                _itemSlots.Add(item);
                _itemSlotsStack.Add(1);
            }
            else
            {
                return false;
            }
            
        }
        //_itemSlots.Add(item);
        UpdateItemSlots();
        return true;
    }
    
    public bool AddItem (Consumable item, int quantity)
    {
        if (_itemSlots.Contains(item))
        {
            // If the item exists then check if
            // it can be added to the stack
            int index = FirstWhereStackIsNotFull(item.id);

            //int index = _itemSlots.LastIndexOf(item);

            if ((_itemSlotsStack[index] + quantity) >= maxStack)
            {
                // Can we add a new stack?
                if (_itemSlots.Count + 1 <= itemLimit)
                {
                    // If yes, fill the stack
                    _itemSlotsStack[index] = maxStack;

                    // Then add the remainder as a new stack
                    int remainder = (_itemSlotsStack[index] + quantity) - maxStack;
                    _itemSlots.Add(item);
                    _itemSlotsStack.Add(remainder);
                }
                else
                {
                    // If no, just fill the stack
                    _itemSlotsStack[index] = maxStack;
                }
            }
            else
            {
                // Add item by the quantity
                _itemSlotsStack[index] += quantity;
            }

        }
        else
        {
            // If the item doesn't exist, check if the inventory
            // isn't full
            if (_itemSlots.Count < itemLimit)
            {
                _itemSlots.Add(item);
                _itemSlotsStack.Add(quantity);
            }
            else
            {
                return false;
            }
        }
        UpdateItemSlots();
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
        _itemSlotsStack.RemoveAt(slot);
        UpdateItemSlots();
        // RemoveAt already reorganizes the list
        return itemBeingRemoved;
    }

    private void UseItemAtSlot (int slot)
    {
        //_itemSlots[slot].itemEffect.Use();
        _itemSlots[slot].Use(playerStatus);
        //_itemSlots[slot].itemEffect.Use(playerStatus);

        if (_itemSlotsStack[slot] == 1)
        {
            // If it's the last item at the stack
            _itemSlotsStack[slot] -= 1;
            RemoveItemAtSlot(slot);
        }
        else
        {
            // If it isn't
            _itemSlotsStack[slot] -= 1;
        }

        // Update view
        UpdateItemSlots();
    }

    private Weapon RemoveWeaponAtSlot (int slot)
    {
        Weapon weaponBeingRemoved = _weaponSlots[slot];
        _weaponSlots.RemoveAt(slot);
        UpdateWeaponSlots();
        // RemoveAt already reorganizes the list
        return weaponBeingRemoved;
    }

    private Weapon EquipNextWeapon (Weapon currentWeapon)
    {
        Weapon nextWeapon = _weaponSlots[0];
        int i;
        for (i = 0; i < _weaponSlots.Count - 1; ++i)
        {
            // Shift all weapons one slot to the left
            _weaponSlots[i] = _weaponSlots[i + 1];
        }
        // Put the current weapon at the end of the queue
        _weaponSlots[i] = currentWeapon;
        return nextWeapon;
    }

    private Weapon EquipPreviousWeapon (Weapon currentWeapon)
    {
        int lastIndex = _weaponSlots.Count - 1;
        Weapon nextWeapon = _weaponSlots[lastIndex];
        int i;
        for (i = 0; i < _weaponSlots.Count - 1; ++i)
        {
            // Shift all weapons one slot to the right
            _weaponSlots[i + 1] = _weaponSlots[i];
        }
        // Put the current weapon at the end of the queue
        _weaponSlots[0] = currentWeapon;
        return nextWeapon;
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
        UserInterfaceController.instance.ShowWeaponSlots();
        UpdateWeaponSlots();
    }

    private void DrawItemSlots ()
    {
        UserInterfaceController.instance.CreateItemSlots(itemLimit);
        UpdateItemSlots();
    }

    private void UpdateWeaponSlots ()
    {
        Sprite currentWeaponSprite = equipment.PlayerWeapon.icon;
        UserInterfaceController.instance.UpdateWeaponSlot(currentWeaponSprite);
    }

    private void UpdateItemSlots ()
    {
        List<Sprite> _itemSprites = new List<Sprite>();
        List<int> _itemStack = new List<int>();

        for (int i = 0; i < _itemSlots.Count; ++i)
        {
            if (_itemSlots[i] != null)
            {
                _itemSprites.Add(_itemSlots[i].icon);
                _itemStack.Add(_itemSlotsStack[i]);
            }
        }

        UserInterfaceController.instance.UpdateItemSlot(_itemSprites, _itemStack);
    }

    public List<int> GetItemList ()
    {
        List<int> itemIds = new List<int>(_itemSlots.Count);

        foreach (Item item in _itemSlots)
        {
            itemIds.Add(item.id);
        }

        return itemIds;
    }

    public List<int> GetItemStacks ()
    {
        List<int> itemStacks = new List<int>(_itemSlotsStack.Count);
        
        foreach (int stack in _itemSlotsStack)
        {
            itemStacks.Add(stack);
        }

        return itemStacks;
    }

    public List<int> GetWeaponList ()
    {
        List<int> weaponIds = new List<int>(_weaponSlots.Count + 1);

        weaponIds.Add(equipment.PlayerWeapon.id);

        foreach (Weapon weapon in _weaponSlots)
        {
            weaponIds.Add(weapon.id);
        }

        return weaponIds;
    }

    private int FirstWhereStackIsNotFull (int itemId)
    {
        for (int i = 0; i < _itemSlots.Count; ++i)
        {
            if (_itemSlots[i].id == itemId && _itemSlotsStack[i] < maxStack)
            {
                return i;
            }
        }
        return 0;
    }

    public void ClearWeapons () 
    {
        _weaponSlots.Clear();
    }

    public void ClearItems () 
    {
        _itemSlots.Clear();
        _itemSlotsStack.Clear();
    }

        
}
