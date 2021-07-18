using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SlidersValue : MonoBehaviour
{
    // Start is called before the first frame update
    TextMeshProUGUI sliderValue;
    void Start()
    {
        sliderValue = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    public void textUpdate(float value)
    {
        Debug.Log(value);
        sliderValue.text = Mathf.RoundToInt(value)  + "";
    }
}
