using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventObject : MonoBehaviour
{
    public event Action<EventObject> OnExecution;

    [SerializeField] private bool isFired = false;

    [SerializeField] private EventType type;

    [SerializeField] public List<GameObject> _children = new List<GameObject>();

    public bool IsFired
    {
        get => isFired;
        set => isFired = value;
    }

    public void HandleExecution ()
    {
        OnExecution?.Invoke(this);
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

}
