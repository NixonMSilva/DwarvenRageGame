using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownController : MonoBehaviour
{
    // Action declaration
    private Action timerCallback;

    private Action timerElapsed;

    // Timer controller
    [SerializeField] private float timer;

    private float originalTime;

    public void SetTimer (float value, Action timerCallback, Action timerElapsed)
    {
        // Intializing timer
        this.timer = originalTime = value;

        // Initializing callback for action
        this.timerCallback = timerCallback;

        this.timerElapsed = timerElapsed;
    }

    private void Update ()
    {
        
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            timerElapsed();
            if (IsTimerComplete())
            {
                timerCallback();
            }
        }
    }

    public bool IsTimerComplete ()
    {
        return (timer <= 0f);
    }

    public float TimeElapsed ()
    {
        return 1f - timer / originalTime;
    }
}
