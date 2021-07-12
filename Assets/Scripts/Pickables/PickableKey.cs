using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableKey : PickableBase
{
    public override void HandlePickUp()
    {
        Destroy (gameObject);
    }
}
