using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveOnStart : MonoBehaviour
{
    private void Start ()
    {
        GameManager.instance?.SaveGame();
        Destroy(gameObject, 0.5f);
    }
}
