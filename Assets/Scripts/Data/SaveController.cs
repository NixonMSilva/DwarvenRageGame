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

    public int sceneNumber;
    public int maxScenes = 50;

    public bool[][] savePickableStatus;
    public bool[][] saveEnemyStatus;

    public int saveWeapon;

    public int saveShield;

    public int saveRanged;

    public int saveGold;

    public List<int> weaponsId;

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

        savePickableStatus = new bool[maxScenes][];
        saveEnemyStatus = new bool[maxScenes][];

        itemsId = new List<int>();
        itemsStack = new List<int>();
        
    }
    
    public void SaveGame ()
    {
        if (hasPlayer)
        {
            StatusController playerStatus = player.GetComponent<StatusController>();

            savePosition = player.transform.position;
            saveHealth = playerStatus.Health;
            saveMaxHealth = playerStatus.MaxHealth;
            saveArmor = playerStatus.Armor;

            PlayerEquipment playerEquipment = player.GetComponent<PlayerEquipment>();

            saveWeapon = playerEquipment.PlayerWeapon.id;

            if (playerEquipment.PlayerShield)
                saveShield = playerEquipment.PlayerShield.id;
            else
                saveShield = 0;

            if (playerEquipment.PlayerRanged)
                saveRanged = playerEquipment.PlayerRanged.id;
            else
                saveRanged = 0;

            saveGold = playerEquipment.Gold;

            Inventory playerInventory = player.GetComponent<Inventory>();

            weaponsId = playerInventory.GetWeaponList();

            itemsId = playerInventory.GetItemList();
            itemsStack = playerInventory.GetItemStacks();

            PickableController pickableController = GameObject.Find("Pickables").GetComponent<PickableController>();
            EnemySpawnManager enemyController = GameObject.Find("Enemies").GetComponent<EnemySpawnManager>();

            sceneNumber = manager.GetSceneNumber();
            savePickableStatus[sceneNumber] = pickableController.GetPickedList();
            saveEnemyStatus[sceneNumber] = enemyController.GetKilledList();

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

            Weapon equippedWeapon = GetWeapon(data.weapon);
            playerEquipment.PlayerWeapon = equippedWeapon;

            Shield equippedShield = GetShield(data.shield);
            playerEquipment.PlayerShield = equippedShield;

            RangedWeapon equippedRanged = GetRanged(data.ranged);
            playerEquipment.PlayerRanged = equippedRanged;

            playerEquipment.Gold = data.gold;

            Inventory playerInventory = player.GetComponent<Inventory>();

            playerInventory.ClearWeapons();
            for (int i = 0; i < data.weaponsId.Length; ++i)
            {
                playerInventory.AddWeapon(GetWeapon(data.weaponsId[i]));
            }

            playerInventory.ClearItems();
            for (int i = 0; i < data.itemId.Length; ++i)
            {
                playerInventory.AddItem(GetItem(data.itemId[i]), data.itemStack[i]);
            }

            PickableController pickableController = GameObject.Find("Pickables").GetComponent<PickableController>();
            EnemySpawnManager enemyController = GameObject.Find("Enemies").GetComponent<EnemySpawnManager>();

            sceneNumber = manager.GetSceneNumber();

            pickableController.SetPickedList(data.pickupStatus[sceneNumber]);
            enemyController.SetKilledList(data.enemyStatus[sceneNumber]);
        }
    }

    private Weapon GetWeapon (int id)
    {
        if (id > 0)
            return manager.GetWeaponById(id);
        else
            return null;
    }

    private Shield GetShield (int id)
    {
        if (id > 0)
            return manager.GetShieldById(id);
        else
            return null;
    }

    private RangedWeapon GetRanged (int id)
    {
        if (id > 0)
            return manager.GetRangedById(id);
        else
            return null;
    }

    private Consumable GetItem (int id)
    {
        if (id > 0)
            return manager.GetItemById(id);
        else
            return null;
    }
}
