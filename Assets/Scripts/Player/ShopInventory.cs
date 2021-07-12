using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShopInventory : MonoBehaviour
{
    [SerializeField] private List<Item> _shopInventory;
    [SerializeField] private List<int> _shopInventoryStack;

    [SerializeField] private ShopList loadedShopList;

    private int shopInventoryLimit = 21;
    private int maxStack = 999;

    public List<Item> ItemList
    {
        get { return _shopInventory; }
        set { _shopInventory = value; }
    }

    public List<int> ItemStack
    {
        get { return _shopInventoryStack; }
        set { _shopInventoryStack = value; }
    }

    private void Awake ()
    {
        // Building slots for all types of items from shop sheet
        for (int i = 0; i < loadedShopList.sheet.Length && i < shopInventoryLimit; ++i)
        {
            _shopInventory.Add(loadedShopList.sheet[i].item);
            _shopInventoryStack.Add(loadedShopList.sheet[i].quantity);
        }
    }
    
    public bool AddItem (Item item)
    {
        if (_shopInventory.Contains(item) && FirstWhereStackIsNotFull(item.id) != -1)
        {
            
            // If the item exists then check if
            // itcan be added to the stack
            // int index = _shopInventory.LastIndexOf(item);

            int index = FirstWhereStackIsNotFull(item.id);

            if (_shopInventoryStack[index] >= maxStack)
            {
                if (_shopInventory.Count + 1 <= shopInventoryLimit)
                {
                    // Add as a new stack if you can
                    _shopInventory.Add(item);
                    _shopInventoryStack.Add(1);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                // Add item
                _shopInventoryStack[index] += 1;
            }
        }
        else
        {
            // If the item doesn't exist, check if the inventory
            // isn't full
            if (_shopInventory.Count < shopInventoryLimit)
            {
                _shopInventory.Add(item);
                _shopInventoryStack.Add(1);
            }
            else
            {
                return false;
            }
        }
        //_shopInventory.Add(item);
        return true;
    }

    public void SubtractItemFromSlot (int slot)
    {
        if (slot > _shopInventory.Count)
            return;

        if (_shopInventoryStack[slot] == 1)
        {
            // If it's the last item at the stack
            _shopInventoryStack[slot] -= 1;
            RemoveItemAtSlot(slot);
        }
        else
        {
            // If it isn't
            _shopInventoryStack[slot] -= 1;
        }
    }

    public Item RemoveItemAtSlot (int slot)
    {
        Item itemBeingRemoved = _shopInventory[slot];
        _shopInventory.RemoveAt(slot);
        _shopInventoryStack.RemoveAt(slot);
        return itemBeingRemoved;
    }

    private int FirstWhereStackIsNotFull (int itemId)
    {
        for (int i = 0; i < _shopInventory.Count; ++i)
        {
            if (_shopInventory[i].id == itemId && _shopInventoryStack[i] < maxStack)
            {
                return i;
            }
        }
        return 0;
    }

}
