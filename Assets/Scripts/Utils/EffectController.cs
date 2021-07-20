using System;
using UnityEngine;

public class EffectController : MonoBehaviour 
{
    
    // Action declaration
    private Action timerCallback;
    private Action onUpdate;
    
    // Percentage count
    private float percentage;

    // Timer controller
    [SerializeField] private float timer;
    private float totalTimer;

    public float Percentage
    {
        get => percentage;
    }
    
    public void SetTimer (float value, Action timerCallback, Action onUpdate)
    {
        // Intializing timer
        this.timer = value;
        this.totalTimer = value;
        percentage = 1f;

        // Initializing callback for action
        this.timerCallback = timerCallback;
        this.onUpdate = onUpdate;
    }

    private void Update() 
    {
        if (timer > 0f)
        {
            percentage = (timer / totalTimer);
            timer -= Time.deltaTime;
            if (IsTimerComplete())
            {
                percentage = 0f;
                timerCallback();
            }
        }
        onUpdate();
    }

    public bool IsTimerComplete () 
    {
        return (timer <= 0f);
    }
}