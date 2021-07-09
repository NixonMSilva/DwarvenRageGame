using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOTController : MonoBehaviour 
{
    // Action declaration
    private Action OnEnd;
    private Action OnTick;

    // Timer controller
    [SerializeField] private float timer;
    [SerializeField] private float tick;

    private float originalTime;
    private float currentTick;

    [SerializeField] private DamageType type;

    public DamageType Type
    {
        get { return type; }
    }

    public void SetTimer (DamageType type, float value, float tickTime, Action onEnd, Action onTick)
    {
        // Intializing timer
        timer = originalTime = value;
        tick = currentTick = tickTime;

        this.type = type;

        // Initializing callback for action
        OnEnd = onEnd;
        OnTick = onTick;
    }

    private void Update() 
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            tick -= Time.deltaTime;
            if (IsTickComplete())
            {
                tick = currentTick;
                OnTick();
            }
            if (IsTimerComplete())
            {
                OnEnd();
            }
        }
    }

    public bool IsTimerComplete () 
    {
        return (timer <= 0f);
    }

    public bool IsTickComplete ()
    {
        return (tick <= 0f);
    }

    public void ResetTimer ()
    {
        timer = originalTime;
    }
}