using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

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
    public bool[][] saveEventStatus;

    public int saveWeapon;

    public int saveShield;

    public int saveRanged;

    public int saveGold;

    public List<int> weaponsId;

    public List<int> itemsId;
    public List<int> itemsStack;

    private GameObject player;
    
    bool hasPlayer = false;  

    [SerializeField] private bool canLoadGame = true;

    [SerializeField] private Resistance defaultResistances;

    public GameObject Player
    {
        get => player;

        set
        {
            player = value;
            
            if (player != null)
                hasPlayer = true;
        }
    }
    
    private void Awake () 
    {
        player = GameObject.Find("Player");

        if (player)
        {
            hasPlayer = true;
        }

        savePickableStatus = new bool[maxScenes][];
        saveEnemyStatus = new bool[maxScenes][];
        saveEventStatus = new bool[maxScenes][];

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
            EventController eventController = GameObject.Find("Events").GetComponent<EventController>();

            sceneNumber = SceneManager.GetActiveScene().buildIndex;

            if (pickableController)
                savePickableStatus[sceneNumber] = pickableController.GetPickedList();

            if (enemyController)
                saveEnemyStatus[sceneNumber] = enemyController.GetKilledList();

            if (eventController)
                saveEventStatus[sceneNumber] = eventController.GetTriggeredList();

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
            EventController eventController = GameObject.Find("Events").GetComponent<EventController>();

            sceneNumber = GameManager.instance.GetSceneNumber();

            pickableController?.SetPickedList(data.pickupStatus[sceneNumber]);

            enemyController?.SetKilledList(data.enemyStatus[sceneNumber]);

            eventController?.SetTriggeredList(data.eventStatus[sceneNumber]);
        }
    }

    public PlayerData LoadGameData ()
    {
        GameData data = SaveSystem.LoadPlayer();
        PlayerData loadingPlayer = new PlayerData();
        
        if (data == null)
            return null;

        loadingPlayer.sceneIndex = data.sceneNumber;

        loadingPlayer.posX = data.position[0];
        loadingPlayer.posY = data.position[1];
        loadingPlayer.posZ = data.position[2];

        loadingPlayer.health = data.health;
        loadingPlayer.armor = data.armor;

        loadingPlayer.gold = data.gold;
        
        // Get all equipped items
        loadingPlayer.playerWeapon = GetWeapon(data.weapon);
        loadingPlayer.playerRanged = GetRanged(data.ranged);
        loadingPlayer.playerShield = GetShield(data.shield);

        // Get all weapons
        loadingPlayer.weaponList.Clear();
        for (int i = 0; i < data.weaponsId.Length; ++i)
        {
            loadingPlayer.weaponList.Add(GetWeapon(data.weaponsId[i]));
            Debug.Log(loadingPlayer.weaponList[i].itemName);
        }

        // Get all items
        loadingPlayer.itemList.Clear();
        loadingPlayer.itemStack.Clear();
        for (int i = 0; i < data.itemId.Length; ++i)
        {
            Consumable newItem = GetItem(data.itemId[i]);
            loadingPlayer.itemList.Add(newItem);
            loadingPlayer.itemStack.Add(data.itemStack[i]);
        }

        // Get all scene statuses
        loadingPlayer.pickableStatus = data.pickupStatus[data.sceneNumber];
        loadingPlayer.enemyStatus = data.enemyStatus[data.sceneNumber];
        loadingPlayer.eventStatus = data.eventStatus[data.sceneNumber];
        
        // Get the resistance
        loadingPlayer.sheet = defaultResistances;

        return loadingPlayer;
    }

    private Weapon GetWeapon (int id)
    {
        if (id > 0)
            return GameManager.instance.GetWeaponById(id);
        else
            return null;
    }

    private Shield GetShield (int id)
    {
        if (id > 0)
            return GameManager.instance.GetShieldById(id);
        else
            return null;
    }

    private RangedWeapon GetRanged (int id)
    {
        if (id > 0)
            return GameManager.instance.GetRangedById(id);
        else
            return null;
    }

    private Consumable GetItem (int id)
    {
        if (id > 0)
            return GameManager.instance.GetItemById(id);
        else
            return null;
    }
}
