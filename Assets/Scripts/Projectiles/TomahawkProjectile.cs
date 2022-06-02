using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomahawkProjectile : ProjectileController
{
    [SerializeField] private float rotationRate = 25f;

    private Vector3 angularVelocity;

    private new void Start ()
    {
        // Destroy projectile after 10 seconds
        Destroy(gameObject, 10f);

        // Add an upwards diagonal force
        rigidBody.AddForce((transform.forward + transform.up) * 2f, ForceMode.Impulse);

        angularVelocity = new Vector3(rotationRate, 0f, 0f);
    }

    private void FixedUpdate ()
    {
        Rotate();
    }

    private void Rotate ()
    {
        Quaternion deltaRotation = Quaternion.Euler(angularVelocity * Time.fixedDeltaTime);
        rigidBody.MoveRotation(rigidBody.rotation * deltaRotation);
    }

    public override void FaceTowards (Vector3 origin, Vector3 newTarget)
    {
        //Debug.Log(newTarget);
        transform.LookAt(newTarget);
    }
}
