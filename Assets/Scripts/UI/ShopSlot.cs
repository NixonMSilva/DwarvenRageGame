using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ShopSlot : MonoBehaviour
{
    [SerializeField] private bool isBuy = true;

    [SerializeField] private ShopInterface shopUI;
    [SerializeField] private ShopSelectedSlot shopSelectedSlot;

    [SerializeField] private Item slotItem;
    [SerializeField] private int slotQuantity;

    [SerializeField] private Image slotItemSprite;
    [SerializeField] private TextMeshProUGUI slotQuantityText;

    public Item SlotItem
    {
        get { return slotItem; }
        set
        {
            if (value != null)
            {
                //Debug.Log(value);
                slotItem = value;
                slotItemSprite.sprite = value.icon;
                slotItemSprite.color = Color.white;
            }
            else
            {
                slotItem = value;
                slotItemSprite.sprite = null;
                slotItemSprite.color = Color.clear;
                SlotQuantity = 0;
            }
        }
    }

    public int SlotQuantity
    {
        get { return slotQuantity; }
        set
        {
            if (slotItem != null)
            {
                slotQuantity = value;
                if (slotQuantity > 1)
                {
                    slotQuantityText.text = "x" + value.ToString();
                }
                else
                {
                    slotQuantityText.text = "";
                }
            }
            else
            {
                slotQuantityText.text = "";
            }
        }
    }

    public bool IsBuy
    {
        get { return isBuy; }
    }

    private void Awake ()
    {
        EmptySlot();
    }

    public void SelectSlot ()
    {
        shopSelectedSlot.Slot = this;
    }

    public void SellSlot ()
    {
        if (slotItem != null)
        {
            shopUI.EventShopSell(this);
        }
        
    }

    public void BuySlot ()
    {
        if (slotItem != null)
        {
            // Buy the item
            if (slotItem.price <= shopUI.PlayerEquipment.Gold)
            {
                shopUI.EventShopPurchase(this);
            }
            else
            {
                shopUI.EventNotEnoughCash();
            }
        }
    }

    public void EmptySlot ()
    {
        slotItemSprite.sprite = null;
        slotItemSprite.color = new Color (1f, 1f, 1f, 0f);
        SlotQuantity = 0;
    }
}
