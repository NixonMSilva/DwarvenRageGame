using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionOnTimer : MonoBehaviour 
{
    
    // Action declaration
    private Action timerCallback;

    // Timer controller
    [SerializeField] private float timer;

    public void SetTimer (float value, Action timerCallback)
    {
        // Intializing timer
        this.timer = value;

        // Initializing callback for action
        this.timerCallback = timerCallback;
    }

    private void Update() 
    {
        if (timer > 0f )
        {
            timer -= Time.deltaTime;
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
}