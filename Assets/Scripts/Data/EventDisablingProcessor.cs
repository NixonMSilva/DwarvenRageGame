using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventDisablingProcessor
{
    public static void DisableEvent (GameObject obj, EventType type)
    {
        // Set the event object as "fired"
        EventObject eventObject = obj.GetComponent<EventObject>();
        eventObject.IsFired = true;
        
        // Handles the deactivation of the event trigger
        // for each different type of event
        switch (type)
        {
            case EventType.chest:
                Object.Destroy(obj.GetComponent<ChestController>());
                break;
            case EventType.door:
                break;
            case EventType.item:
                break;
        }
    }
}
