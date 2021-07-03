using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage (float value);

    void TakeElementalDamage (float value, string type);

    void PlayImpactSound ();
}
