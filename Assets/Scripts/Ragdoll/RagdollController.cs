using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RagdollController : MonoBehaviour
{
    [SerializeField] Rigidbody[] _rbList;
    [SerializeField] Collider[] _colList;

    [SerializeField] Rigidbody _pelvis;

    private void Awake ()
    {
        FillBones();

        foreach (Rigidbody rb in _rbList)
            rb.isKinematic = true;

        foreach (Collider col in _colList)
            col.enabled = false;
    }

    public void StartRagdoll ()
    {
        foreach (Rigidbody rb in _rbList)
            rb.isKinematic = false;

        foreach (Collider col in _colList)
        {
            col.enabled = true;

            // Makes the target drop a weapon and destroys the weapon afterwards
            if (col.gameObject.CompareTag("DroppableWeapon"))
            {
                col.gameObject.transform.parent = null;
                var weaponFadeTimer = col.gameObject.AddComponent<ActionOnTimer>();
                weaponFadeTimer.SetTimer(10f, () =>
                {
                    Destroy(weaponFadeTimer.gameObject);
                });
            }
        }
    }

    public void ApplyForceToPelvis ()
    {
        _pelvis.AddForce(-transform.forward * 4f, ForceMode.Impulse);
    }

    [ContextMenu("Fill Bones")]
    void FillBones ()
    {
        _rbList = GetComponentsInChildren<Rigidbody>();
        // _rbList = rigidBodies.ToList();

        _colList = GetComponentsInChildren<Collider>();
        // _colList = colliders.ToList();
    }
}
