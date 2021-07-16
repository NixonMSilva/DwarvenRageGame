using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    [SerializeField] private Slider vslider;

    [SerializeField] private Slider sslider;

    [SerializeField] private Slider vmslider;
    
    public void ChangeVolume ()
    {
        GameManager.instance.sfxVolume = vslider.value;
    }

    public void ChangeSensitivity ()
    {
        InputHandler.instance.mouseSensitivity = sslider.value;
    }

    public void ChangeMusicVolume ()
    {
        AudioManager.instance.MusicVolume = vmslider.value;
    }
}
