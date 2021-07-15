using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    [SerializeField] private Slider vslider;

    [SerializeField] private Slider sslider;
    
    public void ChangeVolume ()
    {
        GameManager.instance.sfxVolume = vslider.value;
    }

    public void ChangeSensitivity ()
    {
        InputHandler.instance.mouseSensitivity = sslider.value;
    }
}
