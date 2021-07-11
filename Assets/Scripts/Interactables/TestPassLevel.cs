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
}
