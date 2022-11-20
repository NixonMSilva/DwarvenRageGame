using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsController : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject _displayTab;
    [SerializeField] private GameObject _audioTab;
    [SerializeField] private GameObject _controlsTab;

    [SerializeField] private Slider _audioMusicVolumeSlider;
    [SerializeField] private Slider _audioSfxVolumeSlider;
    [SerializeField] private Slider _audioVoiceoverVolumeSlider;

    [SerializeField] private Slider _controlsMouseSensitivitySlider;
    [SerializeField] private Toggle _invertLookToggle;

    [SerializeField] private TMP_Dropdown _resolutionsDropDown;
    [SerializeField] private Toggle _fullscreenToggle;
    [SerializeField] private Toggle _subtitlesToggle;

    private Resolution[] _resolutions;

    #endregion

    #region Unity

    private void Start ()
    {
        GetResolutionsData();
    }

    #endregion

    #region Display

    public void SetQualitySettings (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);

        string[] names = QualitySettings.names;
        Debug.Log("Current setting: " + names[qualityIndex]);
    }

    public void SetFullscreen ()
    {
        Screen.fullScreen = !Screen.fullScreen;
        Debug.Log("Fullscreen status: " + Screen.fullScreen);
    }

    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    private void GetResolutionsData ()
    {
        _resolutions = Screen.resolutions;
        _resolutionsDropDown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < _resolutions.Length; ++i)
        {
            string option = _resolutions[i].width + "x" + _resolutions[i].height;
            options.Add(option);

            if (_resolutions[i].width == Screen.currentResolution.width &&
                _resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        _resolutionsDropDown.AddOptions(options);
        _resolutionsDropDown.value = currentResolutionIndex;
        _resolutionsDropDown.RefreshShownValue();
    }

    public void SetSubtitles ()
    {
        GameManager.instance.showSubtitles = !GameManager.instance.showSubtitles;
        Debug.Log("Subtitle status: " + GameManager.instance.showSubtitles);
    }

    private void UpdateDisplayStatus ()
    {
        _fullscreenToggle.isOn = Screen.fullScreen;
    }

    #endregion

    #region Audio

    public void ChangeMusicVolume (float value)
    {
        // Change the volume for music
        AudioManager.instance.MusicVolume = _audioMusicVolumeSlider.value;
    }

    public void ChangeSfxVolume (float value)
    {
        // Change the volume for sound effects
        GameManager.instance.sfxVolume = _audioSfxVolumeSlider.value;
    }

    public void ChangeVoiceoverVolume (float value)
    {
        // Change the volume for voiceovers
        GameManager.instance.voiceoverVolume = _audioVoiceoverVolumeSlider.value;
    }

    private void UpdateAudioSliders ()
    {
        // Changes the sliders to their values based on global variables
        _audioSfxVolumeSlider.value = GameManager.instance.sfxVolume;
        _audioMusicVolumeSlider.value = AudioManager.instance.MusicVolume;
        _audioVoiceoverVolumeSlider.value = GameManager.instance.voiceoverVolume;
    }

    #endregion

    #region Controls

    public void ChangeMouseSensitivity (float value)
    {
        // Change the mouse look sensitivity
        InputHandler.instance.mouseSensitivity = _controlsMouseSensitivitySlider.value;
    }

    public void SetMouseInvertY (bool value)
    {
        // Change the mouse Y inversion
        InputHandler.instance.isLookYInverted = value;
    }

    private void UpdateControlSliders ()
    {
        // Changes the sliders to their values based on global variables
        _controlsMouseSensitivitySlider.value = InputHandler.instance.mouseSensitivity;
    }

    #endregion

    #region Tabs
    public void ChangeToDisplayTab ()
    {
        _displayTab.SetActive(true);
        _audioTab.SetActive(false);
        _controlsTab.SetActive(false);

        UpdateDisplayStatus();
    }

    public void ChangeToAudioTab ()
    {
        _audioTab.SetActive(true);
        _displayTab.SetActive(false);
        _controlsTab.SetActive(false);

        UpdateAudioSliders();
    }

    public void ChangeToControlsTab ()
    {
        _controlsTab.SetActive(true);
        _displayTab.SetActive(false);
        _audioTab.SetActive(false);

        UpdateControlSliders();
    }

    #endregion
}