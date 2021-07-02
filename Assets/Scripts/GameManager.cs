using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Weapon> _weapons;
    [SerializeField] private List<Shield> _shields;

    [SerializeField] private int sceneNumber = 1;

    private void Awake ()
    {
        //_weapons = Resources.LoadAll<Weapon>("Weapons").ToList();
        //_shields = Resources.LoadAll<Shield>("Shields").ToList();
    }

    public int GetSceneNumber () => sceneNumber;

    public List<Weapon> GetWeapons () => _weapons;

    public List<Shield> GetShields () => _shields;

    public Weapon GetWeaponById (int id)
    {
        return Resources.LoadAll<Weapon>("Weapons").Where(w => w.id == id).First();
        //return _weapons.Where(w => w.id == id).First();
    }

    public Shield GetShieldById (int id)
    {
        return Resources.LoadAll<Shield>("Shields").Where(s => s.id == id).First();
        //return _shields.Where(s => s.id == id).First();
    }

    public Consumable GetItemById (int id)
    {
        return Resources.LoadAll<Consumable>("Consumables").Where(s => s.id == id).First();
    }
}
