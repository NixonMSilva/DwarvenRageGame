using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestInteractable : MonoBehaviour, IInteractable
{
    SaveController manager;
    [SerializeField] private string fala;
    private int aleatorio;
    private void Awake() 
    {
        manager = GameObject.Find("GameManager").GetComponent<SaveController>();
    }

    public void OnInteraction ()
    {
        manager.SaveGame();
        Debug.Log("Game saved!");
    }

    public void Conversar ()
    {
        AudioManager.instance.DestroyAllSounds();
        aleatorio = Random.Range(1, 3);
        switch(aleatorio)
        {
            case 1:
            AudioManager.instance.PlaySound("bode");
            break;
            case 2:
            AudioManager.instance.PlaySound("bode1");
            break;
        }
        
    }

    public void Interagir()
    {
        AudioManager.instance.DestroyAllSounds();
        AudioManager.instance.PlaySound(fala);
    }

    public void Salvar()
    {
        StartCoroutine(Fim());
    }

    IEnumerator Fim()
    {
        AudioManager.instance.DestroyAllSounds();
        AudioManager.instance.PlaySound(fala);
       
        yield return new WaitForSeconds(22); 

        SceneManager.LoadScene(6);
    }
}
