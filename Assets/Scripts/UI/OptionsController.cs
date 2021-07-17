using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    [SerializeField] private Slider vslider;

    [SerializeField] private Slider sslider;

    [SerializeField] private Slider vmslider;

    public void Start ()
    {
        // Changes the sliders to their values based on global variables
        vslider.value = GameManager.instance.sfxVolume;
        sslider.value = InputHandler.instance.mouseSensitivity;
        vmslider.value = AudioManager.instance.MusicVolume;
    }

    public void ChangeVolume ()
    {
        // Change the volume for sound effects
        GameManager.instance.sfxVolume = vslider.value;
    }

    public void ChangeSensitivity ()
    {
        // Change the mouse look sensitivity
        InputHandler.instance.mouseSensitivity = sslider.value;
    }

    public void ChangeMusicVolume ()
    {
        // Change the volume for music
        AudioManager.instance.MusicVolume = vmslider.value;
    }
}
