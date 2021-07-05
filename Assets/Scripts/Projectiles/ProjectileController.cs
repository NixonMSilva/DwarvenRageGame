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

    private Vector3 target;

    private Rigidbody rigidBody;

    private void Awake ()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Start ()
    {
        // Destruir o proj�til caso este se perca por 10
        Destroy(gameObject, 10f);
    }

    private void Update ()
    {
        // Atualizar a velocidade conforme a dire��o e a velocidade padr�o
        //rigidBody.MovePosition(transform.position + target * flightSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter (Collider other)
    {
        int layerId = other.gameObject.layer;
        string layerName = LayerMask.LayerToName(layerId);
        // Realizar as colis�es
        if (_canCollideWith.Contains(layerName))
        {
            IDamageable target;
            if (other.gameObject.TryGetComponent<IDamageable>(out target))
            {
                target.TakeDamage(damageValue, damageType);
            }
            Destroy(gameObject);
        }
    }

    public void SetTarget (Vector3 newTarget)
    {
        target = newTarget.normalized;
        transform.LookAt(target);
        rigidBody.velocity = target * flightSpeed;
    }

    public void FaceTowards (Vector3 origin, Vector3 newTarget)
    {
        transform.LookAt(newTarget - origin);
    }
}
