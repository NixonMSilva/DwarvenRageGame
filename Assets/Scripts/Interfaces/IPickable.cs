using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickable
{
    void OnTriggerEnter (Collider other);

    void HandlePickUp ();

    event Action<IPickable> OnPickUp;

    string GetName ();

}
