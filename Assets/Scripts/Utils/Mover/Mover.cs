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

    private Action completeCallback;
    
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
            completeCallback.Invoke();
    }

    public void MoveTo (Vector3 nextDestination, Action onComplete = null)
    {
        float distanceToFinish = Vector3.Distance(transform.position, nextDestination);
        totalMoveDuration = distanceToFinish / moveSpeed;

        startPosition = transform.position;
        destination = nextDestination;
        elapsedMoveDuration = 0f;
        completeCallback = onComplete;
    }

    private Vector3 Interpolate (Vector3 start, Vector3 end, float percentage) => Vector3.Lerp(start, end, percentage);
}
