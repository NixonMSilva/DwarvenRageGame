using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PickableBase : MonoBehaviour, IPickable
{
    [SerializeField] private float rotationRate = 5f;

    public event Action<IPickable> OnPickUp;

    [SerializeField] public Consumable item;

    private void Awake ()
    {

    }

    private void Update ()
    {
        Rotate();
    }

    public void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StatusController playerStatus;
            other.TryGetComponent<StatusController>(out playerStatus);
            if (CanPick(item.effect, playerStatus))
            {
                if (playerStatus)
                {
                    ApplyEffect(playerStatus);
                    HandlePickUp();
                }
            }
        }
    }

    private bool CanPick (Effect itemEffect, StatusController target)
    {
        if (target == null)
            return false;
        
        switch (itemEffect.type)
        {
            case EffectType.heal:
                if (target.Health >= target.MaxHealth)
                    return false;
                break;
            case EffectType.healArmor:
                if (target.Armor >= target.MaxArmor)
                    return false;
                break;
            case EffectType.berserk:
                break;
            case EffectType.fortune:
                break;
            case EffectType.fireResistance:
                break;
            case EffectType.poisonResistance:
                break;
            case EffectType.addMaxHealth:
                break;
            case EffectType.addMaxArmor:
                break;
            case EffectType.poison:
                break;
            case EffectType.burning:
                break;
            case EffectType.none:
                break;
            default:
                throw new ArgumentException();
        }

        return true;
    }

    public virtual void ApplyEffect (StatusController target)
    {
        item.Use(target);
    }

    public void HandlePickUp ()
    {
        OnPickUp?.Invoke(this);
        Destroy(gameObject);
    }

    private void Rotate ()
    {
        Vector3 currentAngle = transform.rotation.eulerAngles;
        Quaternion newAngle = Quaternion.identity;
        newAngle.eulerAngles = new Vector3(currentAngle.x, currentAngle.y + (rotationRate * Time.deltaTime), currentAngle.z);
        transform.rotation = newAngle;
    }

    public string GetName () => gameObject.name;
    
        

}
