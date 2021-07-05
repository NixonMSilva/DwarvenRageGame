using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [SerializeField] private Weapon playerWeapon = null;
    [SerializeField] private Shield playerShield = null;
    [SerializeField] private RangedWeapon playerRanged = null;

    [SerializeField] private GameObject weaponHUDObject;
    [SerializeField] private GameObject shieldHUDObject;
    private int gold = 0;

    private Animator anim;

    [SerializeField] private AnimatorOverrideController animationSet1H;
    [SerializeField] private AnimatorOverrideController animationSet2H;

    private AttackController attack;
    private BlockController block;
    private StatusController player;

    [SerializeField] private bool isTwoHanded = false;
    [SerializeField] private bool hasShield = false;

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
            UserInterfaceController.instance.UpdateGoldCount(gold);
        }
    }

    public Weapon PlayerWeapon
    {
        get { return playerWeapon; }
        set
        {
            if (playerWeapon.Equals(value) || playerWeapon == null)
            {
                playerWeapon = value;
            }
            else
            {
                playerWeapon.UnequipEffect(player);
                playerWeapon = value;
                playerWeapon.EquipEffect(player);
                attack.Damage = playerWeapon.damage;
            }
            
            anim.Play("weapon_down");

            /*
            ChangeWeaponGraphics(value);
            SetTwoHanded(value.isTwoHanded);

            // JERRYRIGGING: Have to do it twice so it unsuck
            ChangeWeaponGraphics(value); */
        }
    }

    public Shield PlayerShield
    {
        get { return playerShield; }
        set
        {
            hasShield = (value != null);
            anim.SetBool("hasShield", hasShield);
            ChangeShieldGraphics(value);
            playerShield = value;
            
        }
    }

    public RangedWeapon PlayerRanged
    {
        get { return playerRanged; }
        set
        {
            if (value != null)
                attack.RangedCooldown = value.cooldown;
              
            playerRanged = value;
            UpdateRangedFirePosition(value);
            ChangeRangedIcon(value);
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
    
    public bool HasShield
    {
        get { return hasShield; }
        set
        {
            hasShield = value;
            anim.SetBool("hasShield", value);
        }
    }

    private void Awake ()
    {
        anim = GetComponent<Animator>();
        attack = GetComponent<AttackController>();
        block = GetComponent<BlockController>();
        player = GetComponent<StatusController>();

        anim.runtimeAnimatorController = animationSet1H;

        // Set the correct shield status
        HasShield = (playerShield != null);

        // Set the effect of the starting weapon
        playerWeapon.EquipEffect(player);
    }

    private void Start ()
    {
        // Intialize weapon and shield graphics
        ChangeShieldGraphics(playerShield);
        ChangeWeaponGraphics(playerWeapon);

        // Set the correct animation set on start
        if (playerWeapon.isTwoHanded)
        {
            SetTwoHanded(true);
            isTwoHanded = true;
        }

        // Set the correct fire point for the ranged weapon
        UpdateRangedFirePosition(playerRanged);

        // Initializes the ranged weapon
        PlayerRanged = playerRanged;

        // Initializes the player damage to that of his weapon
        attack.Damage = playerWeapon.damage;

        // Initializes player gold
        UserInterfaceController.instance.UpdateGoldCount(gold);
    }

    public void ChangeWeaponGraphics (Weapon wpn)
    {
        Vector3 objScale = new Vector3(wpn.scaleX, wpn.scaleY, wpn.scaleZ);
        Vector3 objPosition = new Vector3(wpn.posX, wpn.posY, wpn.posZ);

        // Adjust the object position
        weaponHUDObject.transform.localPosition = objPosition;

        // Adjust the object scale
        weaponHUDObject.transform.localScale = objScale;

        // Adjust the mesh
        weaponHUDObject.GetComponent<MeshFilter>().mesh = wpn.worldMesh;
        weaponHUDObject.GetComponent<MeshRenderer>().materials = wpn.materialList;
    }

    private void ChangeShieldGraphics (Shield shd)
    {
        if (shd == null)
        {
            RemoveShieldGraphics();
            return;
        }

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

    private void RemoveShieldGraphics ()
    {
        // Makes the mesh invisible
        shieldHUDObject.GetComponent<MeshFilter>().mesh = null;
    }

    public void ChangeWeaponGraphics (RangedWeapon wpn)
    {
        Vector3 objScale = new Vector3(wpn.scaleX, wpn.scaleY, wpn.scaleZ);
        Vector3 objPosition = new Vector3(wpn.posX, wpn.posY, wpn.posZ);

        // Adjust the object position
        weaponHUDObject.transform.localPosition = objPosition;

        // Adjust the object scale
        weaponHUDObject.transform.localScale = objScale;

        // Adjust the mesh
        weaponHUDObject.GetComponent<MeshFilter>().mesh = wpn.worldMesh;
        weaponHUDObject.GetComponent<MeshRenderer>().materials = wpn.materialList;
    }

    public void ChangeRangedIcon (RangedWeapon wpn)
    {
        if (wpn != null)
        {
            UserInterfaceController.instance.ShowRangedSlot();
            UserInterfaceController.instance.UpdateRangedSlot(wpn.icon);
        }
        else
        {
            UserInterfaceController.instance.UpdateRangedSlot(null);
            UserInterfaceController.instance.HideRangedSlot();
            
        }
    }

    public void SetTwoHanded (bool status)
    {
        isTwoHanded = status;
        if (status)
        {
            anim.SetBool("isTwoHanded", true);
            anim.runtimeAnimatorController = animationSet2H;
            anim.SetBool("hasShield", HasShield);
            // Send shield to the inventory
            SetShieldGraphics(false);
        }
        else
        {
            anim.SetBool("isTwoHanded", false);
            anim.runtimeAnimatorController = animationSet1H;
            anim.SetBool("hasShield", HasShield);
            // Bring back the shield if applicable
            SetShieldGraphics(true);
        }
    }

    private void SetShieldGraphics (bool active)
    {
        shieldHUDObject.SetActive(active);
    }

    private void UpdateRangedFirePosition (RangedWeapon ranged)
    {
        if (ranged == null)
            return;

        attack.FirePointA.localPosition = new Vector3(ranged.firePos1X, ranged.firePos1Y, ranged.firePos1Z);
        attack.FirePointB.localPosition = new Vector3(ranged.firePos2X, ranged.firePos2Y, ranged.firePos2Z);
    }
}
