using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    [SerializeField] private Slider sfxVolumeSlider;

    [SerializeField] private Slider musicVolumeSlider;

    [SerializeField] private Slider mouseSensitivitySlider;
    

    public void Start ()
    {
        // Changes the sliders to their values based on global variables
        sfxVolumeSlider.value = GameManager.instance.sfxVolume;
        musicVolumeSlider.value = AudioManager.instance.MusicVolume;
        mouseSensitivitySlider.value = InputHandler.instance.mouseSensitivity;
    }

    public void ChangeSfxVolume ()
    {
        // Change the volume for sound effects
        GameManager.instance.sfxVolume = sfxVolumeSlider.value;
    }

    public void ChangeMusicVolume ()
    {
        // Change the volume for music
        AudioManager.instance.MusicVolume = musicVolumeSlider.value;
    }

    public void ChangeSensitivity ()
    {
        // Change the mouse look sensitivity
        InputHandler.instance.mouseSensitivity = mouseSensitivitySlider.value;
    }
}
