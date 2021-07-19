using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileController : MonoBehaviour
{
    [SerializeField] protected float flightSpeed = 25f;

    [SerializeField] protected List<string> _canCollideWith = new List<string>();

    [SerializeField] protected float damageValue;

    [SerializeField] protected DamageType damageType = DamageType.ranged;
    [SerializeField] protected Effect effect = null;

    [SerializeField] protected bool bleedOnImpact;

    protected GameObject caster;

    protected Vector3 target;

    protected Rigidbody rigidBody;

    private void Awake ()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    protected void Start ()
    {
        // Destruir o projétil caso este se perca por 10 segundos
        Destroy(gameObject, 10f);
    }

    public virtual void OnTriggerEnter (Collider other)
    {
        int layerId = other.gameObject.layer;
        string layerName = LayerMask.LayerToName(layerId);
        
        // Collide if it's on the collidable layer or it isn't the caster;
        if (_canCollideWith.Contains(layerName) && !other.gameObject.Equals(caster))
        {
            IDamageable target;
            if (other.gameObject.TryGetComponent<IDamageable>(out target))
            {
                // Check if the target isn't blocking
                target.CheckForBlock(transform);
                
                if (effect != null)
                {
                    target.TakeDamage(damageValue, damageType, effect);
                }
                else
                {
                    target.TakeDamage(damageValue, damageType);
                }

                // Make the target bleed if possible
                if (bleedOnImpact)
                {
                    target.SpawnBlood(transform.position);
                }
            }
            Destroy(gameObject);
        }
    }

    public void SetCaster (GameObject obj)
    {
        caster = obj;
    }

    public void SetTarget (Vector3 newTarget)
    {
        target = newTarget.normalized;
        transform.LookAt(target);
        rigidBody.velocity = target * flightSpeed;
    }

    public virtual void FaceTowards (Vector3 origin, Vector3 newTarget)
    {
        transform.LookAt(newTarget - origin);
    }
}
