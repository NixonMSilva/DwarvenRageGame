using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{ 
    void TakeDamage (float value, DamageType type);

    void TakeDamage (float value, DamageType type, Effect effect);

    void PlayImpactSound ();

    void SpawnBlood (Vector3 position);

    void SpawnBlood (Transform position);
}
