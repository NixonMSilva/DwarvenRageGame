using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falas : MonoBehaviour
{
    // Start is called before the first frame update
    private Collider[] colliderList;

    [SerializeField] string fala;

    private void Start ()
    {
        colliderList = GetComponentsInChildren<Collider>();
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioManager.instance.PlaySound(fala);
            foreach (Collider collider in colliderList)
            {
            collider.enabled = false;
            }
        }
    }

    public void Ler()
    {
        AudioManager.instance.PlaySound(fala);
    }
}