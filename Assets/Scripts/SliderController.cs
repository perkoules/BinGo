using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public TextMeshProUGUI textToChange, maxCoins;
    private Slider slider;
    public int coinsUsed;

    private void OnEnable()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = Convert.ToInt32(maxCoins.text) / 100f;
        slider.onValueChanged.AddListener(ChangeText);
    }

    private void ChangeText(float selectedValue)
    {
        textToChange.text = selectedValue.ToString("F2");
        coinsUsed = Convert.ToInt32(selectedValue * 100);
    }

    public void ResetSlider()
    {
        slider.value = slider.minValue;
    }
}