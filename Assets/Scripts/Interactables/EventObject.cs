using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventObject : MonoBehaviour, IManageable
{
    public event Action<EventObject> OnExecution;

    public event Action<int> OnStatusChange;

    [SerializeField] private bool isFired = false;

    [SerializeField] private EventType type;

    [SerializeField] public List<GameObject> _children = new List<GameObject>();

    [SerializeField] private int _uniqueId;

    public bool IsFired
    {
        get => isFired;
        set => isFired = value;
    }
    public int UniqueId { get => _uniqueId; set => _uniqueId = value; }

    public GameObject AttachedObject => gameObject;

    public void HandleExecution ()
    {
        OnExecution?.Invoke(this);
        OnStatusChange?.Invoke(UniqueId);
        isFired = true;
    }

    public void DisableEvent ()
    {
        EventDisablingProcessor.DisableEvent(this.gameObject, type);
    }

    public void SetFired (bool value)
    {
        isFired = value;
    }

    public void HideModel ()
    {
        // Disable mesh
        MeshRenderer mesh = GetComponent<MeshRenderer>();
        mesh.enabled = false;
        
        // Disable colliders
        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
    }

    public void HideTooltip ()
    {
        Destroy(GetComponent<TooltipController>());
    }

    public void DestroyObject ()
    {
        // Doesn't destroy the event as you just disable it
        DisableEvent();
    }
}
