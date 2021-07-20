using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestPassLevel : MonoBehaviour
{
    public void EnterDungeon ()
    {
        SceneManager.LoadScene(2);
        AudioManager.instance.PlaySound("door_open");
    }

    public void BackMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Credito()
    {
        SceneManager.LoadScene(3);
    }

    public void Jogo()
    {
        SceneManager.LoadScene(1);
    }

}
