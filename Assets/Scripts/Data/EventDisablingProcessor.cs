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
                DisableChest(obj);
                break;
            case EventType.door:
                DisableDoor(obj);
                break;
            case EventType.item:
                break;
            case EventType.encounter:
                DisableEncounter(obj);
                break;
        }
    }

    private static void DisableChest (GameObject obj)
    {
        if (obj.TryGetComponent<ChestController>(out var chest))
        {
            chest.DestroyChest();
        }
        SetFired(obj);
    }

    private static void DisableDoor (GameObject obj)
    {
       SetFired(obj);
       if (obj.TryGetComponent<EventObject>(out var eventObj))
       {
           eventObj.HideModel();
       }
    }

    private static void DisableEncounter (GameObject obj)
    {
        // Invoke all events from the boss' death
        obj.GetComponent<BossFightController>().BossDisable();
    }

    private static void SetFired (GameObject obj)
    {
        if (obj.TryGetComponent<EventObject>(out var eventObj))
        {
            eventObj.SetFired(true);
            eventObj.HideTooltip();
            DisableParticles(obj);
        }
    }

    private static void DisableParticles (GameObject obj)
    {
        if (obj.TryGetComponent<ParticleSystem>(out var particles))
        {
            particles.Stop();
        }
    }
}
