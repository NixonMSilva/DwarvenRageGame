using System.Collections;
using UnityEngine;

public static class RagdollStartProcessor
{
    public static void ApplyRagdollEffect (this Rigidbody centerOfMass, RagdollStartType type) 
    { 
        switch (type)
        {
            case RagdollStartType.fallSideways:
                centerOfMass.AddForce((centerOfMass.gameObject.transform.forward + Vector3.left) * 15f, ForceMode.Impulse);
                Debug.Log("Falling sideways! " + centerOfMass.gameObject.name);
                break;
            case RagdollStartType.backImpulse:
                centerOfMass.AddForce(centerOfMass.gameObject.transform.forward * 15f, ForceMode.Impulse);
                break;
            case RagdollStartType.none:
                break;
        }
    }
}