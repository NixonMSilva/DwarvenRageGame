using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventDisablingProcessor
{
    public static void DisableEvent (GameObject obj, EventType type)
    {
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
