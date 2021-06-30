using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    // Data to be saved
    public Vector3 savePosition;

    public float saveHealth;

    public float saveMaxHealth;

    public float saveArmor;

    public bool[] savePickableStatus;

    public int saveWeapon;

    public int saveShield;

    public int saveGold;

    public List<int> itemsId;
    public List<int> itemsStack;

    private GameObject player;

    private GameManager manager;

    bool hasPlayer = false;  

    [SerializeField] private bool canLoadGame = true;

    // Start is called before the first frame update

    private void Start() 
    {
        if (canLoadGame)
            LoadGame();
    }

    private void Awake() 
    {
        player = GameObject.Find("Player");
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (player)
        {
            hasPlayer = true;
        }

        itemsId = new List<int>();
        itemsStack = new List<int>();
        
    }
    
    public void SaveGame ()
    {
        if(hasPlayer)
        {
            StatusController playerStatus = player.GetComponent<StatusController>();

            savePosition = player.transform.position;
            saveHealth = playerStatus.Health;
            saveMaxHealth = playerStatus.MaxHealth;
            saveArmor = playerStatus.Armor;

            PlayerEquipment playerEquipment = player.GetComponent<PlayerEquipment>();

            saveWeapon = playerEquipment.PlayerWeapon.id;
            saveShield = playerEquipment.PlayerShield.id;
            saveGold = playerEquipment.Gold;

            Inventory playerInventory = player.GetComponent<Inventory>();

            itemsId = playerInventory.GetItemList();
            itemsStack = playerInventory.GetItemStacks();

            PickableController pickableController = GameObject.Find("Pickables").GetComponent<PickableController>();

            savePickableStatus = pickableController.GetPickedList();

            SaveSystem.Save(this);
        }
    }

    public void LoadGame ()
    {
        GameData data = SaveSystem.LoadPlayer();
       
        if (data != null)
        {
            Vector3 position;
            position.x = data.position[0];
            position.y = data.position[1];
            position.z = data.position[2];

            player.transform.position = position;

            StatusController playerStatus = player.GetComponent<StatusController>();

            playerStatus.Health = data.health;
            playerStatus.MaxHealth = data.maxHealth;
            playerStatus.Armor = data.armor;

            PlayerEquipment playerEquipment = player.GetComponent<PlayerEquipment>();

            playerEquipment.PlayerWeapon = GetWeapon(data.weapon);
            playerEquipment.PlayerShield = GetShield(data.shield);
            playerEquipment.Gold = data.gold;

            Inventory playerInventory = player.GetComponent<Inventory>();

            for (int i = 0; i < data.itemId.Length; ++i)
            {
                playerInventory.AddItem(GetItem(data.itemId[i]), data.itemStack[i]);
            }

            PickableController pickableController = GameObject.Find("Pickables").GetComponent<PickableController>();

            pickableController.SetPickedList(data.pickupStatus);
        }
    }

    private Weapon GetWeapon (int id)
    {
        return manager.GetWeaponById(id);
    }

    private Shield GetShield (int id)
    {
        return manager.GetShieldById(id);
    }

    private Consumable GetItem (int id)
    {
        return manager.GetItemById(id);
    }
}
