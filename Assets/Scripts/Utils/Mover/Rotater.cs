using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    [SerializeField] private float angularSpeed;

    private Quaternion? destination;
    private Quaternion startRotation;

    [SerializeField] private float totalMoveDuration;
    [SerializeField] private float elapsedMoveDuration;

    private Action completeCallback;
    
    private void Update ()
    {
        if (!destination.HasValue)
            return;

        if (elapsedMoveDuration >= totalMoveDuration && totalMoveDuration > 0)
            return;

        elapsedMoveDuration += Time.deltaTime;
        float movePercentage = elapsedMoveDuration / totalMoveDuration;

        transform.rotation = Interpolate(startRotation, destination.Value, movePercentage);

        if (elapsedMoveDuration >= totalMoveDuration)
        {
            completeCallback.Invoke();
            ResetTarget();
        }
    }

    public void MoveTo (Quaternion nextRotation, Action onComplete = null)
    {
        float distanceToFinish = Quaternion.Angle(transform.rotation,nextRotation);
        totalMoveDuration = distanceToFinish / angularSpeed;

        startRotation = transform.rotation;
        destination = nextRotation;
        elapsedMoveDuration = 0f;
        completeCallback = onComplete;
    }

    public void MoveTo (Quaternion nextRotation, bool onlyHorizontal, Action onComplete = null)
    {
        if (onlyHorizontal)
        {
            nextRotation.eulerAngles = new Vector3(0f, nextRotation.eulerAngles.y, 0f);    
        }
        MoveTo(nextRotation, onComplete);
    }

    private void ResetTarget ()
    {
        destination = null;
    }

    private Quaternion Interpolate (Quaternion start, Quaternion end, float percentage) => Quaternion.Slerp(start, end, percentage);
}
