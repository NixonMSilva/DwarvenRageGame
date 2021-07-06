using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ShopSelectedSlot : MonoBehaviour
{
    [SerializeField] private ShopSlot slot;

    [SerializeField] private Item item;

    private GameObject slotIcon;
    private GameObject slotName;
    private GameObject slotDescription;
    private GameObject slotPriceLabel;
    private GameObject slotPriceValue;
    private GameObject slotBuyButton;
    private GameObject slotSellButton;

    private Image slotIconImage;

    private TextMeshProUGUI slotNameText;
    private TextMeshProUGUI slotDescriptionText;
    private TextMeshProUGUI slotPriceValueText;

    public ShopSlot Slot
    {
        get { return slot; }
        set 
        {
            if (value != null)
            {
                slot = value;
                UpdateInformation();
                if (value.SlotItem != null)
                {
                    ShowAllInformation();
                }
                else
                {
                    HideAllInformation();
                }
                    
            }
            else
            {
                slot = value;
                UpdateInformation();
                HideAllInformation();
            }
        }
    }

    private void Awake ()
    {
        slotIcon = GameObject.Find("PanelItemIcon");
        slotIconImage = GameObject.Find("PanelItemIconImage").GetComponent<Image>();

        slotName = GameObject.Find("ItemNameText");
        slotNameText = slotName.GetComponent<TextMeshProUGUI>();

        slotDescription = GameObject.Find("ItemDescText");
        slotDescriptionText = slotDescription.GetComponent<TextMeshProUGUI>();

        slotPriceLabel = GameObject.Find("ItemPriceTextLabel");

        slotPriceValue = GameObject.Find("ItemPriceTextValue");
        slotPriceValueText = slotPriceValue.GetComponent<TextMeshProUGUI>();

        slotBuyButton = GameObject.Find("ButtonBuy");
        slotSellButton = GameObject.Find("ButtonSell");

        HideAllInformation();
    }

    private void ShowAllInformation ()
    {
        UpdateInformation();
        ShowSlotIcon();
        ShowSlotName();
        ShowSlotDescription();
        ShowSlotPrice();
        if (slot == null)
        {
            HideBuyButton();
            HideSellButton();
        }
        else
        {
            if (slot.IsBuy)
            {
                ShowBuyButton();
            }
            else
            {
                ShowSellButton();
            }
        }
        
    }

    private void HideAllInformation ()
    {
        HideSlotIcon();
        HideSlotName();
        HideSlotDescription();
        HideSlotPrice();
        HideBuyButton();
        HideSellButton();
    }

    private void UpdateInformation ()
    {
        // If the last item of the stack was bought / sold
        if (slot == null)
        {
            HideAllInformation();
            return;
        }

        // If the slot is empty
        if (slot.SlotItem == null)
        {
            slotIconImage.sprite = null;
            slotIconImage.color = Color.white;
            slotNameText.text = "";
            slotDescriptionText.text = "";
            HideAllInformation();
        }
        else
        {
            slotIconImage.sprite = slot.SlotItem.icon;
            slotNameText.text = slot.SlotItem.itemName;
            slotDescriptionText.text = slot.SlotItem.itemDescription;

            // Set values in accordance to purchase or selling
            if (slot.IsBuy)
            {
                slotPriceValueText.text = slot.SlotItem.price.ToString();
            }
            else
            {
                int sellValue = (int)(slot.SlotItem.price * 0.5f);
                slotPriceValueText.text = sellValue.ToString();
            }
        }
    }

    private void ShowSlotPrice ()
    {
        slotPriceLabel.SetActive(true);
        slotPriceValue.SetActive(true);
    }

    private void ShowSlotDescription ()
    {
        slotDescription.SetActive(true);
    }

    private void ShowSlotName ()
    {
        slotName.SetActive(true);
    }

    private void ShowSlotIcon ()
    {
        slotIcon.SetActive(true);
    }

    private void HideSellButton ()
    {
        slotSellButton.SetActive(false);
    }

    private void HideBuyButton ()
    {
        slotBuyButton.SetActive(false);
    }

    private void HideSlotPrice ()
    {
        slotPriceLabel.SetActive(false);
        slotPriceValue.SetActive(false);
    }

    private void HideSlotDescription ()
    {
        slotDescription.SetActive(false);
    }

    private void HideSlotName ()
    {
        slotName.SetActive(false);
    }

    private void HideSlotIcon ()
    {
        slotIcon.SetActive(false);
    }

    public void ConfirmPurchase ()
    {
        if (!slot.IsBuy)
        {
            Debug.LogWarning("Slot is not set as buy!");
            return;
        }

        if (slot.SlotItem == null)
            return;

        slot.BuySlot();
        UpdateInformation();
    }

    public void ConfirmSell ()
    {
        if (slot.IsBuy)
        {
            Debug.LogWarning("Slot is not set as sell!");
            return;
        }

        if (slot.SlotItem == null)
            return;

        slot.SellSlot();
        UpdateInformation();
    }

    public void ShowBuyButton ()
    {
        slotBuyButton.SetActive(true);
        slotSellButton.SetActive(false);
    }

    public void ShowSellButton ()
    {
        slotSellButton.SetActive(true);
        slotBuyButton.SetActive(false);
    }


}
