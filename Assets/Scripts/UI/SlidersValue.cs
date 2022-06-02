using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SlidersValue : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI sliderValueText;

    private void Start ()
    {
        TextUpdate();
    }

    public void TextUpdate ()
    {
        sliderValueText.text = Mathf.RoundToInt(((slider.value - slider.minValue) / (slider.maxValue - slider.minValue)) * 100).ToString();
    }
}
