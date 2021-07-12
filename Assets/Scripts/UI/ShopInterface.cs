using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text.RegularExpressions;
using TMPro;

public class ShopInterface : MonoBehaviour
{
    [SerializeField] private List<ShopSlot> _buySlots;
    [SerializeField] private List<ShopSlot> _sellItemSlots;
    [SerializeField] private List<ShopSlot> _sellWeaponSlots;
    [SerializeField] private ShopSlot _sellShieldSlot;
    [SerializeField] private ShopSlot _sellRangedSlot;

    private ShopSelectedSlot selectedSlot;

    // Seller data
    [SerializeField] private ShopController seller;
    [SerializeField] private ShopInventory sellerInventory;
    [SerializeField] private ShopList loadedList;

    // Player data
    private Inventory inventory;
    private PlayerEquipment equipment;

    private GameManager manager;

    public Inventory PlayerInventory
    {
        get { return inventory; }
    }

    public PlayerEquipment PlayerEquipment
    {
        get { return equipment; }
    }

    private void Awake ()
    {
        inventory = GameObject.Find("Player").GetComponent<Inventory>();
        equipment = GameObject.Find("Player").GetComponent<PlayerEquipment>();

        manager = GameObject.Find("GameManager").GetComponent<GameManager>();

        _buySlots = GetComponentsInChildren<ShopSlot>().Where(s => s.gameObject.name.Contains("BuyItem")).ToList();
        _sellItemSlots = GetComponentsInChildren<ShopSlot>().Where(s => s.gameObject.name.Contains("SellItem")).ToList();
        _sellWeaponSlots = GetComponentsInChildren<ShopSlot>().Where(s => s.gameObject.name.Contains("SellWeapon")).ToList();
        _sellShieldSlot = GetComponentsInChildren<ShopSlot>().Where(s => s.gameObject.name.Contains("SellShield")).ToList().First();
        _sellRangedSlot = GetComponentsInChildren<ShopSlot>().Where(s => s.gameObject.name.Contains("SellRanged")).ToList().First();

        selectedSlot = GameObject.Find("PanelItemData").GetComponent<ShopSelectedSlot>();
        
    }

    public void ShowShopInterface (GameObject obj)
    {
        seller = obj.GetComponent<ShopController>();
        sellerInventory = seller.GetComponent<ShopInventory>();
        seller.NewPurchase();
        InitializeBuySlots();
        InitializeSellSlots();
    }

    private void InitializeBuySlots ()
    {
        // Old way:
        /*
        // Building slots for all types of items from shop sheet
        for (int i = 0; i < loadedList.sheet.Length && i < _buySlots.Count; ++i)
        {
            //Debug.Log(loadedList.sheet[i].item);
            _buySlots[i].SlotItem = loadedList.sheet[i].item;
            _buySlots[i].SlotQuantity = loadedList.sheet[i].quantity;
        } */

        // New way:
        for (int i = 0; i < sellerInventory.ItemList.Count && i < _buySlots.Count; ++i)
        {
            _buySlots[i].SlotItem = sellerInventory.ItemList[i];
            _buySlots[i].SlotQuantity = sellerInventory.ItemStack[i];
        }
    }

    private void InitializeSellSlots ()
    {
        UpdatePlayerItemSlots();
        UpdatePlayerWeaponSlots();
        UpdatePlayerShieldSlot();
        UpdatePlayerRangedSlot();
    }

    private void UpdateShopSlots ()
    {
        // Reorganizes the slots
        int i = 0;
        for (i = 0; i < sellerInventory.ItemList.Count && i < _buySlots.Count; ++i)
        {
            _buySlots[i].SlotItem = sellerInventory.ItemList[i];
            _buySlots[i].SlotQuantity = sellerInventory.ItemStack[i];
        }
        
        for (; i < _buySlots.Count; ++i)
        {
            _buySlots[i].SlotQuantity = 0;
            _buySlots[i].SlotItem = null;
        }
    }

    private void UpdatePlayerItemSlots ()
    {
        // Building slots for items from player inventory
        List<int> playerItems = inventory.GetItemList();
        List<int> playerStack = inventory.GetItemStacks();

        int i;
        for (i = 0; i < playerItems.Count && i < _sellItemSlots.Count; ++i)
        {
            _sellItemSlots[i].SlotItem = manager.GetItemById(playerItems[i]);
            _sellItemSlots[i].SlotQuantity = playerStack[i];
        }

        for (; i < _sellItemSlots.Count; ++i)
        {
            _sellItemSlots[i].SlotItem = null;
            _sellItemSlots[i].SlotQuantity = 0;
        }
    }

    private void UpdatePlayerWeaponSlots ()
    {
        // Building slots for weapons from player inventory
        List<int> playerWeapons = inventory.GetWeaponList();

        int i;
        for (i = 0; i < playerWeapons.Count && i < _sellWeaponSlots.Count; ++i)
        {
            _sellWeaponSlots[i].SlotItem = manager.GetWeaponById(playerWeapons[i]);
            _sellWeaponSlots[i].SlotQuantity = 1;
        }
        
        for (; i < _sellWeaponSlots.Count; ++i)
        {
            _sellWeaponSlots[i].SlotItem = null;
            _sellWeaponSlots[i].SlotQuantity = 0;
        }
    }

    private void UpdatePlayerShieldSlot ()
    {
        // Building slots for the shield from player equipped items
        _sellShieldSlot.SlotItem = equipment.PlayerShield;
        _sellShieldSlot.SlotQuantity = 1;
    }

    private void UpdatePlayerRangedSlot ()
    {
        // Building slots for the ranged weapon from player equipped items
        _sellRangedSlot.SlotItem = equipment.PlayerRanged;
        _sellRangedSlot.SlotQuantity = 1;
    }

    public void EventNotEnoughCash ()
    {
        Debug.Log("Not enough cash, stranger!");
        UserInterfaceController.instance.ThrowWarningMessage("You don't have enough cash!");
    }

    public void EventShopPurchase (ShopSlot slot)
    {
        if (slot.SlotItem.GetType() == typeof(Consumable))
        {
            HandleItemPurchase(slot.SlotItem as Consumable, slot);
        }
        else if (slot.SlotItem.GetType() == typeof(Weapon))
        {
            HandleWeaponPurchase(slot.SlotItem as Weapon, slot);
        }
        else if (slot.SlotItem.GetType() == typeof(Shield))
        {
            HandleShieldPurchase(slot.SlotItem as Shield, slot);
        }
        else if (slot.SlotItem.GetType() == typeof(RangedWeapon))
        {
            HandleRangedPurchase(slot.SlotItem as RangedWeapon, slot);
        }
        else
        {
            Debug.LogWarning("Invalid item type at shop!");
        }        
    }

    private void EventPurchaseConfirmed (Item purchasedItem)
    {
        //Debug.Log("Sold!" + purchasedItem.itemName);

        seller.PlayPurchaseSound(purchasedItem.audioPurchaseName);

        seller.BoughtSomething = true;
        
        seller.PlaySellerAnimation();
    }

    private void EventSellConfirmed ()
    {
        seller.PlaySellerAnimation();
    }

    public void EventShopSell (ShopSlot slot)
    {
        if (slot.SlotItem.GetType() == typeof(Consumable))
        {
            HandleItemSell(slot.SlotItem as Consumable, slot);
        }
        else if (slot.SlotItem.GetType() == typeof(Weapon))
        {
            HandleWeaponSell(slot.SlotItem as Weapon, slot);
        }
        else if (slot.SlotItem.GetType() == typeof(Shield))
        {
            HandleShieldSell(slot.SlotItem as Shield, slot);
        }
        else if (slot.SlotItem.GetType() == typeof(RangedWeapon))
        {
            HandleRangedSell(slot.SlotItem as RangedWeapon, slot);
        }
        else
        {
            Debug.LogWarning("Invalid item type at shop!");
        }
    }

    public void EventInventoryFull ()
    {
        Debug.Log("Your inventory is full!");
        UserInterfaceController.instance.ThrowWarningMessage("Your inventory is full!");
    }

    public void EventWeaponFull ()
    {
        Debug.Log("Your weapon slots are full!");
        UserInterfaceController.instance.ThrowWarningMessage("Your weapon slots are full!");
    }

    public void EventShieldFull ()
    {
        Debug.Log("Your shield slot is occupied!");
        UserInterfaceController.instance.ThrowWarningMessage("Your shield slot is occupied!");
    }

    public void EventRangedFull ()
    {
        Debug.Log("Your ranged slot is occupied!");
        UserInterfaceController.instance.ThrowWarningMessage("Your ranged slot is occupied!");
    }

    private void HandleItemPurchase (Consumable purchasedItem, ShopSlot slot)
    {
        if (PlayerInventory.AddItem(purchasedItem))
        {
            // If purchase is sucessful

            int slotIndex = GetSlotNumber(slot);

            // If the item stack has been depleted after purchase
            // remove it from the slot
            /*
            if (slot.SlotQuantity == 1)
            {
                RemoveSlot(slot);
            }
            else
            {
                slot.SlotQuantity -= 1;
            } */

            DeduceGold(purchasedItem.price);
            sellerInventory.SubtractItemFromSlot(slotIndex - 1);
            UpdatePlayerItemSlots();
            UpdateShopSlots();
            EventPurchaseConfirmed(purchasedItem);
        }
        else
        {
            // In case the player's inventory is full
            EventInventoryFull();
        }
    }

    private void HandleWeaponPurchase (Weapon purchasedItem, ShopSlot slot)
    {
        if (PlayerInventory.AddWeapon(purchasedItem))
        {
            int slotIndex = GetSlotNumber(slot);
            sellerInventory.SubtractItemFromSlot(slotIndex - 1);
            // RemoveSlot(slot);
            DeduceGold(purchasedItem.price);
            UpdateShopSlots();
            UpdatePlayerWeaponSlots();
            EventPurchaseConfirmed(purchasedItem);
        }
        else
        {
            EventWeaponFull();
        }
    }

    private void HandleShieldPurchase (Shield purchasedItem, ShopSlot slot)
    {
        if (PlayerEquipment.PlayerShield != null)
        {
            EventShieldFull();
        }
        else
        {
            int slotIndex = GetSlotNumber(slot);
            sellerInventory.SubtractItemFromSlot(slotIndex - 1);
            PlayerEquipment.PlayerShield = purchasedItem;
            // RemoveSlot(slot);
            UpdateShopSlots();
            UpdatePlayerShieldSlot();
            DeduceGold(purchasedItem.price);
            EventPurchaseConfirmed(purchasedItem);
        }
    }

    private void HandleRangedPurchase (RangedWeapon purchasedItem, ShopSlot slot)
    {
        if (PlayerEquipment.PlayerRanged != null)
        {
            EventRangedFull();
        }
        else
        {
            int slotIndex = GetSlotNumber(slot);
            sellerInventory.SubtractItemFromSlot(slotIndex - 1);
            PlayerEquipment.PlayerRanged = purchasedItem;
            // RemoveSlot(slot);
            UpdateShopSlots();
            UpdatePlayerRangedSlot();
            DeduceGold(purchasedItem.price);
            EventPurchaseConfirmed(purchasedItem);
        }
    }

    private void HandleItemSell (Consumable soldItem, ShopSlot slot)
    {
        int slotIndex = GetSlotNumber(slot);
        
        if (slot.SlotQuantity == 1)
        {
            RemoveSlot(slot);
        }
        else
        {
            slot.SlotQuantity -= 1;
            
        }
        AddGold(soldItem.price / 2);
        inventory.SubtractItemFromSlot(slotIndex - 1);
        sellerInventory.AddItem(soldItem);
        UpdatePlayerItemSlots();
        UpdateShopSlots();
        EventSellConfirmed();
    }

    private void HandleWeaponSell (Weapon soldItem, ShopSlot slot)
    {
        if (slot.SlotQuantity == 1)
        {
            RemoveSlot(slot);
        }
        else
        {
            slot.SlotQuantity -= 1;

        }
        AddGold(soldItem.price / 2);
        inventory.RemoveWeaponAtSlot(0);
        sellerInventory.AddItem(soldItem);
        UpdateShopSlots();
        UpdatePlayerWeaponSlots();
        EventSellConfirmed();
    }

    private void HandleShieldSell (Shield soldItem, ShopSlot slot)
    {
        equipment.PlayerShield = null;
        AddGold(soldItem.price / 2);
        sellerInventory.AddItem(soldItem);
        UpdateShopSlots();
        UpdatePlayerShieldSlot();
        EventSellConfirmed();
    }

    private void HandleRangedSell (RangedWeapon soldItem, ShopSlot slot)
    {
        equipment.PlayerRanged = null;
        AddGold(soldItem.price / 2);
        sellerInventory.AddItem(soldItem);
        UpdateShopSlots();
        UpdatePlayerRangedSlot();
        EventSellConfirmed();
    }


    private void RemoveSlot (ShopSlot slot)
    {
        slot.SlotItem = null;
        selectedSlot.Slot = null;
    }

    private void DeduceGold (int delta)
    {
        equipment.Gold -= delta;
        UserInterfaceController.instance.UpdateGoldCount(equipment.Gold, -delta);
    }

    private void AddGold (int delta)
    {
        equipment.Gold += delta;
        UserInterfaceController.instance.UpdateGoldCount(equipment.Gold, delta);
    }

    private int GetSlotNumber (ShopSlot slot)
    {
        string resultString = Regex.Match(slot.gameObject.name, @"\d+").Value;
        return (Int32.Parse(resultString));
    }

    public void CallShopClose ()
    {
        if (PlayerEquipment.PlayerRanged == null)
            UserInterfaceController.instance.HideShopMenu(false);
        else
            UserInterfaceController.instance.HideShopMenu(true);
    }

}
