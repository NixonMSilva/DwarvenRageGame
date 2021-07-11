using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventObject : MonoBehaviour
{
    public event Action<EventObject> OnExecution;

    private bool isFired = false;

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

}
