using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChestController : MonoBehaviour
{
    [SerializeField] private TooltipController tooltip;
    [SerializeField] private Animation animation;
    [SerializeField] private UnityEvent onOpen;
    
    public void OpenChest ()
    {
        animation.Stop();
        animation.Play();
    }

    public void ChestEffect()
    {
        onOpen.Invoke();
    }

    public void AddGold (int amount)
    {
        PlayerEquipment equipment = GameObject.Find("Player").GetComponent<PlayerEquipment>();
        equipment.Gold += amount;
        UserInterfaceController.instance.PlayGoldAnimation(amount);
    }

    public void AddItem (Consumable item)
    {
        Inventory inventory = GameObject.Find("Player").GetComponent<Inventory>();
        inventory.AddItem(item);
        UserInterfaceController.instance.ThrowWarningMessage(item.itemName + "acquired!");
    }
    
    public void AddWeapon (Weapon weapon)
    {
        Inventory inventory = GameObject.Find("Player").GetComponent<Inventory>();
        inventory.AddWeapon(weapon);
        UserInterfaceController.instance.ThrowWarningMessage(weapon.itemName + "acquired!");
    }

    public void PlayChestSound()
    {
        AudioManager.instance.PlaySoundAt(gameObject, "chest_open");
    }
}