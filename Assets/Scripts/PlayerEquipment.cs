using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [SerializeField] private Weapon playerWeapon = null;
    [SerializeField] private Shield playerShield = null;
    [SerializeField] private Inventory inventory = null;

    [SerializeField] private GameObject weaponHUDObject;
    [SerializeField] private GameObject shieldHUDObject;

    private int gold = 0;

    private Animator anim;

    [SerializeField] private AnimatorOverrideController animationSet1H;
    [SerializeField] private AnimatorOverrideController animationSet2H;

    private AttackController attack;
    private BlockController block;

    [SerializeField] private bool isTwoHanded = false;

    [SerializeField] private float baseDamageReduction = 0.15f;

    public int Gold
    {
        get { return gold; }
        set
        {
            gold = value;
            if (gold < 0)
            {
                gold = 0;
            }
        }
    }

    public Weapon PlayerWeapon
    {
        get { return playerWeapon; }
        set
        {
            playerWeapon = value;
            attack.Damage = playerWeapon.damage;
            ChangeWeaponGraphics(value);
            SetTwoHanded(value.isTwoHanded);
            /*
            if (isTwoHanded)
            {
                anim.SetBool("isTwoHanded", true);
                //block.CanBlock = false;
                anim.runtimeAnimatorController = animationSet2H;
                // Send shield to the inventory
                SetShieldGraphics(false);
            }
            else
            {
                anim.SetBool("isTwoHanded", false);
                //block.CanBlock = true;
                anim.runtimeAnimatorController = animationSet1H;
                // Bring back the shield if applicable
                SetShieldGraphics(true);
            }
            */
            // JERRYRIGGING: Have to do it twice so it unsuck
            ChangeWeaponGraphics(value);
        }
    }

    public Shield PlayerShield
    {
        get { return playerShield; }
        set
        {
            playerShield = value;
            ChangeShieldGraphics(value);
        }
    }

    public bool IsTwoHanded
    {
        get { return isTwoHanded; }
    }

    public float BaseDamageReduction
    {
        get { return baseDamageReduction; }
    }

    private void Awake ()
    {
        anim = GetComponent<Animator>();
        attack = GetComponent<AttackController>();
        block = GetComponent<BlockController>();

        anim.runtimeAnimatorController = animationSet1H;
    }

    private void Start ()
    {
        ChangeShieldGraphics(playerShield);
        ChangeWeaponGraphics(playerWeapon);

        if (playerWeapon.isTwoHanded)
        {
            SetTwoHanded(true);
            isTwoHanded = true;
        }
    }

    private void ChangeWeaponGraphics (Weapon wpn)
    {
        Vector3 objScale = new Vector3(wpn.scaleX, wpn.scaleY, wpn.scaleZ);
        Vector3 objPosition = new Vector3(wpn.posX, wpn.posY, wpn.posZ);

        // Adjust the object scale
        weaponHUDObject.transform.localScale = objScale;

        /*
        Debug.Log("Antes: " + weaponHUDObject.transform.localPosition.x + " | " +
            weaponHUDObject.transform.localPosition.y + " | " +
            weaponHUDObject.transform.localPosition.z); */

        // Adjust the object position
        weaponHUDObject.transform.localPosition = objPosition;

        /*
        Debug.Log("Depois: " + weaponHUDObject.transform.localPosition.x + " | " +
            weaponHUDObject.transform.localPosition.y + " | " +
            weaponHUDObject.transform.localPosition.z); */

        // Adjust the mesh
        weaponHUDObject.GetComponent<MeshFilter>().mesh = wpn.worldMesh;
        weaponHUDObject.GetComponent<MeshRenderer>().materials = wpn.materialList;
    }

    private void ChangeShieldGraphics (Shield shd)
    {
        Vector3 objScale = new Vector3(shd.scaleX, shd.scaleY, shd.scaleZ);
        Vector3 objRotation = new Vector3(shd.rotX, shd.rotY, shd.rotZ);

        // Adjust the object scale
        shieldHUDObject.transform.localScale = objScale;

        // Adjust the object rotation
        shieldHUDObject.transform.localEulerAngles = objRotation;

        // Adjust the mesh
        shieldHUDObject.GetComponent<MeshFilter>().mesh = shd.worldMesh;
        shieldHUDObject.GetComponent<MeshRenderer>().materials = shd.materialList;
    }

    private void SetTwoHanded (bool status)
    {
        isTwoHanded = status;
        if (status)
        {
            anim.SetBool("isTwoHanded", true);
            anim.runtimeAnimatorController = animationSet2H;
            // Send shield to the inventory
            SetShieldGraphics(false);
        }
        else
        {
            anim.SetBool("isTwoHanded", false);
            anim.runtimeAnimatorController = animationSet1H;
            // Bring back the shield if applicable
            SetShieldGraphics(true);
        }
    }

    private void SetShieldGraphics (bool active)
    {
        shieldHUDObject.SetActive(active);
    }
}
