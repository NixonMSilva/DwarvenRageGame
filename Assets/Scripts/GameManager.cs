using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _dropTypes;
    [SerializeField] private List<Weapon> _weapons;
    [SerializeField] private List<Shield> _shields;

    private void Awake ()
    {
        //_weapons = Resources.LoadAll<Weapon>("Weapons").ToList();
        //_shields = Resources.LoadAll<Shield>("Shields").ToList();
    }

    public List<GameObject> GetDropTypes () => _dropTypes;

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

    public Item GetItemById (int id)
    {
        return Resources.LoadAll<Item>("Shield").Where(s => s.id == id).First();
    }
}
