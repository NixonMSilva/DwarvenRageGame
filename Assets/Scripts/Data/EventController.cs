using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventController : MonoBehaviour
{
    [SerializeField] private List<EventObject> _eventList;

    [SerializeField] private List<bool> _wasNotTriggered;

    private char[] limits = { '(', ')' };

    private void Awake ()
    {
        for (int i = 0; i < _eventList.Count; ++i)
        {
            _wasNotTriggered.Add(true);
        }

        foreach (EventObject eventObj in _eventList)
        {
            eventObj.OnExecution += HandleExecution;
        }
    }

    private void HandleExecution (EventObject sender)
    {
        
    }
    public bool[] GetTriggeredList ()
    {
        return _wasNotTriggered.ToArray();
    }

    public void SetTriggeredList (bool[] arr)
    {
        // If the array is null then do nothing
        if (arr == null)
            return;

        _wasNotTriggered = arr.ToList();
        for (int i = 0; i < _wasNotTriggered.Count; ++i)
        {
            if (!_wasNotTriggered[i])
            {
                Destroy(_eventList[i].gameObject);
            }
        }
    }

}
