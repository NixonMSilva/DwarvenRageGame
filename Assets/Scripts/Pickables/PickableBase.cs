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
            if (playerStatus)
            {
                ApplyEffect(playerStatus);
                HandlePickUp();
            }
        }
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
