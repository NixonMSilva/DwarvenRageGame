using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float flightSpeed = 25f;

    [SerializeField] private List<string> _canCollideWith = new List<string>();

    [SerializeField] private float damageValue;

    [SerializeField] private DamageType damageType = DamageType.ranged;

    private GameObject caster;

    private Vector3 target;

    protected Rigidbody rigidBody;

    private void Awake ()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Start ()
    {
        // Destruir o projétil caso este se perca por 10
        Destroy(gameObject, 10f);
    }

    private void OnTriggerEnter (Collider other)
    {
        int layerId = other.gameObject.layer;
        string layerName = LayerMask.LayerToName(layerId);
        
        // Collide if it's on the collidable layer or it isn't the caster;
        if (_canCollideWith.Contains(layerName) && !other.gameObject.Equals(caster))
        {
            IDamageable target;
            if (other.gameObject.TryGetComponent<IDamageable>(out target))
            {
                target.TakeDamage(damageValue, damageType);
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
