using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private Vector3? destination;
    private Vector3 startPosition;

    private float totalMoveDuration;
    private float elapsedMoveDuration;

    private Action OnCompleteCallback;
    
    private void Update ()
    {
        if (!destination.HasValue)
            return;

        if (elapsedMoveDuration >= totalMoveDuration && totalMoveDuration > 0)
            return;

        elapsedMoveDuration += Time.deltaTime;
        float movePercentage = elapsedMoveDuration / totalMoveDuration;

        transform.position = Interpolate(startPosition, destination.Value, movePercentage);

        if (elapsedMoveDuration >= totalMoveDuration)
            OnCompleteCallback.Invoke();
    }

    public void MoveTo (Vector3 destination, Action onComplete = null)
    {
        var distanceToFinish = Vector3.Distance(transform.position, destination);
        totalMoveDuration = distanceToFinish / moveSpeed;

        startPosition = transform.position;
        this.destination = destination;
        elapsedMoveDuration = 0f;
        OnCompleteCallback = onComplete;
    }

    private Vector3 Interpolate (Vector3 start, Vector3 end, float percentage)
    {
        return Vector3.Lerp(start, end, percentage);
    }

}
