using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [SerializeField] private List<Weapon> _weapons;
    [SerializeField] private List<Shield> _shields;
    
    [SerializeField] private int sceneNumber = 1;
    
    // Shop settings
    public int timesShopped = 0;

    // Game settings
    public float musicVolume = 1f;
    public float sfxVolume = 1f;

    // Player data
    public PlayerData? playerData = null;
    [SerializeField] private PlayerData defaultPlayerData;
    
    // Event data
    public PickableController pickables;
    public EnemySpawnManager enemies;
    public EventController events;
    
    // Saving data
    [SerializeField] private SaveController save;
    [SerializeField] private SceneLoader loader;

    public bool isLoadingGame = false;

    public PlayerData Player
    {
        get => playerData;
        set => playerData = value;
    }

    // Input handler
    private InputHandler currentInput = null;

    public void SaveCurrentSceneStatus ()
    {   
        var player = GameObject.Find("Player");
        var playerEquipment = player.GetComponent<PlayerEquipment>();
        var playerStatus = player.GetComponent<PlayerStatus>();
        var playerInventory = player.GetComponent<Inventory>();

        Player = new PlayerData(player.transform, playerStatus, playerEquipment, playerInventory);
    }

    public void PurgeData ()
    {
        playerData = defaultPlayerData;
    }

    public void SaveGame ()
    {
        save.Player = GameObject.Find("Player");
        save.SaveGame();
    }

    public void LoadGame ()
    {
        SceneManager.sceneLoaded += ProcessLoad;
        
        isLoadingGame = true;
        
        PlayerData newPlayer = save.LoadGameData();

        if (newPlayer != null)
            Player = newPlayer;
        
        loader.LoadSceneWithoutData (Player.sceneIndex);
    }

    private void ProcessLoad (Scene scene, LoadSceneMode mode)
    {
        if (!isLoadingGame)
        {
            Debug.Log("Not a load operation!");
            return;
        }

        ValidateSceneData ();

        isLoadingGame = false;
        SceneManager.sceneLoaded -= ProcessLoad;
    }

    private void ValidateSceneData ()
    {
        // Validate pickable / enemies / event data
        pickables = GameObject.Find("Pickables").GetComponent<PickableController>();
        enemies = GameObject.Find("Enemies").GetComponent<EnemySpawnManager>();
        events = GameObject.Find("Events").GetComponent<EventController>();
        
        if (pickables != null)
            pickables.SetPickedList(playerData.pickableStatus);
        
        if (enemies != null)
            enemies.SetKilledList(playerData.enemyStatus);
        
        if (events != null)
            events.SetTriggeredList(playerData.eventStatus);

        /*
        Vector3 savedPos = new Vector3(playerData.posX, playerData.posY, playerData.posZ);
        
        GameObject playerObj = GameObject.Find("Player");
        
        playerObj.transform.position = savedPos;
        
        playerObj.GetComponent<PlayerMovement>().SetExactPosition(savedPos) */;

        // Hyper jerryrigging
        Invoke(nameof(MovePlayer), 0.1f);
    }

    private void MovePlayer ()
    {
        Vector3 savedPos = new Vector3(playerData.posX, playerData.posY, playerData.posZ);
        
        GameObject playerObj = GameObject.Find("Player");
        
        playerObj.GetComponent<PlayerMovement>().SetExactPosition(savedPos);
        
        /*
        // Move the player to the correct position
        Vector3 savedPos = new Vector3(playerData.posX, playerData.posY, playerData.posZ);
        
        GameObject playerObj = GameObject.Find("Player");
        
        playerObj.transform.position = savedPos;
        
        Debug.Log("Saved position: " + savedPos + " | Curr pos: " + playerObj.transform.position);
        */
        
        isLoadingGame = false;
    }

    public int GetSceneNumber () => sceneNumber;

    public List<Weapon> GetWeapons () => _weapons;

    public List<Shield> GetShields () => _shields;

    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        save = GetComponent<SaveController>();
        loader = GetComponent<SceneLoader>();
    }

    public Weapon GetWeaponById (int id)
    {
        return Resources.LoadAll<Weapon>("Weapons").Where(w => w.id == id).First();
    }

    public Shield GetShieldById (int id)
    {
        return Resources.LoadAll<Shield>("Shields").Where(s => s.id == id)?.First();
    }

    public Consumable GetItemById (int id)
    {
        return Resources.LoadAll<Consumable>("Consumables").Where(i => i.id == id).First();
    }

    public RangedWeapon GetRangedById (int id)
    {
        return Resources.LoadAll<RangedWeapon>("RangedWeapons").Where(r => r.id == id)?.First();
    }

    public void SetCurrentScene (int id)
    {
        sceneNumber = id;
    }
}
