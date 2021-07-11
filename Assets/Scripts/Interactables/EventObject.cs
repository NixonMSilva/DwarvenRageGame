using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventObject : MonoBehaviour
{
    public event Action<EventObject> OnExecution;

    [SerializeField] private EventType type;

    [SerializeField] public List<GameObject> _children = new List<GameObject>();

    public void HandleExecution ()
    {
        OnExecution?.Invoke(this);
    }

    public void DisableEvent ()
    {
        EventDisablingProcessor.DisableEvent(this.gameObject, type);
    }

}
