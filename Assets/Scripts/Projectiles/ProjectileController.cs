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

    [SerializeField] protected ProjectileCollision collisionSounds;
    [SerializeField] protected bool bleedOnImpact;

    protected GameObject caster;

    protected Vector3 target;

    protected Rigidbody rigidBody;
    
    private HashSet<IDamageable> damagedObjects = new HashSet<IDamageable>();

    private void Awake ()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    protected void Start ()
    {
        // Destruir o proj?til caso este se perca por 10 segundos
        Destroy(gameObject, 10f);
        damagedObjects.Clear();
    }

    public virtual void OnTriggerEnter (Collider other)
    {
        int layerId = other.gameObject.layer;
        string layerName = LayerMask.LayerToName(layerId);
        
        // Collide if it's on the collidable layer or it isn't the caster;
        if (_canCollideWith.Contains(layerName) && !other.gameObject.Equals(caster))
        {
            IDamageable target = other.gameObject.GetComponentInParent<IDamageable>();
            
            if (target != null && damagedObjects.Add(target))
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

                // If the player threw / fired the object, then show hitmark
                if (caster.CompareTag("Player"))
                {
                    UserInterfaceController.instance.ShowHitmark();
                }
            }
            PlayCollisionSound(other);
            Destroy(gameObject);
        }
    }

    protected void PlayCollisionSound (Collider other)
    {
        // Play sound referring to the proper hit, if applicable
        if (collisionSounds)
        {
            string soundName = collisionSounds.GetSoundForTag(other.tag);
            if (soundName != null)
            {
                AudioManager.instance.PlaySound(soundName);
            }
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
        Quaternion lookRotation = Quaternion.LookRotation(newTarget, Vector3.up);
        transform.rotation = lookRotation;
    }
}
