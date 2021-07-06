using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOpenDoor : MonoBehaviour, IInteractable
{
    private GameObject player;
    private GameObject chave;
    private TooltipController chavecollider;
    private bool haschave = false;

    private EnemySpawnManager enemies;
    private bool[] boss;
    
    [SerializeField] private string nomechave;
    [SerializeField] private float positionX;
    [SerializeField] private float positionY;
    [SerializeField] private float positionZ;
    private void Awake() 
    {
        player = GameObject.Find("Player");
        chave = GameObject.Find(""+nomechave+"");
        chavecollider = GetComponent<TooltipController>();    
    }
    
    public void OnInteraction ()
    {
       Vector3 position;
        position.x = positionX;
        position.y = positionY;
        position.z = positionZ;

        player.transform.position = position;
    }

    public void PegarChave ()
    {
       chavecollider.enabled = false;
       chave.SetActive(false);
       haschave = true;
       
       Debug.Log(haschave);
    }

    public void EntrarPortaTrancada ()
    {
        
       if(!haschave)
       {
           Debug.Log("trancado");
       }
       else
       {
            Debug.Log("Pode entrar");
            OnInteraction ();
       }
       
    }

 public void PortaBoss ()
    {
        enemies = GameObject.Find("Enemies").GetComponent<EnemySpawnManager>();
        boss = enemies.GetKilledList();
        for (int i = 0; i < boss.Length; ++i)
        {
            if(boss[2] == false)
            {
                Debug.Log("Boss foi morto");
                haschave = true;
            }
            
        }
        if(!haschave)
        {
           Debug.Log("trancado");
        }
        else
        {
            Debug.Log("Pode entrar");
            OnInteraction ();
        }
       
    }

}
