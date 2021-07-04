using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventObject : MonoBehaviour
{
    public event Action<EventObject> OnExecution;

    [SerializeField] public List<GameObject> _children = new List<GameObject>();

    private void Awake ()
    {

    }

    public void HandleExecution ()
    {
        OnExecution?.Invoke(this);
    }

}
