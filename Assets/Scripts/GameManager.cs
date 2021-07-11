using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using System.Linq;
using JetBrains.Annotations;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Weapon> _weapons;
    [SerializeField] private List<Shield> _shields;
    public static GameManager instance;
    [SerializeField] private int sceneNumber = 1;
    
    // Shop settings
    public int timesShopped = 0;

    // Game settings
    public float musicVolume = 1f;
    public float sfxVolume = 1f;

    // Player data
    [CanBeNull] public PlayerData playerData = null;

    public PlayerData Player
    {
        get => playerData;
        set => playerData = value;
    }

    // Input handler
    private InputHandler currentInput = null;

    public void SaveCurrentSceneStatus()
    {   
        var player = GameObject.Find("Player");
        var playerEquipment = player.GetComponent<PlayerEquipment>();
        var playerStatus = player.GetComponent<PlayerStatus>();
        var playerInventory = player.GetComponent<Inventory>();

        Player = new PlayerData(playerStatus, playerEquipment, playerInventory);
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
