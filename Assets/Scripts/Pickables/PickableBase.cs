using UnityEngine;
using UnityEngine.Events;
using System;

public class PickableBase : MonoBehaviour, IPickable, IManageable
{
    [SerializeField] private float rotationRate = 5f;

    public event Action<IPickable> OnPickUp;

    public event Action<int> OnStatusChange;

    [SerializeField] private UnityEvent OnPickUpUnity;

    [SerializeField] public Consumable item;

    [SerializeField] private int _uniqueId;

    public int UniqueId
    {
        get { return _uniqueId; }
        set { _uniqueId = value; }
    }

    public GameObject AttachedObject => gameObject;

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
                    UserInterfaceController.instance.ShowDamagePanel(Color.white);
                    AudioManager.instance.PlaySound(item.audioPickupName);
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
            default:
                throw new ArgumentException();
        }

        return true;
    }

    public virtual void ApplyEffect (StatusController target)
    {
        item.Use(target);
    }

    public virtual void HandlePickUp ()
    {
        OnPickUp?.Invoke(this);
        OnPickUpUnity?.Invoke();
        OnStatusChange?.Invoke(UniqueId);
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

    public void DestroyObject ()
    {
        Destroy(gameObject);
    }
}
