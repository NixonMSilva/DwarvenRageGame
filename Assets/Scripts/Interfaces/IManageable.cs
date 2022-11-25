using System;
using UnityEngine;

public interface IManageable
{
    public int UniqueId { get; set; }

    public GameObject AttachedObject { get; }

    public event Action<int> OnStatusChange;

    public void DestroyObject ();   
}
