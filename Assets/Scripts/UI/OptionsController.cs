using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    [SerializeField] private Slider slider;
    
    public void ChangeVolume ()
    {
        GameManager.instance.sfxVolume = slider.value;
    }
}
